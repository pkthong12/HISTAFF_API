using API.All.DbContexts;
using API.DTO;
using CORE.StaticConstant;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.DataValidation.Contracts;
using RegisterServicesWithReflection.Services.Base;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using System.Drawing;
using API.Socket;
using OfficeOpenXml.Style;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using CORE.DataContract;
using API.Controllers.HuOrganization;
using CORE.Services.File;
using CORE.GenericUOW;
using API.All.HRM.Profile.ProfileAPI.HuOrganization;
using System.Diagnostics;
using CORE.Extension;
using CORE.AutoMapper;
using Common.Extensions;
using API.All.Services;
using System.Dynamic;

namespace API.All.SYSTEM.CoreAPI.Xlsx
{

    public class MlsData
    {
        public string Key { get; set; } = null!;
        public string Vi { get; set; } = null!;
        public string En { get; set; } = null!;
    }

    [ScopedRegistration]
    public class XlsxRepository : IXlsxRepository
    {
        FullDbContext _dbContext;
        IOptions<AppSettings> _appSettingsOptions;
        AppSettings _appSettings;
        private readonly GenericUnitOfWork _uow;

        private List<MlsData> _mlsData;
        private List<SysOtherListTypeDTO>? _otherListTypes;
        private List<SysOtherListDTO>? _otherLists;
        private List<HuNationDTO>? _nations;
        private List<HuProvinceDTO>? _provinces;
        private List<HuDistrictDTO>? _districts;
        private List<HuWardDTO>? _wards;
        private List<HuPositionDTO>? _positions;
        private HuOrganizationRepository _huOrganizationRepository;
        private IFileService _fileService;
        private IWebHostEnvironment _env;
        private IEmailService _emailService;
        private int dataProjectionOffset = 500;

        private List<int> AccumulatedStepDuration = new() { 0, 10, 20, 50, 80, 90, 100 };

        private readonly IHubContext<SignalHub> _hubContext;

        public XlsxRepository(
            FullDbContext dbContext,
            IHubContext<SignalHub> hubContext,
            IOptions<AppSettings> options,
            IFileService fileService,
            IWebHostEnvironment env,
             IEmailService emailService
            )
        {
            _uow = new(dbContext);
            _dbContext = dbContext;
            _appSettingsOptions = options;
            _appSettings = options.Value;
            _env = env;
            _appSettings = options.Value;
            _fileService = fileService;
            _huOrganizationRepository = new(dbContext, env, options, fileService, emailService);
            _hubContext = hubContext;
            _emailService = emailService;

            _mlsData = _dbContext.SysLanguages.Select(x => new MlsData()
            {
                Key = x.KEY,
                Vi = x.VI,
                En = x.EN
            }).OrderBy(x => x.Key).ToList();

            _otherListTypes = _dbContext.SysOtherListTypes.Where(x => x.IS_ACTIVE ?? false).Select(x => new SysOtherListTypeDTO()
            {
                Id = x.ID,
                Code = x.CODE,
                Name = x.NAME,
            }).OrderBy(x => x.Name).ToList();

            _otherLists = _dbContext.SysOtherLists.Where(x => x.IS_ACTIVE ?? false).Select(x => new SysOtherListDTO()
            {
                Id = x.ID,
                Code = x.CODE,
                Name = x.NAME,
                TypeId = x.TYPE_ID,
            }).OrderBy(x => x.Name).ToList();

            _positions = _dbContext.HuPositions.Where(x => x.IS_ACTIVE ?? false).Select(x => new HuPositionDTO()
            {
                Id = x.ID,
                Code = x.CODE,
                Name = x.NAME,
            }).OrderBy(x => x.Name).ToList();

            /* Currently PRIMARY_KEY for HU_PROVINCE is NATION_ID referenced to SYS_OTHER_LIST instead of HU_NATION */
            /* But this is not well-designed. Check this Where caryfully when UAT */

            _nations = _dbContext.HuNations
                        .Select(x => new HuNationDTO()
                        {
                            Id = x.ID,
                            Code = x.CODE,
                            Name = x.NAME,
                        }).OrderBy(x => x.Name).ToList();

            _provinces = _dbContext.HuProvinces
                        .Select(x => new HuProvinceDTO()
                        {
                            Id = x.ID,
                            Code = x.CODE,
                            Name = x.NAME,
                            NationId = x.NATION_ID
                        }).OrderBy(x => x.Name).ToList();

            _districts = _dbContext.HuDistricts
                        .Select(x => new HuDistrictDTO()
                        {
                            Id = x.ID,
                            Code = x.CODE,
                            Name = x.NAME,
                            ProvinceId = x.PROVINCE_ID
                        }).OrderBy(x => x.ProvinceId).ThenBy(x => x.Name).ToList();

            _wards = _dbContext.HuWards
                        .Select(x => new HuWardDTO()
                        {
                            Id = x.ID,
                            Code = x.CODE,
                            Name = x.NAME,
                            DistrictId = x.DISTRICT_ID
                        }).OrderBy(x => x.DistrictId).ThenBy(x => x.Name).ToList();

        }

        private async void SendTaskProgressInfo(string username, string message, string outerMessage, string innerMessage, decimal innerProportion, int stepIndex)
        {
            // The IHubContext is for sending notifications to clients, it is not used to call methods on the Hub.
            var curValue = AccumulatedStepDuration[stepIndex];
            await _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
            {
                SignalType = "TASK_PROGRESS",
                Message = "APP ĐANG XỬ LÝ TÁC VỤ, XIN VUI LÒNG ĐỢI..."/*message*/,
                Data = new
                {
                    OuterMessage = outerMessage,
                    OuterPercent = curValue.ToString() + "%",
                    InnerMessage = innerMessage,
                    InnerPercent = Math.Round((innerProportion * 100), 0).ToString() + "%"
                }
            });
        }
        public async Task<MemoryStream> ExportCorePageListGridToExcel(ExportCorePageListGridToExcelDTO request, string location, string sid, string username)
        {
            try
            {
                // Check request.Columns for duplicat keys
                var groupBy = request.Columns.GroupBy(x => x.Field).Select(x => new { Field = x.Key, FieldCount = x.Count() });
                var tryFind = groupBy.FirstOrDefault(x => x.FieldCount > 1);



                if (tryFind != null)
                {
                    await _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                    {
                        SignalType = "TASK_PROGRESS",
                        Message = "APP ĐANG XỬ LÝ TÁC VỤ, XIN VUI LÒNG ĐỢI..."/*message*/,
                        Data = new
                        {
                            OuterMessage = CommonMessageCode.XLSX_COLUMN_FIELD_CAN_NOT_BE_DUPLICATED + " (" + tryFind.Field + ")",
                            OuterPercent = "100%",
                            InnerMessage = "...",
                            InnerPercent = "100%"
                        },
                        Error = true
                    });
                    throw new Exception(CommonMessageCode.XLSX_COLUMN_FIELD_CAN_NOT_BE_DUPLICATED + " (" + tryFind.Field + ")");
                }

                string templatePath = Path.Combine(location, "BlankWorkbook.xlsx");
                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }
                FileInfo file = new(templatePath);
                using (ExcelPackage package = new(file))
                {
                    package.Compatibility.IsWorksheets1Based = false;
                    ExcelWorkbook wb = package.Workbook;
                    ExcelWorksheets worksheets = wb.Worksheets;
                    var ws = worksheets[0];

                    var visibleColumns = request.Columns.Where(x => x.Hidden != true).ToList();

                    var colIndex = 1;
                    visibleColumns.ForEach(c =>
                    {
                        var tran = _mlsData.SingleOrDefault(x => x.Key == c.Caption);
                        ws.Cells[1, colIndex].Value = tran != null ? tran.Vi : c.Caption;
                        colIndex++;
                    });

                    var rowCount = request.Data.Count;
                    var columnCount = visibleColumns.Count;
                    var rowIndex = 2;
                    request.Data.ForEach(async item =>
                    {
                        await _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                        {
                            SignalType = "TASK_PROGRESS",
                            Message = "APP ĐANG XỬ LÝ TÁC VỤ, XIN VUI LÒNG ĐỢI..."/*message*/,
                            Data = new
                            {
                                OuterMessage = (rowIndex - 1).ToString(),
                                OuterPercent = $"{decimal.Parse((rowIndex - 1).ToString()) / decimal.Parse(rowCount.ToString()) * 100}%",
                                InnerMessage = "...",
                                InnerPercent = "100%"
                            }
                        });
                        var row = new ExpandoObject() as IDictionary<string, object?>;
                        var colIndex = 1;
                        foreach (KeyValuePair<string, object?> keyValuePair in item)
                        {
                            if (visibleColumns.SingleOrDefault(x => x.Field == keyValuePair.Key) != null)
                            {
                                Trace.WriteLine($"{keyValuePair.Key}: ${keyValuePair.Value}");
                                ws.Cells[rowIndex, colIndex].Value = keyValuePair.Value;

                                if (visibleColumns[colIndex - 1].Type == "date" || visibleColumns[colIndex - 1].Pipe == "DATE")
                                {
                                    ws.Cells[rowIndex, colIndex].Style.Numberformat.Format = "dd-mm-yyyy";
                                }
                                else if (visibleColumns[colIndex - 1].Type == "time" || visibleColumns[colIndex - 1].Pipe == "TIME_HHMM")
                                {
                                    ws.Cells[rowIndex, colIndex].Style.Numberformat.Format = "hh:MM:ss";
                                }
                                else if (visibleColumns[colIndex - 1].Type == "formated_decimal")
                                {
                                    ws.Cells[rowIndex, colIndex].Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \"-\"??_);_(@_)";
                                }
                                else if (visibleColumns[colIndex - 1].Type == "formated_money")
                                {
                                    ws.Cells[rowIndex, colIndex].Style.Numberformat.Format = "###,###,###0";
                                }
                            }
                            colIndex++;
                        }
                        rowIndex++;
                    });

                    var headerRangeAddress = ExcelCellBase.GetAddress(1, 1, 1, columnCount);
                    var range = ws.Cells[headerRangeAddress];
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#358ccb"));
                    range.Style.Font.Color.SetColor(ColorTranslator.FromHtml("white"));
                    range.Style.Font.Bold = true;


                    var overallRangeAddress = ExcelCellBase.GetAddress(1, 1, rowCount + 1, columnCount);
                    var overallRange = ws.Cells[overallRangeAddress];
                    overallRange.AutoFitColumns();
                    overallRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    overallRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    overallRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    overallRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    overallRange.Style.Border.Top.Color.SetColor(Color.Gray);
                    overallRange.Style.Border.Right.Color.SetColor(Color.Gray);
                    overallRange.Style.Border.Bottom.Color.SetColor(Color.Gray);
                    overallRange.Style.Border.Left.Color.SetColor(Color.Gray);

                    var stream = new MemoryStream(package.GetAsByteArray());

                    return await Task.Run(() => stream);

                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MemoryStream> DownloadTemplate(DownloadTemplateDTO request, string location, string sid)
        {

            try
            {
                string templatePath = Path.Combine(location, request.ExCode);
                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }
                FileInfo file = new(templatePath);
                using (ExcelPackage package = new(file))
                {
                    package.Compatibility.IsWorksheets1Based = false;
                    ExcelWorkbook wb = package.Workbook;
                    ExcelWorksheets worksheets = wb.Worksheets;
                    if (worksheets.Count > 0)
                    {
                        var ws = worksheets[0];
                        // delete all existing validations
                        ws.DataValidations.Clear();

                        List<string> row2 = new();
                        List<string> row4 = new();
                        List<string> row5 = new();

                        int columnIndex = 1;
                        while (ws.Cells[2, columnIndex].Value != null)
                        {
                            row2.Add(ws.Cells[2, columnIndex].Value.ToString() ?? "");
                            row4.Add(ws.Cells[4, columnIndex].Value.ToString() ?? "");
                            row5.Add(ws.Cells[5, columnIndex].Value.ToString() ?? "");

                            columnIndex++;
                        }
                        int columnIndexMax = columnIndex--;

                        // test add range based on the 5th row
                        columnIndex = 1;
                        while (ws.Cells[2, columnIndex].Value != null)
                        {
                            // add validation
                            var colAddress = ExcelCellBase.GetAddress(6, columnIndex, 200, columnIndex, true);
                            var litsAddress = ExcelCellBase.GetAddress(5, 1, 5, 136, true);
                            IExcelDataValidationList val = ws.DataValidations.AddListValidation(colAddress);

                            val.Formula.ExcelFormula = litsAddress;

                            columnIndex++;
                        }

                        var stream = new MemoryStream(package.GetAsByteArray());

                        return await Task.Run(() => stream);

                    }
                    else
                    {
                        throw new Exception(CommonMessageCode.THE_BOOK_CONTAINS_NO_SHEET);
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MemoryStream> GenerateTemplate(ExObject request, string location, string sid, string username)
        {
            try
            {
                if (_otherListTypes == null)
                {
                    throw new Exception(CommonMessageCode.ACTIVE_SYS_OTHER_LIST_TYPE_QUERY_WAS_NULL);
                }

                if (_otherLists == null)
                {
                    throw new Exception(CommonMessageCode.ACTIVE_SYS_OTHER_LIST_TYPE_QUERY_WAS_NULL);
                }

                // load configuraion
                var jsonFile = Path.Combine(location, request.ExCode + ".json");
                var jsonConfigurationString = File.ReadAllText(jsonFile);
                if (jsonConfigurationString == null) throw new Exception($"Could not read configuration file {jsonFile}");
                ExFile[]? exFile = JsonDocument.Parse(jsonConfigurationString).RootElement.GetProperty("ExFiles").Deserialize<ExFile[]>();
                var config = exFile?.Where(x => x.ExCode == request.ExCode).FirstOrDefault() ?? throw new Exception(CommonMessageCode.CAN_NOT_FIND_CONFIG_FOR_XLSX_FILE + " " + request.ExCode);
                if (config == null) throw new Exception($"Could not read config from json file for code {request.ExCode}");
                var dataStartRow = config.DataStartRow;
                var rowsToPrepare = config.RowsToPrepare;
                var exDirectReferences = config.ExDirectReferences;
                var exConsequentReferences = config.ExConsequentReferences;
                var exIndirectReferences = config.ExIndirectReferences;

                string blankWorkbookPath = Path.Combine(location, "BlankWorkbook.xlsx");
                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }
                FileInfo file = new(blankWorkbookPath);
                var package = new ExcelPackage(file) ?? throw new Exception(CommonMessageCode.CAN_NOT_OPEN_BLANK_WORKBOOK); ;
                package.Compatibility.IsWorksheets1Based = false;

                ExcelWorkbook wb = package.Workbook;
                ExcelWorksheets worksheets = wb.Worksheets;



                /* TẠO CÁC VÙNG THAM CHIẾU VÀ TÌM KIẾM LIÊN QUAN TỚI BẢNG SYS_OTHER_LIST */
                /*************************************************************************/
                // STEP = 1;
                if (config.RenderOtherListReferences == true)
                {
                    RenderOtherListReferences(ref package, ref wb, username);
                }
                /*************************************************************************/



                /* TẠO CÁC VÙNG THAM CHIẾU VÀ TÌM KIẾM LIÊN QUAN TỚI CÁC BẢNG TỈNH - HUYỆN - XÃ */
                /********************************************************************************/
                // STEP = 2;
                if (config.RenderAdministrativePlaces == true)
                {
                    RenderAdministrativePlaces(ref package, ref wb, username);
                }
                /********************************************************************************/



                /* TẠO CÁC VÙNG THAM CHIẾU VÀ TÌM KIẾM TRỰC TIẾP */
                /*************************************************/
                // STEP = 3;
                if (exDirectReferences != null)
                {
                    PrepareDirectReference(ref wb, ref exDirectReferences, username);
                }
                /*************************************************/


                /* TẠO CÁC VÙNG TÌM KIẾM NỐI TIẾP */
                /*************************************************/
                // STEP = 3A;
                if (exConsequentReferences != null)
                {
                    PrepareConsequentReference(ref wb, ref exConsequentReferences, username);
                }
                /*************************************************/


                /* TẠO CÁC VÙNG THAM CHIẾU VÀ TÌM KIẾM GIÁN TIẾP */
                /*************************************************/
                // STEP = 4;
                if (exIndirectReferences != null)
                {
                    PrepareIndirectReference(ref wb, ref exIndirectReferences, username);
                }
                /*************************************************/

                /* TẠO ORG_TREE */
                /*************************************************/
                // STEP = 5;
                if (config.BuildOrgTree == true)
                {
                    _huOrganizationRepository = new HuOrganizationRepository(_dbContext, _env, _appSettingsOptions, _fileService, _emailService);
                    List<HuOrganizationTreeBlockDTO> _treeData = await _huOrganizationRepository.BuildOrgTree(sid);
                    BuildOrgTreeAndPosition(_treeData, ref wb, username, 2);
                }
                /*************************************************/

                // Khởi tạo ws cho người dùng INPUT
                var ws = package.Workbook.Worksheets.Add("INPUT"); // Sheet cuối cùng

                /****************************************************************************************************************************************/
                /* TẠO CÁC DROPDOWN VÀ VLOOKUP CHO BẢNG CHÍNH       
                /****************************************************************************************************************************************/
                // STEP = 6;
                // Delete default sheet
                wb.Worksheets.Delete(wb.Worksheets[0]);

                var count = config.ExTables.Count;

                int colIndex = 1;


                for (var i = 0; i < count; i++)
                {
                    // Bảng entity cần dựng trong trang chính
                    var table = config?.ExTables[i] ?? throw new Exception($"Table was null at index {i}");

                    // Đánh dấu cột đầu tiên cho mỗi table
                    int startColumn = colIndex;

                    var entityType = _dbContext.Model.FindEntityType("API.Entities." + table?.Table);
                    if (entityType != null)
                    {
                        var tableName = entityType.Name.Split('.').Last();

                        List<ExColumnRule> columnRules = new();
                        List<string> renderColumns = new();
                        List<string> requiredColumns = new();

                        // detect if this property is in config
                        var filter = config?.ExTables.Where(x => x.Table == tableName).FirstOrDefault();
                        if (filter != null)
                        {
                            columnRules = filter.Rules;
                            renderColumns = filter.RenderColumns ?? new List<string>();
                            requiredColumns = filter.RequiredColumns ?? new List<string>();
                        }

                        if (renderColumns.Count == 0) throw new Exception($"RenderColumns was empty for table {tableName}");

                        var fullProperties = entityType.GetProperties();
                        var properties = fullProperties.Where(x => renderColumns.Contains(x.Name));


                        var checkNotInEntity = renderColumns.Where(x => fullProperties.All(m => m.Name != x)).FirstOrDefault();


                        if (checkNotInEntity != null)
                        {
                            // Nếu rule của cột này không chứa ConsequentReference => đẩy lỗi trả về
                            if (columnRules.Where(x => x.Column == checkNotInEntity!).FirstOrDefault()!.ConsequentReference == null)
                            {
                                throw new Exception($"Property {checkNotInEntity} not found in the entity class");
                            }
                        };


                        if (properties != null)
                        {

                            var propertyList = properties.OrderBy(x => renderColumns.IndexOf(x.Name)).ToList();

                            var propertyCount = propertyList.Count;
                            for (var j = 0; j < propertyCount; j++)
                            {
                                var property = propertyList[j];
                                var propertyName = property.Name;
                                bool required = requiredColumns.Any(x => x == propertyName);

                                Trace.WriteLine(propertyName, colIndex.ToString());

                                // find if the property needs a dropdown or a consequence
                                var rule = columnRules.SingleOrDefault(x => x.Column == propertyName);

                                if (rule != null)
                                {
                                    // find if dropdown is from SYS_OTHER_LIST

                                    if (rule.SysOtherListTypeCode != null)
                                    {
                                        FillTopRows(ref ws, ref entityType, ref property, colIndex, ref tableName, ref propertyName, addNote: true, request.Lang, required);
                                        AddDropDownFromSysOtherList(ref ws, ref dataStartRow, colIndex, ref rowsToPrepare, ref rule);
                                        FillTopRows(ref ws, ref entityType, ref property, colIndex + dataProjectionOffset, ref tableName, ref propertyName, addNote: false, request.Lang, required);
                                        AddNativeColumnWithVlookupFromSysOtherList(ref ws, ref dataStartRow, colIndex + dataProjectionOffset, ref rowsToPrepare, ref rule);
                                    }

                                    // find if dropdown is from DirectReference
                                    if (rule.DirectReference != null)
                                    {
                                        if (exDirectReferences != null)
                                        {
                                            FillTopRows(ref ws, ref entityType, ref property, colIndex, ref tableName, ref propertyName, addNote: true, request.Lang, required);
                                            AddDropDownFromDirectReference(ref ws, ref dataStartRow, colIndex, ref rowsToPrepare, ref rule);
                                            FillTopRows(ref ws, ref entityType, ref property, colIndex + dataProjectionOffset, ref tableName, ref propertyName, addNote: false, request.Lang, required);
                                            AddNativeColumnWithVlookupFromDirectReference(ref ws, ref dataStartRow, colIndex + dataProjectionOffset, ref rowsToPrepare, ref rule);
                                        }
                                        else
                                        {
                                            throw new Exception(CommonMessageCode.NO_EX_DIRECT_REFFERENCE_FOUND);
                                        }
                                    }


                                    /*
                                     BREAKING CHANGE
                                     */
                                    // find if column is in ConsequentReference


                                    if (rule.ConsequentReference != null)
                                    {
                                        if (exConsequentReferences != null)
                                        {
                                            FillTopRows(ref ws, ref entityType, ref property, colIndex, ref tableName, ref propertyName, addNote: true, request.Lang, required);
                                            AddReadonlyColumnWithVlookupFromConsequentReference(ref ws, ref dataStartRow, colIndex, ref rowsToPrepare, ref rule);
                                        }
                                        else
                                        {
                                            throw new Exception(CommonMessageCode.NO_EX_CONSEQUENT_REFFERENCE_FOUND);
                                        }
                                    }

                                    // find if dropdown is from IndirectReference
                                    if (rule.IndirectReference != null)
                                    {
                                        if (exIndirectReferences != null)
                                        {
                                            FillTopRows(ref ws, ref entityType, ref property, colIndex, ref tableName, ref propertyName, addNote: true, request.Lang, required);
                                            AddIndirectReference(ref ws, ref dataStartRow, colIndex, ref rowsToPrepare, ref rule, ref exIndirectReferences, table!.Table, startColumn);
                                            FillTopRows(ref ws, ref entityType, ref property, colIndex + dataProjectionOffset, ref tableName, ref propertyName, addNote: false, request.Lang, required);
                                            AddNativeColumnWithVlookupFromIndirectReference(ref ws, ref dataStartRow, colIndex + dataProjectionOffset, ref rowsToPrepare, ref rule, ref exIndirectReferences);
                                        }
                                        else
                                        {
                                            throw new Exception(CommonMessageCode.NO_EX_INDIRECT_REFFERENCE_FOUND);
                                        }
                                    }

                                    // find if dropdown is from OrgTree
                                    if (rule.OrgTreeReference == true)
                                    {
                                        FillTopRows(ref ws, ref entityType, ref property, colIndex, ref tableName, ref propertyName, addNote: true, request.Lang, required);
                                        AddDropDownFromOrgTree(ref ws, ref dataStartRow, colIndex, ref rowsToPrepare);
                                        FillTopRows(ref ws, ref entityType, ref property, colIndex + dataProjectionOffset, ref tableName, ref propertyName, addNote: false, request.Lang, required);
                                        AddNativeColumnWithVlookupFromOrgTree(ref ws, ref dataStartRow, colIndex + dataProjectionOffset, ref rowsToPrepare);
                                    }

                                    // if column is of type DateTime/DateTime? and requires Time
                                    if (rule.TimeRequired == true)
                                    {
                                        FillTopRows(ref ws, ref entityType, ref property, colIndex, ref tableName, ref propertyName, addNote: true, request.Lang, required, timeRequired: true);
                                        FillTopRows(ref ws, ref entityType, ref property, colIndex + dataProjectionOffset, ref tableName, ref propertyName, addNote: false, request.Lang, required);
                                        AddSimpleLinkedFormula(ref ws, ref dataStartRow, colIndex + dataProjectionOffset, ref rowsToPrepare);
                                    }

                                }
                                else
                                {
                                    FillTopRows(ref ws, ref entityType, ref property, colIndex, ref tableName, ref propertyName, addNote: true, request.Lang, required);
                                    FillTopRows(ref ws, ref entityType, ref property, colIndex + dataProjectionOffset, ref tableName, ref propertyName, addNote: false, request.Lang, required);
                                    AddSimpleLinkedFormula(ref ws, ref dataStartRow, colIndex + dataProjectionOffset, ref rowsToPrepare);
                                }
                                colIndex++;
                            }

                        }
                        else
                        {
                            throw new Exception(CommonMessageCode.NO_PROPERTY_FOUND_IN_THE_ENTITY_TYPE + " " + nameof(entityType));
                        }
                    }
                    else
                    {
                        throw new Exception(CommonMessageCode.NO_ENTITY_TYPE_FOUND_FOR_THE_GIVEN_NAME + " " + table?.Table ?? "");
                    }

                    /* TẠO VÙNG THAM CHIẾU CHO HÀM MATCH ĐỂ TRUY VẤN VỊ TRÍ CỘT */
                    /************************************************************/
                    SendTaskProgressInfo(username, "PrepareColumnMatchReference", "", "", 0, 4);
                    if (exIndirectReferences != null)
                    {
                        // (colIndex - 1) = cột cuối cùng của ExTable hiện tại
                        PrepareColumnMatchReference(ref wb, ref ws, startColumn, (colIndex - 1) + dataProjectionOffset, table!.Table);
                    }
                    /************************************************************/

                    /* TÔ MÀU CHO HEADER */
                    /************************************************************/
                    var headerRangeAddress = ExcelCellBase.GetAddress(3, startColumn, 3, colIndex - 1);
                    var range = ws.Cells[headerRangeAddress];
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(table!.HeaderBgColor));
                    range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(table.HeaderTextColor));
                    range.Style.Font.Bold = true;
                    /************************************************************/
                    /* WRAPTEXT PHẦN GHI CHÚ */
                    /************************************************************/
                    var commentRangeAddress = ExcelCellBase.GetAddress(5, startColumn, 5, colIndex - 1);
                    var commentRange = ws.Cells[commentRangeAddress];
                    commentRange.Style.WrapText = true;
                    commentRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    commentRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    var commentRow = ws.Row(5);
                    commentRow.Height = 50;
                    /************************************************************/


                }

                var overallRangeAddress = ExcelCellBase.GetAddress(1, 1, rowsToPrepare + 6, colIndex - 1);
                var overallRange = ws.Cells[overallRangeAddress];
                overallRange.AutoFitColumns();
                overallRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                overallRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                overallRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                overallRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                overallRange.Style.Border.Top.Color.SetColor(Color.Gray);
                overallRange.Style.Border.Right.Color.SetColor(Color.Gray);
                overallRange.Style.Border.Bottom.Color.SetColor(Color.Gray);
                overallRange.Style.Border.Left.Color.SetColor(Color.Gray);

                // Giấu dòng 1,2
                for (var i = 1; i <= 4; i++)
                {
                    if (i != 3)
                    {
                        var row = ws.Row(i);
                        row.Hidden = true;
                        row.Style.Locked = true;
                    }
                }
                // Dòng header
                var headerRow = ws.Row(3);
                headerRow.Height = 50;
                headerRow.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                headerRow.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                headerRow.Style.WrapText = true;

                // Unlock the Input
                for (var i = 1; i < colIndex; i++)
                {
                    var col = ws.Column(i);
                    col.Style.Locked = false;
                }

                // Lock the Input readonly system rows
                var sysRowsAddress = ExcelCellBase.GetAddress(1, 1, 5, colIndex);
                var sysRowsRange = ws.Cells[sysRowsAddress];
                sysRowsRange.Style.Locked = true;

                // Lock and hide the Core
                for (var i = dataProjectionOffset + 1; i < colIndex + dataProjectionOffset; i++)
                {
                    var col = ws.Column(i);
                    col.Style.Locked = true;
                    col.Hidden = true;
                }



                ws.Select();
                // Giấu tất cả các sheet khác
                worksheets.Where(x => x.Name != "INPUT").ToList().ForEach(item =>
                {
                    item.Hidden = eWorkSheetHidden.VeryHidden;
                });

                ws.Protection.AllowDeleteColumns = false;
                ws.Protection.AllowDeleteRows = true;
                ws.Protection.AllowInsertColumns = false;
                ws.Protection.AllowInsertRows = false;
                ws.Protection.AllowFormatColumns = true;
                ws.Protection.IsProtected = true;

                /****************************************************************************************************************************************/


                // using stream without newStream will raise an read/write timeout error
                var newStream = new MemoryStream(package.GetAsByteArray());

                package.Dispose();

                return await Task.Run(() => newStream);

            }
            catch (Exception ex)
            {
                // The IHubContext is for sending notifications to clients, it is not used to call methods on the Hub.
                await _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                {
                    SignalType = "TASK_PROGRESS",
                    ex.Message,
                    Data = "90%",
                    Error = true
                });
                throw new Exception(ex.Message);
            }
        }

        // STEP 3
        private void PrepareDirectReference(ref ExcelWorkbook wb, ref List<ExDirectReference> exDirectReferences, string username)
        {
            ExcelWorksheets worksheets = wb.Worksheets;

            var count = exDirectReferences.Count;
            for (var i = 0; i < exDirectReferences.Count; i++)
            {
                var directReference = exDirectReferences[i];

                /*
                 * Tạm thời chưa bỏ được ID. Ví dụ bảng HU_CONTRACT không có tổ hợp cột nào định tính đơn nhất ngoài Mã NV + Ngày hiệu lực, trong khi DATETIME2 đang gặp vấn đề về múi giờ
                if (directReference.UniqueIndexColumns.IndexOf("ID") >= 0)
                {
                    // Không sử dụng ID trong bộ cột định tính đơn nhất
                    throw new Exception("Column ID should be add automatically by default, do not include it in the UniqueIndexColumns list");
                }
                */

                // Kết hợp namespace và tên bảng để tìm ra entityType từ _dbContext
                var entityType = _dbContext.Model.FindEntityType("API.Entities." + directReference.Table);
                if (entityType != null)
                {
                    if (directReference.UniqueIndexColumns.Count == 0)
                    {
                        // Cần có bộ cột định tính đơn nhất
                        throw new Exception("Invalid object UniqueIndexColumns for reference " + directReference.Table);
                    }

                    // Đơn nhất đơn / đơn nhất kết hợp
                    bool compositMode = directReference.UniqueIndexColumns.Count > 1;
                    string columnsStringFormula;

                    // Nếu là đơn nhất kết hợp
                    // Ghép thứ nhất nguyên dạng, các cột sau có thêm dấu []
                    if (compositMode)
                    {
                        // Công thức ghép côt cho hàm thủ tục SQL
                        columnsStringFormula = "''";
                        var index = 0;
                        directReference.UniqueIndexColumns.ForEach(column =>
                        {
                            if (index == 0)
                            {
                                columnsStringFormula += " + " + column;
                            }
                            else
                            {
                                columnsStringFormula += " + ' [' + " + column + " + ']'";
                            }
                            index++;
                        });
                    }
                    // Nếu là đơn nhất đơn
                    // Lấy nguyên dạng giá trị cột duy nhất này
                    else
                    {
                        columnsStringFormula = directReference.UniqueIndexColumns[0];
                    }
                    string condition = directReference.Condition ?? "(1=1)";

                    SendTaskProgressInfo(username, "PrepareDirectReference", directReference.Table ?? "", "", decimal.Parse((i + 1).ToString()) / decimal.Parse(count.ToString()), 2);

                    // Truy vấn thủ tục SQL để lấy dữ liệu
                    string cnnString = _appSettings.ConnectionStrings.CoreDb;
                    List<ExReferenceColumn> referenceData;
                    using (SqlConnection cnn = new(cnnString))
                    {
                        using (SqlCommand cmd = new())
                        {
                            using (DataSet ds = new())
                            {
                                cmd.Connection = cnn;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = XlsxSqlProcedure.XLS_READ_BY_TABLE_NAME_AND_COLUMNS_FORMULA;
                                cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_TABLE", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = directReference.Table });
                                cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_TEXT_FORMULA", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = columnsStringFormula });
                                cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_CONDITION", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = condition });
                                using (SqlDataAdapter da = new(cmd))
                                {
                                    da.Fill(ds);
                                    referenceData = ds.Tables[0].ToList<ExReferenceColumn>();
                                };
                            };
                        };
                    };

                    // Thêm sheet có tên = bảng tham chiếu
                    var ws = worksheets.Add(directReference.Table);

                    var rangeAddress = ExcelCellBase.GetAddress(1, 1, 1 + referenceData.Count, 2);
                    var range = ws.Cells[rangeAddress];
                    // Đổ dữ liệu trả về từ SQL
                    range.LoadFromCollection(referenceData, true);

                    // Chuẩn bị dropdown
                    var dropdownAddress = ExcelCellBase.GetAddress(2, 1, 1 + referenceData.Count, 1);
                    var dropdownRange = ws.Cells[dropdownAddress];
                    // Chuẩn bị lookup
                    var lockupAddress = ExcelCellBase.GetAddress(2, 1, 1 + referenceData.Count, 2);
                    var lockupRange = ws.Cells[lockupAddress];

                    // Tại sheet hiện tại (có tên là Table) thêm tên range trùng tên table để validate
                    if (!wb.Names.Any(x => x.Name == directReference.Table)) wb.Names.Add(directReference.Table, dropdownRange);
                    // Tại sheet hiện tại (có tên là Table) thêm tên range = tên table + "_LOOKUP" để lookup
                    if (!wb.Names.Any(x => x.Name == directReference.Table + "_LOOKUP")) wb.Names.Add(directReference.Table + "_LOOKUP", lockupRange);

                }
                else
                {
                    throw new Exception(CommonMessageCode.NO_ENTITY_TYPE_FOUND_FOR_THE_GIVEN_NAME + " " + directReference.Table);
                }

            };

        }

        // STEP 3A
        private void PrepareConsequentReference(ref ExcelWorkbook wb, ref List<ExConsequentReference> exConsequentReferences, string username)
        {
            ExcelWorksheets worksheets = wb.Worksheets;

            var count = exConsequentReferences.Count;
            for (var i = 0; i < exConsequentReferences.Count; i++)
            {
                var consequentReference = exConsequentReferences[i];


                // Kết hợp namespace và tên bảng để tìm ra entityType từ _dbContext
                var entityType = _dbContext.Model.FindEntityType("API.Entities." + consequentReference.Table);
                if (entityType != null)
                {

                    string columnsStringFormula = consequentReference.ColumnToLookup;

                    string condition = consequentReference.Condition ?? "(1=1)";

                    SendTaskProgressInfo(username, "PrepareConsequentReference", consequentReference.Table ?? "", "", decimal.Parse((i + 1).ToString()) / decimal.Parse(count.ToString()), 2);

                    // Truy vấn thủ tục SQL để lấy dữ liệu
                    string cnnString = _appSettings.ConnectionStrings.CoreDb;
                    List<ExReferenceColumn> referenceData;
                    using (SqlConnection cnn = new(cnnString))
                    {
                        using (SqlCommand cmd = new())
                        {
                            using (DataSet ds = new())
                            {
                                cmd.Connection = cnn;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = XlsxSqlProcedure.XLS_READ_BY_TABLE_NAME_AND_COLUMNS_FORMULA;
                                cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_TABLE", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = consequentReference.Table });
                                cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_TEXT_FORMULA", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = columnsStringFormula });
                                cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_CONDITION", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = condition });
                                using (SqlDataAdapter da = new(cmd))
                                {
                                    da.Fill(ds);
                                    referenceData = ds.Tables[0].ToList<ExReferenceColumn>();
                                };
                            };
                        };
                    };

                    // Thêm sheet có tên = bảng tham chiếu
                    if (worksheets.Any(x => x.Name == consequentReference.ConsequentReferenceName))
                    {
                        throw new Exception(CommonMessageCode.XLSX_SHEET_EXISTS_ALREADY + " " + consequentReference.ConsequentReferenceName);
                    }

                    var ws = worksheets.Add(consequentReference.ConsequentReferenceName);

                    var rangeAddress = ExcelCellBase.GetAddress(1, 1, 1 + referenceData.Count, 2);
                    var range = ws.Cells[rangeAddress];
                    // Đổ dữ liệu trả về từ SQL

                    var referenceDataReverse = new List<ExReferenceColumnReverse>();
                    referenceData.ForEach(row =>
                    {
                        referenceDataReverse.Add(new ExReferenceColumnReverse()
                        {
                            Value = row.Value,
                            Text = row.Text
                        });
                    });


                    range.LoadFromCollection(referenceDataReverse, true);

                    // Chuẩn bị dropdown
                    var dropdownAddress = ExcelCellBase.GetAddress(2, 1, 1 + referenceData.Count, 1);
                    var dropdownRange = ws.Cells[dropdownAddress];
                    // Chuẩn bị lookup
                    var lockupAddress = ExcelCellBase.GetAddress(2, 1, 1 + referenceData.Count, 2);
                    var lockupRange = ws.Cells[lockupAddress];

                    // Tại sheet hiện tại (có tên là ConsequentReferenceName) thêm tên range trùng tên ConsequentReferenceName để validate
                    if (!wb.Names.Any(x => x.Name == consequentReference.ConsequentReferenceName)) wb.Names.Add(consequentReference.ConsequentReferenceName, dropdownRange);
                    // Tại sheet hiện tại (có tên là ConsequentReferenceName) thêm tên range = tên ConsequentReferenceName + "__LOOKUP" để lookup
                    if (!wb.Names.Any(x => x.Name == consequentReference.ConsequentReferenceName + "__LOOKUP")) wb.Names.Add(consequentReference.ConsequentReferenceName + "__LOOKUP", lockupRange);

                }
                else
                {
                    throw new Exception(CommonMessageCode.NO_ENTITY_TYPE_FOUND_FOR_THE_GIVEN_NAME + " " + consequentReference.Table);
                }

            };

        }

        // STEP4
        private void PrepareIndirectReference(ref ExcelWorkbook wb, ref List<ExIndirectReference> exIndirectReferences, string username)
        {
            ExcelWorksheets worksheets = wb.Worksheets;

            var count = exIndirectReferences.Count;
            for (var i = 0; i < count; i++)
            {
                var indirectReference = exIndirectReferences[i] ?? throw new Exception($"indirectReference at index {i} not found");

                string parentCondition = indirectReference.ParentCondition ?? "(1=1)";
                string childCondition = indirectReference.ChildCondition ?? "(1=1)";

                // Kết hợp namespace và tên bảng để tìm ra entityType từ _dbContext
                var parentEntityType = _dbContext.Model.FindEntityType("API.Entities." + indirectReference.ParentTable);
                if (parentEntityType != null)
                {
                    if (indirectReference.ParentUniqueIndexColumns.Count == 0 || indirectReference.ChildUniqueIndexColumns.Count == 0)
                    {
                        throw new Exception("Invalid object UniqueIndexColumns for reference " + indirectReference.ParentTable);
                    }
                    bool parentCompositMode = indirectReference.ParentUniqueIndexColumns.Count > 1;
                    bool childCompositMode = indirectReference.ChildUniqueIndexColumns.Count > 1;
                    string parentColumnsStringFormula;
                    string childColumnsStringFormula;

                    // Nếu là đơn nhất kết hợp
                    // Ghép thứ nhất nguyên dạng, các cột sau có thêm dấu []
                    if (parentCompositMode)
                    {
                        parentColumnsStringFormula = "''";
                        var parentIndex = 0;
                        indirectReference.ParentUniqueIndexColumns.ForEach(column =>
                        {
                            if (parentIndex == 0)
                            {
                                parentColumnsStringFormula += " + " + column;
                            }
                            else
                            {
                                parentColumnsStringFormula += " + ' [' + " + column + " + ']'";
                            }
                            parentIndex++;
                        });
                    }
                    // Nếu là đơn nhất đơn
                    // Lấy nguyên dạng giá trị cột duy nhất này
                    else
                    {
                        parentColumnsStringFormula = indirectReference.ParentUniqueIndexColumns[0];
                    }

                    // Tương tự đối với bảng con
                    if (childCompositMode)
                    {
                        childColumnsStringFormula = "''";
                        var childIndex = 0;
                        indirectReference.ChildUniqueIndexColumns.ForEach(column =>
                        {
                            if (childIndex == 0)
                            {
                                childColumnsStringFormula += " + " + column;
                            }
                            else
                            {
                                childColumnsStringFormula += " + ' [' + " + column + " + ']'";
                            }
                            childIndex++;
                        });
                    }
                    else
                    {
                        childColumnsStringFormula = indirectReference.ChildUniqueIndexColumns[0];
                    }

                    List<ExReferenceColumn> parentReferenceData;
                    string cnnString = _appSettings.ConnectionStrings.CoreDb;
                    using (SqlConnection cnn = new(cnnString))
                    {
                        using (SqlCommand cmd = new())
                        {
                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            /****************************************/
                            /* Read Parent Table then loop Children */

                            // Read Parent
                            cmd.CommandText = XlsxSqlProcedure.XLS_READ_BY_TABLE_NAME_AND_COLUMNS_FORMULA;
                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_TABLE", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = indirectReference.ParentTable });
                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_TEXT_FORMULA", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = parentColumnsStringFormula });
                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_CONDITION", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = parentCondition });

                            using (DataSet ds = new())
                            {
                                using (SqlDataAdapter da = new(cmd))
                                {
                                    da.Fill(ds);
                                    parentReferenceData = ds.Tables[0].ToList<ExReferenceColumn>();
                                };
                            };
                        };
                    };

                    // Add parent sheet
                    ExcelWorksheet wsParent;
                    if (worksheets.Any(x => x.Name == indirectReference.ParentTable))
                    {
                        wsParent = worksheets.Single(x => x.Name == indirectReference.ParentTable);
                    }
                    else
                    {
                        wsParent = worksheets.Add(indirectReference.ParentTable);
                    }

                    // Add one sheet for children
                    ExcelWorksheet wsChildren;
                    if (worksheets.Any(x => x.Name == indirectReference.ChildTable))
                    {
                        wsChildren = worksheets.Single(x => x.Name == indirectReference.ChildTable);
                    }
                    else
                    {
                        wsChildren = worksheets.Add(indirectReference.ChildTable);
                    }

                    var parentCount = parentReferenceData.Count;
                    if (parentCount == 0) parentCount = 1;

                    var rangeAddress = ExcelCellBase.GetAddress(1, 1, 1 + parentCount, 2);
                    var range = wsParent.Cells[rangeAddress];
                    range.LoadFromCollection(parentReferenceData, true);
                    var dropdownAddress = ExcelCellBase.GetAddress(2, 1, 1 + parentCount, 1);
                    var dropdownRange = wsParent.Cells[dropdownAddress];
                    var lockupAddress = ExcelCellBase.GetAddress(2, 1, 1 + parentCount, 2);
                    var lockupRange = wsParent.Cells[lockupAddress];
                    // Tại sheet hiện tại (có tên = ParentTable) thêm tên range trùng tên ParentTable để validate
                    if (!wb.Names.Any(x => x.Name == indirectReference.ParentTable)) wb.Names.Add(indirectReference.ParentTable, dropdownRange);
                    // Tại sheet hiện tại (có tên = ParentTable) thêm tên range = tên ParentTable + "_LOOKUP" để lookup
                    if (!wb.Names.Any(x => x.Name == indirectReference.ParentTable + "_LOOKUP")) wb.Names.Add(indirectReference.ParentTable + "_LOOKUP", lockupRange);

                    // Loop children
                    parentCount = parentReferenceData.Count;
                    int childBlockCol = 4 /*change 1 to 4 by "anh Văn Tân"*/;
                    for (var j = 0; j < parentCount; j++)
                    {

                        var currentParent = parentReferenceData[j];

                        SendTaskProgressInfo(username, "PrepareIndirectReference", currentParent.Text, "", decimal.Parse((j + 1).ToString()) / decimal.Parse(parentCount.ToString()), 3);

                        List<ExReferenceColumn> childReferenceData;
                        using (SqlConnection cnn = new(cnnString))
                        {
                            using (SqlCommand cmdChild = new())
                            {
                                cmdChild.Connection = cnn;
                                cmdChild.CommandType = CommandType.StoredProcedure;
                                cmdChild.CommandText = XlsxSqlProcedure.XLS_READ_CHILD_BY_PARENT_KEY_VALUE_AND_COLUMNS_FORMULA;
                                cmdChild.Parameters.Add(new SqlParameter { ParameterName = "@P_PARENT_KEY_VALUE", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = currentParent.Value });
                                cmdChild.Parameters.Add(new SqlParameter { ParameterName = "@P_CHILD_TABLE", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = indirectReference.ChildTable });
                                cmdChild.Parameters.Add(new SqlParameter { ParameterName = "@P_CHILD_KEY", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = indirectReference.ChildKey });
                                cmdChild.Parameters.Add(new SqlParameter { ParameterName = "@P_TEXT_FORMULA", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = childColumnsStringFormula });
                                cmdChild.Parameters.Add(new SqlParameter { ParameterName = "@P_CONDITION", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = childCondition });

                                using (SqlDataAdapter daChild = new(cmdChild))
                                {
                                    using (DataSet dsChild = new())
                                    {
                                        daChild.Fill(dsChild);
                                        childReferenceData = dsChild.Tables[0].ToList<ExReferenceColumn>();
                                    }
                                }

                            }
                        }

                        var childCount = childReferenceData.Count;
                        if (childCount == 0) childCount = 1;
                        var childRangeAddress = ExcelCellBase.GetAddress(1, childBlockCol, 1 + childCount, childBlockCol + 1);
                        var childRange = wsChildren.Cells[childRangeAddress];
                        childRange.LoadFromCollection(childReferenceData, true);
                        // sửa lại ô thứ nhất dòng 1 thành tên <ParentTable>+<Text>
                        wsChildren.Cells[1, childBlockCol].Value = indirectReference.ParentTable + $" [{currentParent.Text}]";

                        var childDropdownAddress = ExcelCellBase.GetAddress(2, childBlockCol, 1 + childCount, childBlockCol);
                        var childDropdownRange = wsChildren.Cells[childDropdownAddress];
                        var childLockupAddress = ExcelCellBase.GetAddress(2, childBlockCol, 1 + childCount, childBlockCol + 1);
                        var childLockupRange = wsChildren.Cells[childLockupAddress];
                        /* Tên của vùng tham chiếu được tạo động = <ParentTable> + "_ID_" + <ChildTable> */
                        /* Tên của vùng tìm kiếm được tạo động = <ParentTable> +"_ID_" + <ChildTable> +"_LOOKUP" */
                        var refName = indirectReference.ParentTable + $"_{currentParent.Value}_" + indirectReference.ChildTable;
                        var refLookup = refName + "_LOOKUP";
                        if (!wb.Names.Any(x => x.Name == refName)) wb.Names.Add(refName, childDropdownRange);
                        if (!wb.Names.Any(x => x.Name == refLookup)) wb.Names.Add(refLookup, childLockupRange);

                        // Tịnh tiến sang bên phải 3 cột (để lại một cột trống)
                        childBlockCol += 3;
                    }
                    // Thêm vào vùng cuối hai name range ảo <ParentTable> + "_0_" + <ChildTable> và <ParentTable> + "__" + <ChildTable>
                    var lastDropdownAddress1 = ExcelCellBase.GetAddress(2, childBlockCol, 2, childBlockCol);
                    var lastDropdownRange1 = wsChildren.Cells[lastDropdownAddress1];
                    var lastLockupAddress1 = ExcelCellBase.GetAddress(2, childBlockCol, 2, childBlockCol + 1);
                    var lastLockupRange1 = wsChildren.Cells[lastLockupAddress1];
                    /* Tên của vùng tham chiếu được tạo động = <ParentTable> + "_0_" + <ChildTable> */
                    /* Tên của vùng tìm kiếm được tạo động = <ParentTable> +"_0_" + <ChildTable> +"_LOOKUP" */
                    var lastName1 = indirectReference.ParentTable + "_0_" + indirectReference.ChildTable;
                    var lastNameLookup1 = lastName1 + "_LOOKUP";
                    if (!wb.Names.Any(x => x.Name == lastName1)) wb.Names.Add(lastName1, lastDropdownRange1);
                    if (!wb.Names.Any(x => x.Name == lastNameLookup1)) wb.Names.Add(lastNameLookup1, lastLockupRange1);
                    childBlockCol += 3;
                    var lastDropdownAddress2 = ExcelCellBase.GetAddress(2, childBlockCol, 2, childBlockCol);
                    var lastDropdownRange2 = wsChildren.Cells[lastDropdownAddress2];
                    var lastLockupAddress2 = ExcelCellBase.GetAddress(2, childBlockCol, 2, childBlockCol + 1);
                    var lastLockupRange2 = wsChildren.Cells[lastLockupAddress2];
                    /* Tên của vùng tham chiếu được tạo động = <ParentTable> + "__" + <ChildTable> */
                    /* Tên của vùng tìm kiếm được tạo động = <ParentTable> +"__" + <ChildTable> +"_LOOKUP" */
                    var lastName2 = indirectReference.ParentTable + "__" + indirectReference.ChildTable;
                    var lastNameLookup2 = lastName2 + "_LOOKUP";
                    if (!wb.Names.Any(x => x.Name == lastName2)) wb.Names.Add(lastName2, lastDropdownRange2);
                    if (!wb.Names.Any(x => x.Name == lastNameLookup2)) wb.Names.Add(lastNameLookup2, lastLockupRange2);

                }
                else
                {
                    throw new Exception(CommonMessageCode.NO_ENTITY_TYPE_FOUND_FOR_THE_GIVEN_NAME + " " + indirectReference.ParentTable);
                }

            };

        }

        private void BuildOrgTreeAndPosition(List<HuOrganizationTreeBlockDTO> _treeData, ref ExcelWorkbook wb, string username, int levelSpace)
        {
            var maxCodeLength = _dbContext.HuOrganizations.AsNoTracking().Max(x => x.CODE.Length);
            var blockCount = _treeData.Count();

            void Loop(ref HuOrganizationTreeBlockDTO currentParent)
            {
                var children = currentParent.Children;

                for (var i = 0; i < currentParent.Children.Count; i++)
                {
                    var child = currentParent.Children[i];
                    child.Level = currentParent.Level + 1;
                    Loop(ref child);
                }
            }

            for (var i = 0; i < blockCount; i++)
            {
                var block = _treeData[i];
                block.Level = 1;
                Loop(ref block);

            }

            // Create sheet "ORG_TREE"
            ExcelWorksheet ws;
            var check = wb.Worksheets.SingleOrDefault(x => x.Name == "HU_ORGANIZATION");
            if (check == null)
            {
                ws = wb.Worksheets.Add("HU_ORGANIZATION");
            }
            else
            {
                ws = check;
            }
            var wholeRangeAddress = ExcelCellBase.GetFullAddress(ws.Name, "A:B");
            var range = ws.Cells[wholeRangeAddress];
            range.Style.Font.Name = "Courier New"; // the only monospaced unicode;

            void LoopGenerate(HuOrganizationTreeBlockDTO currentParent, ref ExcelWorksheet ws, ref int row, ref int maxCodeLength)
            {
                row++;
                var probel = new string(' ', currentParent.Level == 1 ? 0 : (int)currentParent.Level! * levelSpace);
                ws.Cells[row, 1].Value = $"{probel}{currentParent.Name} [{currentParent.Code}]";
                ws.Cells[row, 2].Value = currentParent.Id;

                var children = currentParent.Children;

                for (var i = 0; i < currentParent.Children.Count; i++)
                {
                    var child = currentParent.Children[i];
                    SendTaskProgressInfo(username, "BuildOrgTreeAndPosition", currentParent.Name, child.Name, decimal.Parse((i + 1).ToString()) / decimal.Parse(blockCount.ToString()), 4);

                    LoopGenerate(child, ref ws, ref row, ref maxCodeLength);
                }
            }

            var row = 1;
            _treeData.ForEach(block =>
            {
                LoopGenerate(block, ref ws, ref row, ref maxCodeLength);
            });

            // Chuẩn bị dropdown
            var dropdownAddress = ExcelCellBase.GetAddress(2, 1, row + 1, 1);
            var dropdownRange = ws.Cells[dropdownAddress];
            // Chuẩn bị lookup
            var lockupAddress = ExcelCellBase.GetAddress(2, 1, row + 1, 2);
            var lockupRange = ws.Cells[lockupAddress];

            if (!wb.Names.Any(x => x.Name == "HU_ORGANIZATION")) wb.Names.Add("HU_ORGANIZATION", dropdownRange);
            if (!wb.Names.Any(x => x.Name == "HU_ORGANIZATION_LOOKUP")) wb.Names.Add("HU_ORGANIZATION_LOOKUP", lockupRange);


        }

        // STEP 5
        private void PrepareColumnMatchReference(ref ExcelWorkbook wb, ref ExcelWorksheet ws, int startColumn, int colIndex, string currentTable)
        {
            // Các table có thể có các cột trùng tên, nên vùng tham chiếu phải dùng riêng
            // Tìm côt cha tương ứng, sử dụng hàm MATCH
            // Vùng để truy vấn MATCH sẽ là dòng 2 bao gồm các cột từ startColumn (đã đánh dấu) đến cột hiện tại (colIndex)
            var matchAddress = ExcelCellBase.GetAddress(2, startColumn + dataProjectionOffset, 2, colIndex);
            ExcelRange matchRange = ws.Cells[matchAddress];

            var matchRangeName = currentTable + "_COLUMN_MATCH";

            if (!wb.Names.Any(x => x.Name == matchRangeName)) wb.Names.Add(matchRangeName, matchRange);

        }

        private void AddNativeColumnWithVlookupFromSysOtherList(ref ExcelWorksheet ws, ref int dataStartRow, int colIndex, ref int rowsToPrepare, ref ExColumnRule rule)
        {
            var lookupAddress = ExcelCellBase.GetAddress(dataStartRow, colIndex, dataStartRow + rowsToPrepare, colIndex);
            ExcelRange lookupRange = ws.Cells[lookupAddress];
            lookupRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            lookupRange.Style.Fill.BackgroundColor.SetColor(Color.Transparent);
            lookupRange.FormulaR1C1 = $"=IF(RC[-{dataProjectionOffset}]=\"\",\"\",IF(ISERROR(VLOOKUP(RC[-{dataProjectionOffset}],{rule.SysOtherListTypeCode}_LOOKUP, 3, 0)),\"\",VLOOKUP(RC[-{dataProjectionOffset}],{rule.SysOtherListTypeCode}_LOOKUP, 3, 0)))";
            // TO DO: HIDE THIS COLUMN
        }

        private void AddNativeColumnWithVlookupFromDirectReference(ref ExcelWorksheet ws, ref int dataStartRow, int colIndex, ref int rowsToPrepare, ref ExColumnRule rule)
        {
            var lookupAddress = ExcelCellBase.GetAddress(dataStartRow, colIndex, dataStartRow + rowsToPrepare, colIndex);
            ExcelRange lookupRange = ws.Cells[lookupAddress];
            lookupRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            lookupRange.Style.Fill.BackgroundColor.SetColor(Color.Transparent);
            lookupRange.FormulaR1C1 = $"=IF(RC[-{dataProjectionOffset}]=\"\",\"\",IF(ISERROR(VLOOKUP(RC[-{dataProjectionOffset}],{rule.DirectReference}_LOOKUP, 2, 0)),\"\",VLOOKUP(RC[-{dataProjectionOffset}],{rule.DirectReference}_LOOKUP, 2, 0)))";
            // TO DO: HIDE THIS COLUMN
        }

        private void AddReadonlyColumnWithVlookupFromConsequentReference(ref ExcelWorksheet ws, ref int dataStartRow, int colIndex, ref int rowsToPrepare, ref ExColumnRule rule)
        {
            var lookupAddress = ExcelCellBase.GetAddress(dataStartRow, colIndex, dataStartRow + rowsToPrepare, colIndex);
            ExcelRange lookupRange = ws.Cells[lookupAddress];
            lookupRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            lookupRange.Style.Fill.BackgroundColor.SetColor(Color.Transparent);
            lookupRange.FormulaR1C1 = $"=IF(RC[{rule.ConsequentBaseColumnR1C1Offset + dataProjectionOffset}]=\"\",\"\",IF(ISERROR(VLOOKUP(RC[{rule.ConsequentBaseColumnR1C1Offset + dataProjectionOffset}],{rule.ConsequentReference}__LOOKUP, 2, 0)),\"\",VLOOKUP(RC[{rule.ConsequentBaseColumnR1C1Offset + dataProjectionOffset}],{rule.ConsequentReference}__LOOKUP, 2, 0)))";
        }

        private void AddNativeColumnWithVlookupFromIndirectReference(ref ExcelWorksheet ws, ref int dataStartRow, int colIndex, ref int rowsToPrepare, ref ExColumnRule rule, ref List<ExIndirectReference> exIndirectReferences)
        {
            var lookupAddress = ExcelCellBase.GetAddress(dataStartRow, colIndex, dataStartRow + rowsToPrepare, colIndex);
            ExcelRange lookupRange = ws.Cells[lookupAddress];
            lookupRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            lookupRange.Style.Fill.BackgroundColor.SetColor(Color.Transparent);

            if (rule.IndirectReference == null) throw new Exception($"IndirectReference was null for column {rule.Column}");

            //IndirectReference[1] là bảng con. Vùng lookup này đã được tạo từ bước trước. Cân kiểm tra thêm
            lookupRange.FormulaR1C1 = $"=IF(RC[-{dataProjectionOffset}]=\"\",\"\",IF(ISERROR(VLOOKUP(RC[-{dataProjectionOffset}],{rule.IndirectReference[1]}_LOOKUP, 2, 0)),\"\",VLOOKUP(RC[-{dataProjectionOffset}],{rule.IndirectReference[1]}_LOOKUP, 2, 0)))";
            // TO DO: HIDE THIS COLUMN
        }

        private void AddSimpleLinkedFormula(ref ExcelWorksheet ws, ref int dataStartRow, int colIndex, ref int rowsToPrepare)
        {
            var lookupAddress = ExcelCellBase.GetAddress(dataStartRow, colIndex, dataStartRow + rowsToPrepare, colIndex);
            ExcelRange lookupRange = ws.Cells[lookupAddress];
            lookupRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            lookupRange.Style.Fill.BackgroundColor.SetColor(Color.Transparent);
            string formula = $"=IF(RC[-{dataProjectionOffset}]=\"\",\"\",RC[-{dataProjectionOffset}])";
            lookupRange.FormulaR1C1 = formula;
        }

        private void AddNativeColumnWithVlookupFromOrgTree(ref ExcelWorksheet ws, ref int dataStartRow, int colIndex, ref int rowsToPrepare)
        {
            var lookupAddress = ExcelCellBase.GetAddress(dataStartRow, colIndex, dataStartRow + rowsToPrepare, colIndex);
            ExcelRange lookupRange = ws.Cells[lookupAddress];
            lookupRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            lookupRange.Style.Fill.BackgroundColor.SetColor(Color.Transparent);
            lookupRange.FormulaR1C1 = $"=IF(RC[-{dataProjectionOffset}]=\"\",\"\",IF(ISERROR(VLOOKUP(RC[-{dataProjectionOffset}],HU_ORGANIZATION_LOOKUP, 2, 0)),\"\",VLOOKUP(RC[-{dataProjectionOffset}],HU_ORGANIZATION_LOOKUP, 2, 0)))";
        }

        private static void AddDropDownFromSysOtherList(ref ExcelWorksheet ws, ref int dataStartRow, int colIndex, ref int rowsToPrepare, ref ExColumnRule rule)
        {
            // define range to add validation to
            var entryAddress = ExcelCellBase.GetAddress(dataStartRow, colIndex, dataStartRow + rowsToPrepare, colIndex);
            ExcelRange entryRange = ws.Cells[entryAddress];
            entryRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            entryRange.Style.Fill.BackgroundColor.SetColor(Color.Transparent);
            // add a validation and set values
            var validation = ws.DataValidations.AddListValidation(entryAddress);
            validation.ShowErrorMessage = true;
            validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            validation.ErrorTitle = "An invalid value was entered";
            validation.Error = "Select a value from the list";
            validation.Formula.ExcelFormula = "=" + rule.SysOtherListTypeCode;
        }


        private static void AddDropDownFromDirectReference(ref ExcelWorksheet ws, ref int dataStartRow, int colIndex, ref int rowsToPrepare, ref ExColumnRule rule)
        {
            // define range to add validation to
            var entryAddress = ExcelCellBase.GetAddress(dataStartRow, colIndex, dataStartRow + rowsToPrepare, colIndex);
            ExcelRange entryRange = ws.Cells[entryAddress];
            entryRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            entryRange.Style.Fill.BackgroundColor.SetColor(Color.Transparent);
            // add a validation and set values
            var validation = ws.DataValidations.AddListValidation(entryAddress);
            validation.ShowErrorMessage = true;
            validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            validation.ErrorTitle = "An invalid value was entered";
            validation.Error = "Select a value from the list";
            validation.Formula.ExcelFormula = "=" + rule.DirectReference;
        }

        private void AddIndirectReference(ref ExcelWorksheet ws, ref int dataStartRow, int colIndex, ref int rowsToPrepare, ref ExColumnRule rule, ref List<ExIndirectReference> exIndirectReferences, string currentTable, int startColumn)
        {
            // define range to add validation to
            var entryAddress = ExcelCellBase.GetAddress(dataStartRow, colIndex, dataStartRow + rowsToPrepare, colIndex);
            ExcelRange entryRange = ws.Cells[entryAddress];
            entryRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            entryRange.Style.Fill.BackgroundColor.SetColor(Color.Transparent);

            /* Kiểm tra IndirectReference của rule phải có khai báo tương ứng trong list exIndirectReferences */
            /* Khi một dropdown (C) có nguồn phụ thuộc vào một dropdown khác (P)
            /* Thì rule phải khai báo sự phụ thuộc này */
            var indirectReference = rule.IndirectReference ?? throw new Exception($"IndirectReference was null in the rule for column {rule.Column}");
            ExIndirectReference? check = null;
            if (exIndirectReferences.Any(x => x.ParentTable == indirectReference[0] && x.ChildTable == indirectReference[1]))
            {
                check = exIndirectReferences.Single(x => x.ParentTable == indirectReference[0] && x.ChildTable == indirectReference[1]);
            }
            else
            {
                throw new Exception($"No pair {indirectReference[0]} {indirectReference[1]} matched ExIndirectReferences for column {rule.Column}");
            }


            // KHOAI //
            /* Công thức khó nhất Trái Đất nằm ở đây */
            // Tìm exIndirectReference tương ứng

            var parentTable = check.ParentTable;
            var childTable = check.ChildTable;

            // add a validation and set values
            var validation = ws.DataValidations.AddListValidation(entryAddress);
            validation.ShowErrorMessage = true;
            validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            validation.ErrorTitle = "Không có giá trị này trong danh sách";
            validation.Error = "Xin mời chọn giá trị từ danh sách";


            var exIndirectReference = exIndirectReferences
                .Single(x => x.ParentTable == parentTable && x.ChildTable == childTable);

            if (rule.IndirectValue != null && rule.IndirectColumn != null)
            {
                throw new Exception($"Please consider to use only one of IndirectValue or IndirectColumn for column {rule.Column}");
            }
            if (rule.IndirectValue == null && rule.IndirectColumn == null)
            {
                throw new Exception($"Please predefine rather IndirectValue or IndirectColumn for column {rule.Column}");
            }

            // Ví dụ mặc định các tỉnh được lấy ra từ Quốc Gia có ID = 1
            if (rule.IndirectValue != null)
            {
                validation.Formula.ExcelFormula = $"={parentTable}_{rule.IndirectValue}_{childTable}";
            }
            else
            {
                /* Trường hợp này khoai hơn, 
                 * (I have a headache now)
                 * cần sử dụng hàm MATCH để tìm columnIndex của cột cha 
                 * Dịch duyển +1 sang phải để tìm ID của cha
                 */

                // Tìm index của cột nguồn 
                var srcIndexFormula = $"MATCH(\"{rule.IndirectColumn}\",{currentTable}_COLUMN_MATCH,0)+{dataProjectionOffset + startColumn - 1}";
                // Tìm ID của cha qua hàm INDIRECT với tham số thứ hai là FALSE (dùng tham chiếu kiểu R1C1)
                var valueLookupFormula = $"INDIRECT(\"RC\"&{srcIndexFormula},FALSE)";
                // Vùng tham chiếu tương ứng
                var refFormula = "\"" + parentTable + "\" & \"_\" & " + valueLookupFormula + " & \"_\" & \"" + childTable + "\"";
                // Gán công thức validation
                //entryRange.FormulaR1C1 = "=" + refFormula;
                validation.Formula.ExcelFormula = $"=INDIRECT({refFormula})";
            }
        }

        private static void AddDropDownFromOrgTree(ref ExcelWorksheet ws, ref int dataStartRow, int colIndex, ref int rowsToPrepare)
        {
            // define range to add validation to
            var entryAddress = ExcelCellBase.GetAddress(dataStartRow, colIndex, dataStartRow + rowsToPrepare, colIndex);
            ExcelRange entryRange = ws.Cells[entryAddress];
            entryRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            entryRange.Style.Fill.BackgroundColor.SetColor(Color.Transparent);
            // add a validation and set values
            var validation = ws.DataValidations.AddListValidation(entryAddress);
            validation.ShowErrorMessage = true;
            validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
            validation.ErrorTitle = "An invalid value was entered";
            validation.Error = "Select a value from the list";
            validation.Formula.ExcelFormula = "=HU_ORGANIZATION";
        }

        private void FillTopRows(ref ExcelWorksheet ws, ref IEntityType entityType, ref IProperty property, int colIndex, ref string tableName, ref string propertyName, bool addNote = true, string lang = "vi", bool required = false, bool timeRequired = false)
        {
            ws.Cells[1, colIndex].Value = entityType.Name;
            ws.Cells[2, colIndex].Value = propertyName;
            var key = property.Name == "ID" ? "UI_ENTITY_FIELD_CAPTION_COMMON_ID" : "UI_ENTITY_FIELD_CAPTION_" + tableName + "_" + property.Name;
            if (lang == "en")
            {
                ws.Cells[3, colIndex].Value = _mlsData.Where(x => x.Key == key).FirstOrDefault()?.En ?? key;
            }
            else
            {
                ws.Cells[3, colIndex].Value = _mlsData.Where(x => x.Key == key).FirstOrDefault()?.Vi ?? key;
            }

            var propertyType = property.PropertyInfo?.PropertyType;

            if (propertyType != null)
            {
                string columnType;
                if (propertyType.Name != "Nullable`1")
                {
                    columnType = propertyType.Name;
                }
                else
                {
                    if (propertyType.FullName != null)
                    {
                        columnType = propertyType.FullName.Split("[[")[1].Split(",").First().Split(".").Last() + "?";
                    }
                    else
                    {
                        throw new Exception(CommonMessageCode.PROPERTY_TYPE_FULL_NAME_WAS_NULL + " " + property.Name);
                    }
                }
                ws.Cells[4, colIndex].Value = columnType;
                if (addNote)
                {
                    var note = TypeToUserNote(columnType, lang, required, timeRequired);
                    ws.Cells[5, colIndex].Value = note.Text;
                    if (note.Comment != null)
                    {
                        try // Chỉ thêm comment nếu chưa có
                        {
                            if (ws.Cells[5, colIndex].Comment != null) ws.Comments.Remove(ws.Cells[5, colIndex].Comment);
                            ws.Cells[5, colIndex].AddComment(note.Comment, "Histaff");
                        }
                        catch { }
                    }
                };

            }
            else
            {
                throw new Exception(CommonMessageCode.PROPERTY_TYPE_WAS_NULL + " " + property.Name);
            }
        }

        // STEP1
        private void RenderOtherListReferences(ref ExcelPackage package, ref ExcelWorkbook wb, string username)
        {
            // Render SYS_OTHER_LIST references
            var ws = package.Workbook.Worksheets.Add("SYS_OTHER_LIST");

            /*
            var rangeAddress = ExcelCellBase.GetAddress(1, 1, _otherListTypes.Count, 3);
            var range = ws.Cells[rangeAddress];
            range.LoadFromCollection(_otherListTypes, true);
            */

            ws.Cells[1, 1].Value = "NAME";
            ws.Cells[1, 2].Value = "CODE";
            ws.Cells[1, 3].Value = "ID";


            var typeBlockStartColumn = 1;
            var count = _otherListTypes?.Count ?? 0;
            for (var i = 0; i < _otherListTypes?.Count; i++)
            {

                var type = _otherListTypes[i];
                ws.Cells[1, typeBlockStartColumn].Value = "NAME";
                ws.Cells[1, typeBlockStartColumn + 1].Value = "CODE";
                ws.Cells[1, typeBlockStartColumn + 2].Value = "ID";

                ws.Cells[2, typeBlockStartColumn].Value = type.Name;
                ws.Cells[2, typeBlockStartColumn + 1].Value = type.Code;
                ws.Cells[2, typeBlockStartColumn + 2].Value = type.Id;

                var children = _otherLists?.Where(x => x.TypeId == type.Id).ToList();

                // left 1 row blank and load children
                var typeBlockChildrenRow = 4;

                if (children != null && children?.Count != 0)
                {
                    children?.ForEach(row =>
                    {

                        SendTaskProgressInfo(username, "RenderOtherListReferences", type.Name ?? "", row.Name ?? "", decimal.Parse((i + 1).ToString()) / decimal.Parse(count.ToString()), 0);

                        ws.Cells[typeBlockChildrenRow, typeBlockStartColumn].Value = row.Name;
                        ws.Cells[typeBlockChildrenRow, typeBlockStartColumn + 1].Value = row.Code;
                        ws.Cells[typeBlockChildrenRow, typeBlockStartColumn + 2].Value = row.Id;
                        typeBlockChildrenRow++;
                    });
                }
                else
                {
                    typeBlockChildrenRow++;
                }

                var rangeAddress = ExcelCellBase.GetAddress(4, typeBlockStartColumn, typeBlockChildrenRow - 1, typeBlockStartColumn);
                ExcelRange range = ws.Cells["SYS_OTHER_LIST!" + rangeAddress];
                if (!wb.Names.Any(x => x.Name == type.Code)) wb.Names.Add(type.Code, range);
                var lookupAddress = ExcelCellBase.GetAddress(4, typeBlockStartColumn, typeBlockChildrenRow - 1, typeBlockStartColumn + 2);
                ExcelRange lookupRange = ws.Cells["SYS_OTHER_LIST!" + lookupAddress];
                if (!wb.Names.Any(x => x.Name == type.Code + "_LOOKUP")) wb.Names.Add(type.Code + "_LOOKUP", lookupRange);


                typeBlockStartColumn += 4;
            }
        }

        // STEP2
        private void RenderAdministrativePlaces(ref ExcelPackage package, ref ExcelWorkbook wb, string username)
        {
            // Render HU_NATION HU_PROVINCE, HU_DISTRICT, HU_WARD references
            var ws = package.Workbook.Worksheets.Add("NATION_PROVINCE_DISTRICT_WARD");

            int currentBlockColumn = 1;
            // NATION
            var count = _nations?.Count ?? 0;
            for (var i = 0; i < _nations?.Count; i++)
            {
                var nation = _nations[i];

                ws.Cells[1, currentBlockColumn].Value = nation.Name;

                var provinces = _provinces?.Where(x => x.NationId == nation.Id).ToList();

                var curProvinceRow = 2;
                for (var j = 0; j < provinces?.Count; j++)
                {
                    var province = provinces[j];

                    SendTaskProgressInfo(username, "RenderAdministrativePlaces", nation.Name ?? "", province.Name ?? "", decimal.Parse((i + 1).ToString()) / decimal.Parse(count.ToString()), 1);

                    ws.Cells[curProvinceRow, currentBlockColumn].Value = province.Name;
                    curProvinceRow++;
                }
                // Add current Nation Province range
                if (curProvinceRow == 2) curProvinceRow = 3; // a bank Cell if no above was no province
                var provinceRangeAddress = ExcelCellBase.GetAddress(2, currentBlockColumn, --curProvinceRow, currentBlockColumn);
                ExcelRange provinceRange = ws.Cells["NATION_PROVINCE_DISTRICT_WARD!" + provinceRangeAddress];
                if (!wb.Names.Any(x => x.Name == "NATION_" + nation.Id)) wb.Names.Add("NATION_" + nation.Id, provinceRange);

                currentBlockColumn++;
            }
            // add NATIION name reange
            var rangeAddress = ExcelCellBase.GetAddress(1, 1, 1, --currentBlockColumn);
            ExcelRange range = ws.Cells["NATION_PROVINCE_DISTRICT_WARD!" + rangeAddress];
            if (!wb.Names.Any(x => x.Name == "HU_NATION")) wb.Names.Add("HU_NATION", range);
        }

        public async Task<MemoryStream> ImportXlsxToDb(ImportXlsxToDbDTO request, string location, string sid, string username)
        {
            _uow.CreateTransaction();
            try
            {
                SendTaskProgressInfo(
                    username: username,
                    message: "Reading file and configuration",
                    outerMessage: "",
                    innerMessage: "",
                    innerProportion: 0.1m,
                    0
                    );
                // load configuraion
                var jsonFile = Path.Combine(location, request.ExCode + ".json");
                var jsonConfigurationString = File.ReadAllText(jsonFile);
                if (jsonConfigurationString == null) throw new Exception($"Could not read configuration file {jsonFile}");
                ExFile[]? exFile = JsonDocument.Parse(jsonConfigurationString).RootElement.GetProperty("ExFiles").Deserialize<ExFile[]>();
                var config = exFile?.Where(x => x.ExCode == request.ExCode).FirstOrDefault() ?? throw new Exception(CommonMessageCode.CAN_NOT_FIND_CONFIG_FOR_XLSX_FILE + " " + request.ExCode);
                if (config == null) throw new Exception($"Could not read config from json file for code {request.ExCode}");
                var dataStartRow = config.DataStartRow;

                var fullStr = request.Base64String;
                var base64 = fullStr.Split(";base64,")[1];
                var byteArray = Convert.FromBase64String(base64);
                ExcelPackage package = ConvertByteArrayToExcelPackage(byteArray);

                // Thông thường mỗi một nghiệp vụ import chỉ thực hiện cho một bảng
                // Nhưng hiện tại HU_EMPLOYEE_CV và HU_EMPLOYEE đang được thiết kế để import chung 1 lần

                // Clean up the previous tmp data
                // Check tmpData
                for (var i = 0; i < config.ExTables.Count; i++)
                {
                    var exTable = config.ExTables[i];
                    switch (exTable.Table)
                    {
                        case "HU_EMPLOYEE_CV":
                            var range_HU_EMPLOYEE_CV_IMPORT = _dbContext.HuEmployeeCvImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.HuEmployeeCvImports.RemoveRange(range_HU_EMPLOYEE_CV_IMPORT);
                            _dbContext.SaveChanges();

                            break;
                        case "HU_EMPLOYEE":

                            var range_HU_EMPLOYEE_IMPORT = _dbContext.HuEmployeeImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.HuEmployeeImports.RemoveRange(range_HU_EMPLOYEE_IMPORT);
                            _dbContext.SaveChanges();

                            break;
                        case "AT_SWIPE_DATA":

                            var range_AT_SWIPE_DATA_IMPORT = _dbContext.AtSwipeDataImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.AtSwipeDataImports.RemoveRange(range_AT_SWIPE_DATA_IMPORT);
                            _dbContext.SaveChanges();

                            break;
                        case "HU_EVALUATE":
                            var range_HU_EVALUATE = _dbContext.HuEvaluateImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.HuEvaluateImports.RemoveRange(range_HU_EVALUATE);
                            _dbContext.SaveChanges();
                            break;
                        case "HU_EVALUATION_COM":
                            var range_HU_EVALUATION = _dbContext.HuEvaluationComImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.HuEvaluationComImports.RemoveRange(range_HU_EVALUATION);
                            _dbContext.SaveChanges();
                            break;
                        case "HU_EVALUATION_CONCURRENT":
                            var range_HU_EVALUATE_CONCURRENT = _dbContext.HuEvaluateConcurrentImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.HuEvaluateConcurrentImports.RemoveRange(range_HU_EVALUATE_CONCURRENT);
                            _dbContext.SaveChanges();
                            break;
                        case "HU_WORKING":
                            if (exTable.Version == "1")
                            {
                                var range_HU_WORKING = _dbContext.HuWorkingImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                                _dbContext.HuWorkingImports.RemoveRange(range_HU_WORKING);
                            }
                            else if (exTable.Version == "2")
                            {
                                var range_HU_WORKING_HSL_PC = _dbContext.HuWorkingHslPcImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                                _dbContext.HuWorkingHslPcImports.RemoveRange(range_HU_WORKING_HSL_PC);
                            }
                            _dbContext.SaveChanges();
                            break;
                        case "HU_WORKING_ALLOW":
                            var range_HU_WORKING_ALLOW_IMPORT = _dbContext.HuWorkingAllowImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.HuWorkingAllowImports.RemoveRange(range_HU_WORKING_ALLOW_IMPORT);
                            _dbContext.SaveChanges();
                            break;
                        case "HU_CONTRACT":
                            var range_HU_CONTRACT = _dbContext.HuContractImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.HuContractImports.RemoveRange(range_HU_CONTRACT);
                            _dbContext.SaveChanges();
                            break;
                        case "HU_WELFARE_MNG":
                            var range_HU_WELFARE_MNG_IMPORT = _dbContext.HuWelfareMngImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.HuWelfareMngImports.RemoveRange(range_HU_WELFARE_MNG_IMPORT);
                            _dbContext.SaveChanges();
                            break;
                        case "HU_ALLOWANCE_EMP":
                            var range_HU_ALLOWANCE_EMP = _dbContext.HuAllowanceEmpImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.HuAllowanceEmpImports.RemoveRange(range_HU_ALLOWANCE_EMP);
                            _dbContext.SaveChanges();
                            break;
                        case "HU_WORKING_BEFORE":
                            var range_HU_WORKING_BEFORE_IMPORT = _dbContext.HuWorkingBeforeImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.HuWorkingBeforeImports.RemoveRange(range_HU_WORKING_BEFORE_IMPORT);
                            _dbContext.SaveChanges();
                            break;
                        case "HU_FAMILY":
                            var range_HU_FAMILY_IMPORT = _dbContext.HuFamilyImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.HuFamilyImports.RemoveRange(range_HU_FAMILY_IMPORT);
                            _dbContext.SaveChanges();
                            break;
                        case "INS_INFORMATION":
                            var range_INS_INFORMATION_IMPORT = _dbContext.InsInformationImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.InsInformationImports.RemoveRange(range_INS_INFORMATION_IMPORT);
                            _dbContext.SaveChanges();
                            break;
                        case "HU_CERTIFICATE":
                            var range_HU_CERTIFICATE_IMPORT = _dbContext.HuCertificateImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.HuCertificateImports.RemoveRange(range_HU_CERTIFICATE_IMPORT);
                            _dbContext.SaveChanges();
                            break;
                        case "HU_COMMEND":
                            var range_HU_COMMEND_IMPORT = _dbContext.HuCommendImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.HuCommendImports.RemoveRange(range_HU_COMMEND_IMPORT);
                            _dbContext.SaveChanges();
                            break;
                        case "HU_COMMEND_EMPLOYEE":
                            var range_HU_COMMEND_EMPLOYEE_IMPORT = _dbContext.HuCommendEmployeeImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.HuCommendEmployeeImports.RemoveRange(range_HU_COMMEND_EMPLOYEE_IMPORT);
                            _dbContext.SaveChanges();
                            break;
                        case "HU_FILECONTRACT":
                            var range_HU_FILECONTRACT_IMPORT = _dbContext.HuFilecontractImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.HuFilecontractImports.RemoveRange(range_HU_FILECONTRACT_IMPORT);
                            _dbContext.SaveChanges();
                            break;
                        case "AT_SIGN_DEFAULT":
                            var range_AT_SIGN_DEFAULT_IMPORT = _dbContext.AtSignDefaultImports.Where(x => x.XLSX_USER_ID == sid && x.XLSX_EX_CODE == request.ExCode).ToList();
                            _dbContext.AtSignDefaultImports.RemoveRange(range_AT_SIGN_DEFAULT_IMPORT);
                            _dbContext.SaveChanges();
                            break;
                        default:
                            break;
                    }
                };

                // Kết quả:
                List<List<object>> result = new();
                DateTime now = DateTime.UtcNow;
                long session = now.GetJavascriptTimeStamp();

                for (var i = 0; i < config.ExTables.Count; i++)
                {
                    var exTable = config.ExTables[i];
                    DataTable? dataTable = GetDataTableFromExcelPackage(
                        pck: package,
                        config: config,
                        exTable: exTable,
                        username: username
                        );

                    switch (exTable.BufferTable)
                    {
                        case "HU_EMPLOYEE_CV_IMPORT":
                            List<HU_EMPLOYEE_CV_IMPORT> strongList_HU_EMPLOYEE_CV_IMPORT = dataTable!.ToList<HU_EMPLOYEE_CV_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_EMPLOYEE_CV> existing = _dbContext.HuEmployeeCvs.ToList();
                                List<HU_EMPLOYEE_CV> listToCheck = ListMapper<HU_EMPLOYEE_CV_IMPORT, HU_EMPLOYEE_CV>.Merge(strongList_HU_EMPLOYEE_CV_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });

                            strongList_HU_EMPLOYEE_CV_IMPORT.ForEach(item =>
                            {
                                if (item.ID_NO == null && item.PASS_NO != null)
                                {
                                    item.ID_NO = item.PASS_NO;
                                }
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                            });

                            // Insert range to DB
                            _dbContext.HuEmployeeCvImports.AddRange(strongList_HU_EMPLOYEE_CV_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_EMPLOYEE_CV_IMPORT.ToList<object>());
                            break;
                        case "HU_EMPLOYEE_IMPORT":
                            List<HU_EMPLOYEE_IMPORT> strongList_HU_EMPLOYEE_IMPORT = dataTable!.ToList<HU_EMPLOYEE_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_EMPLOYEE> existing = _dbContext.HuEmployees.ToList();
                                List<HU_EMPLOYEE> listToCheck = ListMapper<HU_EMPLOYEE_IMPORT, HU_EMPLOYEE>.Merge(strongList_HU_EMPLOYEE_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });


                            strongList_HU_EMPLOYEE_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                            });

                            // Insert range to DB
                            _dbContext.HuEmployeeImports.AddRange(strongList_HU_EMPLOYEE_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_EMPLOYEE_IMPORT.ToList<object>());
                            break;
                        case "AT_SWIPE_DATA_IMPORT":
                            List<AT_SWIPE_DATA_IMPORT> strongList_AT_SWIPE_DATA_IMPORT = dataTable!.ToList<AT_SWIPE_DATA_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<AT_SWIPE_DATA> existing = _dbContext.AtSwipeDatas.ToList();
                                List<AT_SWIPE_DATA> listToCheck = ListMapper<AT_SWIPE_DATA_IMPORT, AT_SWIPE_DATA>.Merge(strongList_AT_SWIPE_DATA_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });


                            strongList_AT_SWIPE_DATA_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                            });

                            // Insert range to DB
                            _dbContext.AtSwipeDataImports.AddRange(strongList_AT_SWIPE_DATA_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_AT_SWIPE_DATA_IMPORT.ToList<object>());
                            break;
                        case "HU_EVALUATE_IMPORT":
                            List<HU_EVALUATE_IMPORT> strongList_HU_EVALUATE_IMPORT = dataTable!.ToList<HU_EVALUATE_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_EVALUATE> existing = _dbContext.HuEvaluates.ToList();
                                List<HU_EVALUATE> listToCheck = ListMapper<HU_EVALUATE_IMPORT, HU_EVALUATE>.Merge(strongList_HU_EVALUATE_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });
                            var getOtherListByCode01 = await _dbContext.SysOtherLists.AsNoTracking().SingleOrDefaultAsync(x => x.CODE == "LXL01");
                            strongList_HU_EVALUATE_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                                item.EVALUATE_TYPE = getOtherListByCode01?.ID;
                            });

                            // Insert range to DB
                            
                            _dbContext.HuEvaluateImports.AddRange(strongList_HU_EVALUATE_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_EVALUATE_IMPORT.ToList<object>());
                            break;
                        case "HU_EVALUATION_COM_IMPORT":
                            List<HU_EVALUATION_COM_IMPORT> strongList_HU_EVALUATION_COM_IMPORT = dataTable!.ToList<HU_EVALUATION_COM_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_EVALUATION_COM> existing = _dbContext.HuEvaluationComs.ToList();
                                List<HU_EVALUATION_COM> listToCheck = ListMapper<HU_EVALUATION_COM_IMPORT, HU_EVALUATION_COM>.Merge(strongList_HU_EVALUATION_COM_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });

                            strongList_HU_EVALUATION_COM_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                            });

                            // Insert range to DB
                            _dbContext.HuEvaluationComImports.AddRange(strongList_HU_EVALUATION_COM_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_EVALUATION_COM_IMPORT.ToList<object>());
                            break;
                        case "HU_EVALUATE_CONCURRENT_IMPORT":
                            List<HU_EVALUATE_CONCURRENT_IMPORT> strongList_HU_EVALUATE_CONCURRENT_IMPORT = dataTable!.ToList<HU_EVALUATE_CONCURRENT_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_EVALUATE> existing = _dbContext.HuEvaluates.ToList();
                                List<HU_EVALUATE> listToCheck = ListMapper<HU_EVALUATE_CONCURRENT_IMPORT, HU_EVALUATE>.Merge(strongList_HU_EVALUATE_CONCURRENT_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });
                            var getOtherListByCode02 = _dbContext.SysOtherLists.Where(x => x.CODE == "LXL02").FirstOrDefault();
                            strongList_HU_EVALUATE_CONCURRENT_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                                item.EVALUATE_TYPE = getOtherListByCode02?.ID;
                            });

                            // Insert range to DB
                            _dbContext.HuEvaluateConcurrentImports.AddRange(strongList_HU_EVALUATE_CONCURRENT_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_EVALUATE_CONCURRENT_IMPORT.ToList<object>());
                            break;
                        case "HU_WORKING_IMPORT":
                            List<HU_WORKING_IMPORT> strongList_HU_WORKING_IMPORT = dataTable!.ToList<HU_WORKING_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_WORKING> existing = _dbContext.HuWorkings.ToList();
                                List<HU_WORKING> listToCheck = ListMapper<HU_WORKING_IMPORT, HU_WORKING>.Merge(strongList_HU_WORKING_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });

                            strongList_HU_WORKING_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                            });

                            // Insert range to DB
                            _dbContext.HuWorkingImports.AddRange(strongList_HU_WORKING_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_WORKING_IMPORT.ToList<object>());
                            break;
                        case "HU_CONTRACT_IMPORT":
                            List<HU_CONTRACT_IMPORT> strongList_HU_CONTRACT_IMPORT = dataTable!.ToList<HU_CONTRACT_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_CONTRACT> existing = _dbContext.HuContracts.ToList();
                                List<HU_CONTRACT> listToCheck = ListMapper<HU_CONTRACT_IMPORT, HU_CONTRACT>.Merge(strongList_HU_CONTRACT_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });

                            strongList_HU_CONTRACT_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                                item.STATUS_ID = OtherConfig.STATUS_WAITING;
                            });

                            // Insert range to DB
                            _dbContext.HuContractImports.AddRange(strongList_HU_CONTRACT_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_CONTRACT_IMPORT.ToList<object>());
                            break;
                        case "HU_FILECONTRACT_IMPORT":
                            List<HU_FILECONTRACT_IMPORT> strongList_HU_FILECONTRACT_IMPORT = dataTable!.ToList<HU_FILECONTRACT_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_FILECONTRACT> existing = _dbContext.HuFileContracts.ToList();
                                List<HU_FILECONTRACT> listToCheck = ListMapper<HU_FILECONTRACT_IMPORT, HU_FILECONTRACT>.Merge(strongList_HU_FILECONTRACT_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });

                            strongList_HU_FILECONTRACT_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                                item.STATUS_ID = OtherConfig.STATUS_WAITING;
                            });

                            // Insert range to DB
                            _dbContext.HuFilecontractImports.AddRange(strongList_HU_FILECONTRACT_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_FILECONTRACT_IMPORT.ToList<object>());
                            break;

                        case "HU_WELFARE_MNG_IMPORT":
                            List<HU_WELFARE_MNG_IMPORT> strongList_HU_WELFARE_MNG_IMPORT = dataTable!.ToList<HU_WELFARE_MNG_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_WELFARE_MNG> existing = _dbContext.HuWelfareMngs.ToList();
                                List<HU_WELFARE_MNG> listToCheck = ListMapper<HU_WELFARE_MNG_IMPORT, HU_WELFARE_MNG>.Merge(strongList_HU_WELFARE_MNG_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });

                            strongList_HU_WELFARE_MNG_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                            });

                            // Insert range to DB
                            _dbContext.HuWelfareMngImports.AddRange(strongList_HU_WELFARE_MNG_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_WELFARE_MNG_IMPORT.ToList<object>());
                            break;
                        case "HU_ALLOWANCE_EMP_IMPORT":
                            List<HU_ALLOWANCE_EMP_IMPORT> strongList_HU_ALLOWANCE_EMP_IMPORT = dataTable!.ToList<HU_ALLOWANCE_EMP_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_ALLOWANCE_EMP> existing = _dbContext.HuAllowanceEmps.ToList();
                                List<HU_ALLOWANCE_EMP> listToCheck = ListMapper<HU_ALLOWANCE_EMP_IMPORT, HU_ALLOWANCE_EMP>.Merge(strongList_HU_ALLOWANCE_EMP_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });

                            strongList_HU_ALLOWANCE_EMP_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                            });

                            // Insert range to DB
                            _dbContext.HuAllowanceEmpImports.AddRange(strongList_HU_ALLOWANCE_EMP_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_ALLOWANCE_EMP_IMPORT.ToList<object>());
                            break;
                        case "HU_WORKING_BEFORE_IMPORT":
                            List<HU_WORKING_BEFORE_IMPORT> strongList_HU_WORKING_BEFORE_IMPORT = dataTable!.ToList<HU_WORKING_BEFORE_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_WORKING_BEFORE> existing = _dbContext.HuWorkingBefores.ToList();
                                List<HU_WORKING_BEFORE> listToCheck = ListMapper<HU_WORKING_BEFORE_IMPORT, HU_WORKING_BEFORE>.Merge(strongList_HU_WORKING_BEFORE_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });


                            strongList_HU_WORKING_BEFORE_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                            });

                            // Insert range to DB
                            _dbContext.HuWorkingBeforeImports.AddRange(strongList_HU_WORKING_BEFORE_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_WORKING_BEFORE_IMPORT.ToList<object>());
                            break;
                        case "HU_FAMILY_IMPORT":
                            List<HU_FAMILY_IMPORT> strongList_HU_FAMILY_IMPORT = dataTable!.ToList<HU_FAMILY_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_FAMILY> existing = _dbContext.HuFamilys.ToList();
                                List<HU_FAMILY> listToCheck = ListMapper<HU_FAMILY_IMPORT, HU_FAMILY>.Merge(strongList_HU_FAMILY_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });


                            strongList_HU_FAMILY_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                            });

                            // Insert range to DB
                            _dbContext.HuFamilyImports.AddRange(strongList_HU_FAMILY_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_FAMILY_IMPORT.ToList<object>());
                            break;
                        case "HU_WORKING_HSL_PC_IMPORT":
                            List<HU_WORKING_HSL_PC_IMPORT> strongList_HU_WORKING_HSL_PC_IMPORT = dataTable!.ToList<HU_WORKING_HSL_PC_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_WORKING> existing = _dbContext.HuWorkings.ToList();
                                List<HU_WORKING> listToCheck = ListMapper<HU_WORKING_HSL_PC_IMPORT, HU_WORKING>.Merge(strongList_HU_WORKING_HSL_PC_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });


                            strongList_HU_WORKING_HSL_PC_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;

                                if (item.SALARY_LEVEL_ID != null)
                                {
                                    // lấy hệ số chức danh
                                    var get_COEFFICIENT = _dbContext.HuSalaryLevels.Where(x => x.ID == item.SALARY_LEVEL_ID).Select(x => x.COEFFICIENT).First();
                                    item.COEFFICIENT = get_COEFFICIENT;
                                }

                                if (item.SALARY_LEVEL_DCV_ID != null)
                                {
                                    // lấy hệ số điểm công việc
                                    var get_COEFFICIENT_DCV = _dbContext.HuSalaryLevels.Where(x => x.ID == item.SALARY_LEVEL_DCV_ID).Select(x => x.COEFFICIENT).First();
                                    item.COEFFICIENT_DCV = get_COEFFICIENT_DCV;
                                }
                            });

                            // Insert range to DB
                            _dbContext.HuWorkingHslPcImports.AddRange(strongList_HU_WORKING_HSL_PC_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_WORKING_HSL_PC_IMPORT.ToList<object>());
                            break;
                        case "HU_WORKING_ALLOW_IMPORT":
                            List<HU_WORKING_ALLOW_IMPORT> strongList_HU_WORKING_ALLOW_IMPORT = dataTable!.ToList<HU_WORKING_ALLOW_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_WORKING_ALLOW> existing = _dbContext.HuWorkingAllows.ToList();
                                List<HU_WORKING_ALLOW> listToCheck = ListMapper<HU_WORKING_ALLOW_IMPORT, HU_WORKING_ALLOW>.Merge(strongList_HU_WORKING_ALLOW_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });


                            strongList_HU_WORKING_ALLOW_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                            });

                            // Insert range to DB
                            _dbContext.HuWorkingAllowImports.AddRange(strongList_HU_WORKING_ALLOW_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_WORKING_ALLOW_IMPORT.ToList<object>());
                            break;
                        case "INS_INFORMATION_IMPORT":
                            List<INS_INFORMATION_IMPORT> strongList_INS_INFORMATION_IMPORT = dataTable!.ToList<INS_INFORMATION_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<INS_INFORMATION> existing = _dbContext.InsInformations.ToList();
                                List<INS_INFORMATION> listToCheck = ListMapper<INS_INFORMATION_IMPORT, INS_INFORMATION>.Merge(strongList_INS_INFORMATION_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });


                            strongList_INS_INFORMATION_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                            });

                            // Insert range to DB
                            _dbContext.InsInformationImports.AddRange(strongList_INS_INFORMATION_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_INS_INFORMATION_IMPORT.ToList<object>());
                            break;
                        case "HU_COMMEND_IMPORT":
                            List<HU_COMMEND_IMPORT> strongList_HU_COMMEND_IMPORT = dataTable!.ToList<HU_COMMEND_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_COMMEND> existing = _dbContext.HuCommends.ToList();
                                List<HU_COMMEND> listToCheck = ListMapper<HU_COMMEND_IMPORT, HU_COMMEND>.Merge(strongList_HU_COMMEND_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });


                            strongList_HU_COMMEND_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                            });

                            // Insert range to DB
                            _dbContext.HuCommendImports.AddRange(strongList_HU_COMMEND_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_COMMEND_IMPORT.ToList<object>());
                            break;
                        case "HU_COMMEND_EMPLOYEE_IMPORT":
                            List<HU_COMMEND_EMPLOYEE_IMPORT> strongList_HU_COMMEND_EMPLOYEE_IMPORT = dataTable!.ToList<HU_COMMEND_EMPLOYEE_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_COMMEND_EMPLOYEE> existing = _dbContext.HuCommendEmployees.ToList();
                                List<HU_COMMEND_EMPLOYEE> listToCheck = ListMapper<HU_COMMEND_EMPLOYEE_IMPORT, HU_COMMEND_EMPLOYEE>.Merge(strongList_HU_COMMEND_EMPLOYEE_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });

                            strongList_HU_COMMEND_EMPLOYEE_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                            });

                            // Insert range to DB
                            _dbContext.HuCommendEmployeeImports.AddRange(strongList_HU_COMMEND_EMPLOYEE_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_COMMEND_EMPLOYEE_IMPORT.ToList<object>());
                            break;
                        case "HU_CERTIFICATE_IMPORT":
                            List<HU_CERTIFICATE_IMPORT> strongList_HU_CERTIFICATE_IMPORT = dataTable!.ToList<HU_CERTIFICATE_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<HU_CERTIFICATE> existing = _dbContext.HuCertificates.ToList();
                                List<HU_CERTIFICATE> listToCheck = ListMapper<HU_CERTIFICATE_IMPORT, HU_CERTIFICATE>.Merge(strongList_HU_CERTIFICATE_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });

                            strongList_HU_CERTIFICATE_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                            });

                            // Insert range to DB
                            _dbContext.HuCertificateImports.AddRange(strongList_HU_CERTIFICATE_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_HU_CERTIFICATE_IMPORT.ToList<object>());
                            break;
                        case "AT_SIGN_DEFAULT_IMPORT":
                            List<AT_SIGN_DEFAULT_IMPORT> strongList_AT_SIGN_DEFAULT_IMPORT = dataTable!.ToList<AT_SIGN_DEFAULT_IMPORT>();

                            // Check unique indexes (ui);
                            exTable.UniqueIndexes?.ForEach(ui =>
                            {
                                string uiStr = "new (";
                                string uiKeys = "";
                                string notNullStr = "";
                                string selectStr = "";
                                ui.ForEach(c =>
                                {
                                    uiStr += $"it.{c}, ";
                                    uiKeys += $"{c},";
                                    notNullStr += $"{c}!=null && ";
                                    selectStr += $"it.Key.{c} as {c},";
                                });
                                uiStr = uiStr[..(uiStr.Length - 2)];
                                uiKeys = uiKeys[..(uiKeys.Length - 1)];
                                notNullStr = notNullStr[..(notNullStr.Length - 4)];
                                selectStr = selectStr[..(selectStr.Length - 1)];
                                uiStr += ")";
                                selectStr += "";

                                List<AT_SIGN_DEFAULT> existing = _dbContext.AtSignDefaults.ToList();
                                List<AT_SIGN_DEFAULT> listToCheck = ListMapper<AT_SIGN_DEFAULT_IMPORT, AT_SIGN_DEFAULT>.Merge(strongList_AT_SIGN_DEFAULT_IMPORT, existing, ui);
                                var groupped = listToCheck.AsQueryable().GroupBy(uiStr).Select($"new ({selectStr}, Count() as KeyCount)");
                                var ck = groupped.FirstOrDefault($"{notNullStr} && KeyCount > 1");

                                if (ck != null)
                                {
                                    string duplicatedValue = "[";
                                    ui.ForEach(c =>
                                    {
                                        duplicatedValue += $"{ck[c].ToString()},";
                                    });
                                    duplicatedValue = duplicatedValue[..(duplicatedValue.Length - 1)];
                                    duplicatedValue += "]";
                                    throw new Exception($"Trùng mã {duplicatedValue} đối với [{uiKeys}]");
                                }

                            });

                            strongList_AT_SIGN_DEFAULT_IMPORT.ForEach(item =>
                            {
                                item.XLSX_USER_ID = sid;
                                item.XLSX_EX_CODE = config.ExCode;
                                item.XLSX_INSERT_ON = now;
                                item.XLSX_SESSION = session;
                                item.XLSX_FILE_NAME = request.FileName;
                            });

                            // Insert range to DB
                            _dbContext.AtSignDefaultImports.AddRange(strongList_AT_SIGN_DEFAULT_IMPORT);
                            _dbContext.SaveChanges();

                            result.Add(strongList_AT_SIGN_DEFAULT_IMPORT.ToList<object>());
                            break;
                        default:
                            throw new Exception("XLSX_ERROR_PLEASE_UPDATE_YOUR_BUFFER_TABLE_IN_SWITCH_CASES");
                    };

                }


                // The Final
                _uow.Commit();

                // using stream without newStream will raise an read/write timeout error
                var newStream = new MemoryStream(package.GetAsByteArray());

                package.Dispose();

                await Task.Run(() =>
                {
                    SendTaskProgressInfo(username, "Done", session.ToString(), "", 1, 5);
                });

                return newStream;

            }
            catch (Exception ex)
            {
                _uow.Rollback();
                await _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                {
                    SignalType = "TASK_PROGRESS",
                    ex.Message,
                    Data = "90%",
                    Error = true
                });

                throw new Exception(ex.Message);
            }

        }

        internal InfoCell TypeToUserNote(string type, string lang, bool required, bool timeRequired = false, bool timeOnly = false)
        {
            string keyPostFix = required ? "_NOT_NULL" : "";
            switch (type)
            {
                case "String":
                    if (lang == "en")
                    {
                        if (timeOnly == true)
                        {
                            return new()
                            {
                                Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TIME_ONLY")?.En ?? type,
                                Comment = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TIME_ONLY_COMMENT")?.En ?? "'hh:mm:ss or hh:mm"
                            };
                        }
                        else
                        {
                            return new() { Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_STRING_NOT_NULL")?.En ?? type };
                        }
                    }
                    else
                    {
                        if (timeOnly == true)
                        {
                            return new()
                            {
                                Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TIME_ONLY")?.Vi ?? type,
                                Comment = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TIME_ONLY_COMMENT")?.Vi ?? "'hh:mm:ss or hh:mm"
                            };
                        }
                        else
                        {
                            return new() { Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_STRING_NOT_NULL")?.Vi ?? type };
                        }
                    }



                case "String?":
                    if (lang == "en")
                    {
                        return new() { Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_STRING" + keyPostFix)?.En ?? type };
                    }
                    else
                    {
                        return new() { Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_STRING" + keyPostFix)?.Vi ?? type };
                    }
                case "Int64":
                    if (lang == "en")
                    {
                        return new() { Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_PICK_FROM_LIST_NOT_NULL")?.En ?? type };
                    }
                    else
                    {
                        return new() { Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_PICK_FROM_LIST_NOT_NULL")?.Vi ?? type };
                    }
                case "Int64?":
                    if (lang == "en")
                    {
                        return new() { Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_PICK_FROM_LIST" + keyPostFix)?.En ?? type };
                    }
                    else
                    {
                        return new() { Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_PICK_FROM_LIST" + keyPostFix)?.Vi ?? type };
                    }
                case "DateTime":
                    if (timeRequired == true)
                    {

                        if (lang == "en")
                        {
                            return new()
                            {
                                Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE_TIME_NOT_NULL")?.En ?? type,
                                Comment = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE_TIME_COMMENT")?.En ?? "'dd/MM/yyyy hh:mm:ss"
                            };

                        }
                        else
                        {
                            return new()
                            {
                                Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE_NOT_NULL")?.Vi ?? type,
                                Comment = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE_COMMENT")?.Vi ?? "'dd/MM/yyyy hh:mm:ss"
                            };

                        }

                    }
                    else
                    {

                        if (lang == "en")
                        {
                            return new()
                            {
                                Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE_NOT_NULL")?.En ?? type,
                                Comment = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE_COMMENT")?.En ?? "'dd/MM/yyyy"
                            };

                        }
                        else
                        {
                            return new()
                            {
                                Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE_NOT_NULL")?.Vi ?? type,
                                Comment = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE_COMMENT")?.Vi ?? "'dd/MM/yyyy"
                            };

                        }

                    }

                case "DateTime?":

                    if (timeRequired == true)
                    {
                        if (lang == "en")
                        {
                            return new()
                            {
                                Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE_TIME" + keyPostFix)?.En ?? type,
                                Comment = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE_TIME" + keyPostFix)?.En ?? "'dd/MM/yyyy hh:mm:ss"
                            };

                        }
                        else
                        {
                            return new()
                            {
                                Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE_TIME" + keyPostFix)?.Vi ?? type,
                                Comment = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE" + keyPostFix)?.Vi ?? "'dd/MM/yyyy hh:mm:ss"
                            };

                        }

                    }
                    else
                    {
                        if (lang == "en")
                        {
                            return new()
                            {
                                Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE" + keyPostFix)?.En ?? type,
                                Comment = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE" + keyPostFix)?.En ?? "'dd/MM/yyyy"
                            };

                        }
                        else
                        {
                            return new()
                            {
                                Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE" + keyPostFix)?.Vi ?? type,
                                Comment = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_DATE" + keyPostFix)?.Vi ?? "'dd/MM/yyyy"
                            };

                        }
                    }


                case "Boolean":
                    if (lang == "en")
                    {
                        return new()
                        {
                            Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_BOOL_NOT_NULL")?.En ?? "1=YES; 0=NO",
                            Comment = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_BOOL_COMMENT")?.En ?? "1=YES; 0=NO"
                        };

                    }
                    else
                    {

                        return new()
                        {
                            Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_BOOL_NOT_NULL")?.Vi ?? "1=YES; 0=NO",
                            Comment = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_BOOL_COMMENT")?.Vi ?? "1=YES; 0=NO"
                        };

                    }

                case "Boolean?":
                    if (lang == "en")
                    {
                        return new()
                        {
                            Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_BOOL" + keyPostFix)?.En ?? "1=YES; 0=NO",
                            Comment = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_BOOL_COMMENT" + keyPostFix)?.En ?? "1=YES; 0=NO"
                        };

                    }
                    else
                    {

                        return new()
                        {
                            Text = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_BOOL" + keyPostFix)?.Vi ?? "1=YES; 0=NO",
                            Comment = _mlsData.SingleOrDefault(x => x.Key == "XLSX_TYPE_OF_BOOL_COMMENT")?.Vi ?? "1=YES; 0=NO"
                        };

                    }
                default:
                    return new() { Text = type };
            }

        }

        public static ExcelPackage ConvertByteArrayToExcelPackage(byte[] byteArray)
        {
            using MemoryStream memStream = new(byteArray);
            ExcelPackage package = new();
            package.Load(memStream);
            return package;
        }

        public DataTable? GetDataTableFromExcelPackage(ExcelPackage pck, ExFile config, ExTable exTable, string username, bool hasHeader = true, string lang = "vi")
        {

            try
            {
                var ws = pck.Workbook.Worksheets.Single(x => x.Name == "INPUT") ?? throw new Exception("XLSX_ERROR_SHEET_INPUT_NOT_FOUND");
                var bufferClassName = exTable.BufferTable;

                // Tính phạm vi cột của bảng hiện tại
                var sortedExTables = config.ExTables.OrderBy(x => x.RenderOrder).ToList();
                int startColumn = dataProjectionOffset + 1;
                int endColumn = -1;

                for (var i = 0; i < sortedExTables.Count; i++)
                {
                    var e = sortedExTables[i];

                    var testDuplicated = e.RenderColumns.Distinct().ToList();

                    if (testDuplicated.Count < e.RenderColumns.Count) throw new Exception($"Duplicated columns found in RenderColumns of {e.Table}");

                    var columnCount = e.RenderColumns.Count;
                    if (e.Table == exTable.Table)
                    {
                        endColumn = startColumn + columnCount - 1;
                        break;
                    }
                    else
                    {
                        startColumn += columnCount;
                    }
                }

                if (endColumn == -1) throw new Exception("XLSX_ERROR_WHILE_INITIALIZE_COLUMN_RANGE");

                DataTable tbl = new();
                var columnRow = config.ColumnRow;
                var dataStartRow = config.DataStartRow;
                List<bool> columnsIdentityCheck = new List<bool>();
                foreach (var firstRowCell in ws.Cells[columnRow, startColumn, columnRow, endColumn])
                {
                    Type type = ws.Cells[4, firstRowCell.Start.Column].Text.SqlColumnTypeToCType();
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column), type);
                }
                tbl.Columns.Add("XLSX_ROW", typeof(int));
                for (int rowNum = dataStartRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {

                    SendTaskProgressInfo(
                        username: username,
                        message: $"Import from {exTable.Table}",
                        outerMessage: exTable.Table,
                        innerMessage: rowNum.ToString(),
                        innerProportion: decimal.Parse(rowNum.ToString()) / decimal.Parse(ws.Dimension.End.Row.ToString()),
                        4
                    );

                    // Kiểm tra IdentityColumns có giá trị thì mới Insert
                    var identityColumns = exTable.IdentityColumns;
                    var identityColumnsCondition = true;
                    var colIndex = startColumn;

                    for (var i = 0; i < identityColumns.Count; i++)
                    {
                        var c = identityColumns[i];
                        for (var j = startColumn; j < endColumn; j++)
                        {
                            if (ws.Cells[Row: columnRow, Col: j].Text == c)
                            {
                                // Check indentity colums
                                var projectedCell = ws.Cells[Row: rowNum, Col: j];
                                var curCell = ws.Cells[Row: rowNum, Col: j - dataProjectionOffset];
                                if (projectedCell.Text == "")
                                {
                                    if (curCell.Comment != null) ws.Comments.Remove(curCell.Comment);
                                    curCell.AddComment("Cột định danh của bảng không được để trống", "Histaff");
                                    //throw new Exception(Trans("XLSX_ERROR_FOR_IDENTITY_COLUMN", lang));
                                    
                                    identityColumnsCondition = false;
                                    break;
                                }
                            }
                        }

                        if (ws.Cells[Row: rowNum, Col: colIndex].Text == "")
                        {
                            identityColumnsCondition = false;
                            break;
                        };
                    }

                    if (identityColumnsCondition)
                    {
                        columnsIdentityCheck.Add(identityColumnsCondition);
                        var rules = exTable.Rules;
                        var wsRow = ws.Cells[rowNum, startColumn, rowNum, endColumn];
                        DataRow row = tbl.Rows.Add();
                        int colIndexOfDataRow = 0;
                        foreach (var cell in wsRow)
                        {
                            var colOfSheet = cell.Start.Column;

                            var type = ws.Cells[4, cell.Start.Column].Text;
                            switch (type)
                            {
                                case "String":
                                case "String?":
                                    row[colIndexOfDataRow] = cell.Text;
                                    break;
                                case "Int64":
                                case "Int64?":
                                case "Int32":
                                case "Int32?":
                                case "Double":
                                case "Double?":
                                case "Boolean":
                                case "Boolean?":
                                case "Decimal":
                                case "Decimal?":
                                    if (cell.Text == "")
                                    {
                                        row[colIndexOfDataRow] = DBNull.Value;
                                    }
                                    else
                                    {
                                        row[colIndexOfDataRow] = cell.Value;
                                    }
                                    break;
                                case "DateTime":
                                case "DateTime?":

                                    if (cell.Text == "")
                                    {
                                        row[colIndexOfDataRow] = DBNull.Value;
                                    }
                                    else
                                    {

                                        var rule = rules.SingleOrDefault(x => x.Column == ws.Cells[2, cell.Start.Column].Text);
                                        var timeRequired = rule?.TimeRequired ?? false;

                                        if (timeRequired)
                                        {
                                            var dateTime = cell.Text.Split(' ');
                                            var dateStr = dateTime[0];
                                            var timeStr = dateTime[1];

                                            int dd, MM, yyyy, hh, mm, ss = 0;

                                            var arrDate = dateStr.Split('/');
                                            dd = int.Parse(arrDate[0]);
                                            MM = int.Parse(arrDate[1]);
                                            yyyy = int.Parse(arrDate[2]);

                                            var arrTime = timeStr.Split(':');
                                            hh = int.Parse(arrTime[0]);
                                            mm = int.Parse(arrTime[1]);
                                            if (arrTime.Length == 3) ss = int.Parse(arrTime[2]);


                                            if (arrDate.Length == 3 && arrTime.Length >= 2)
                                            {

                                                if (dd > 31 || MM > 12 || hh > 23 || mm > 59 || ss > 59)
                                                {
                                                    if (ws.Cells[rowNum, colOfSheet - dataProjectionOffset].Comment != null) ws.Comments.Remove(ws.Cells[rowNum, colOfSheet - dataProjectionOffset].Comment);
                                                    ws.Cells[rowNum, colOfSheet - dataProjectionOffset].AddComment(
                                                        Trans("XLSX_ERROR_WRONG_DATE_TIME", lang),
                                                        "Histaff"
                                                        );
                                                    row[colIndexOfDataRow] = DBNull.Value;
                                                }
                                                else
                                                {
                                                    var dt = new DateTime(yyyy, MM, dd, hh, mm, ss);
                                                    row[colIndexOfDataRow] = dt;
                                                }
                                            }
                                            else
                                            {
                                                if (ws.Cells[rowNum, colOfSheet - dataProjectionOffset].Comment != null) ws.Comments.Remove(ws.Cells[rowNum, colOfSheet - dataProjectionOffset].Comment);
                                                ws.Cells[rowNum, colOfSheet - dataProjectionOffset].AddComment(
                                                    Trans("XLSX_ERROR_WRONG_DATE", lang),
                                                    "Histaff"
                                                    );
                                                row[colIndexOfDataRow] = DBNull.Value;
                                            }

                                        }
                                        else
                                        {
                                            var arr = cell.Text.Split('/');
                                            if (arr.Length == 3)
                                            {

                                                if (int.Parse(arr[0]) < 32 && int.Parse(arr[1]) < 13)
                                                {
                                                    var d = new DateTime(int.Parse(arr[2]), int.Parse(arr[1]), int.Parse(arr[0]));
                                                    row[colIndexOfDataRow] = d;
                                                }
                                                else
                                                {
                                                    if (ws.Cells[rowNum, colOfSheet - dataProjectionOffset].Comment != null) ws.Comments.Remove(ws.Cells[rowNum, colOfSheet - dataProjectionOffset].Comment);
                                                    ws.Cells[rowNum, colOfSheet - dataProjectionOffset].AddComment(
                                                        Trans("XLSX_ERROR_WRONG_DATE", lang),
                                                        "Histaff"
                                                        );
                                                    row[colIndexOfDataRow] = DBNull.Value;
                                                }
                                            }
                                            else
                                            {
                                                if (ws.Cells[rowNum, colOfSheet - dataProjectionOffset].Comment != null) ws.Comments.Remove(ws.Cells[rowNum, colOfSheet - dataProjectionOffset].Comment);
                                                ws.Cells[rowNum, colOfSheet - dataProjectionOffset].AddComment(
                                                    Trans("XLSX_ERROR_WRONG_DATE", lang),
                                                    "Histaff"
                                                    );
                                                row[colIndexOfDataRow] = DBNull.Value;
                                            }
                                        }

                                    }
                                    break;
                                default:
                                    row[colIndexOfDataRow] = cell.Text;
                                    break;
                            };
                            colIndexOfDataRow++;
                        }
                        row["XLSX_ROW"] = rowNum;
                    }
                    else
                    {
                        // Take note for file
                    }
                }
                if(columnsIdentityCheck.Count == 0)
                {
                    throw new Exception(Trans("XLSX_ERROR_FOR_IDENTITY_COLUMN", lang));
                }
                return tbl;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public string Trans(string key, string lang)
        {
            if (lang.Length != 2) throw new Exception("Wrong parameter lang (must have length = 2)");
            string prop = lang[0].ToString().ToUpper() + lang[1];
            var tryFind = _mlsData.SingleOrDefault(x => x.Key == key);
            var type = typeof(MlsData);
            var property = type.GetProperty(prop);
            if (tryFind != null)
            {
                string? value = property?.GetValue(tryFind)?.ToString();
                return value ?? key;
            }
            else
            {
                return key;
            }
        }

    }
}
