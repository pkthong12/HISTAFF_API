using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Report;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using API.Entities.REPORT;
using API.Socket;
using Aspose.Cells;
using Aspose.Words;
using Aspose.Words.Properties;
using Common.DataAccess;
using Common.Interfaces;
using CORE.DataContract;
using CORE.DTO;
using CORE.GenericUOW;
using CORE.StaticConstant;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using OfficeOpenXml.Style;
using RegisterServicesWithReflection.Services.Base;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace API.All.HRM.XlsxReport
{
    [ScopedRegistration]
    public class XlsxReportRepository : IXlsxReportRepository
    {
        private readonly FullDbContext _dbContext;
        protected AbsQueryDataTemplate QueryData;
        private IHubContext<SignalHub> _hubContext;
        private AppSettings _appSettings;

        private List<SYS_LANGUAGE> _mls;

        public XlsxReportRepository(FullDbContext context, IHubContext<SignalHub> hubContext, IOptions<AppSettings> options)
        {
            _dbContext = context;
            QueryData = new SqlQueryDataTemplate(_dbContext);
            _hubContext = hubContext;
            _mls = _dbContext.SysLanguages.AsNoTracking().OrderBy(x => x.KEY).ToList();
            _appSettings = options.Value;
        }

        public async Task<MemoryStream> GetReport(ReportDTO request, string location, string sid, string username)
        {

            try
            {

                // load configuraion
                var jsonFile = Path.Combine(location, request.ErCode + ".json");
                var jsonConfigurationString = File.ReadAllText(jsonFile) ?? throw new Exception($"Could not read configuration file {jsonFile}");
                ExcelReportDTO config = JsonDocument.Parse(jsonConfigurationString).RootElement.GetProperty("Root").Deserialize<ExcelReportDTO>()!;
                if (config == null) throw new Exception($"Could not read config from json file for code {request.ErCode}");

                string listOrgIds = string.Join(",", request.OrgIds!.ToArray()).ToString();
                request.ListOrgIds = listOrgIds;

                var now = DateTime.Now;
                string monthYearNow = now.ToString("MM/yyyy");
                string dateNow = now.ToString("dd/MM/yyyy");
                string dateTimeNow = now.ToString("dd/MM/yyyy HH:mm:ss");

                if (request.Year != null)
                {
                    monthYearNow = now.Month.ToString() + "/" + request.Year.ToString();
                    dateNow = now.Day.ToString() + "/" + now.Month.ToString() + "/" + request.Year.ToString();
                }
                if (request.Month != null)
                {
                    monthYearNow = request.Month.ToString() + "/" + now.Year.ToString();
                    dateNow = now.Day.ToString() + "/" + request.Month.ToString() + "/" + now.Year.ToString();
                }
                if (request.Year != null && request.Month != null)
                {
                    monthYearNow = request.Month.ToString() + "/" + request.Year.ToString();
                    dateNow = now.Day.ToString() + "/" + request.Month.ToString() + "/" + request.Year.ToString();
                }


                var parameterValues = config.ParameterValues;

                // Truy vấn thủ tục SQL để lấy dữ liệu
                string cnnString = _appSettings.ConnectionStrings.CoreDb;
                using SqlConnection cnn = new(cnnString);
                using SqlCommand cmd = new();
                using DataSet ds = new();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = request.StoreName;
                parameterValues.ForEach(p =>
                {
                    cmd.Parameters.Add(new SqlParameter
                    {
                        ParameterName = p.FeildName,
                        SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), p.DataType, true),
                        Direction = ParameterDirection.Input,
                        Value = request.GetType().GetProperty(p.DtoName)!.GetValue(request, null),
                    });
                });

                using SqlDataAdapter da = new(cmd);
                da.Fill(ds);

                string blankWorkbookPath = Path.Combine(location, "BlankWorkbook.xlsx");
                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }
                FileInfo file = new(blankWorkbookPath);
                var package = new ExcelPackage(file) ?? throw new Exception(CommonMessageCode.CAN_NOT_OPEN_BLANK_WORKBOOK);
                package.Compatibility.IsWorksheets1Based = false;

                ExcelWorkbook wb = package.Workbook;
                ExcelWorksheets worksheets = wb.Worksheets;


                ExcelWorksheet ws = worksheets.Add(config.ErCode);

                // format text all sheet
                ws.Cells.Style.Font.Name = config.FontFamily == null ? "Times New Roman" : config.FontFamily;
                ws.Cells.Style.Font.Size = config.Size == null ? 11 : config.Size!.Value;

                // merge cell
                if (config.MegreCellRange != null)
                {
                    config.MegreCellRange.ForEach(c =>
                    {
                        ws.Cells[c].Merge = true;
                    });
                }

                // Horizontal Alignment Center
                if (config.HorizontalAlignmentCenter != null)
                {
                    config.HorizontalAlignmentCenter.ForEach(c =>
                    {
                        ws.Cells[c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    });
                }

                // Horizontal Alignment Left
                if (config.HorizontalAlignmentLeft != null)
                {
                    config.HorizontalAlignmentLeft.ForEach(c =>
                    {
                        ws.Cells[c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    });
                }

                // font size specific cell 

                if (config.FontSizeSpecifics != null)
                {
                    config.FontSizeSpecifics.ForEach(c =>
                    {
                        ws.Cells[c.Cell].Style.Font.Size = c.Size;
                    });
                }

                // header title
                if (config.HeaderTitles != null)
                {
                    /*config.HeaderTitles.ForEach(p =>
                    {
                        ws.Cells[p.Cell].Value = p.Text;
                    });*/
                    config.HeaderTitles.ForEach(c =>
                    {
                        if (c.Text == "")
                        {
                            ws.Cells[c.Cell].Value = "";
                        }
                        else if (c.Text == "ER_HEADER_REPORT_DATE")
                        {
                            string[] dateList = dateNow.Split("/");
                            string text = request.Lang == "vi" ?
                                                            _mls.Single(x => x.KEY == c.Text).VI :
                                                            (request.Lang == "en" ? _mls.Single(x => x.KEY == c.Text).EN : _mls.Single(x => x.KEY == c.Text).VI);
                            int index = 0;
                            foreach (var item in dateList)
                            {
                                text = text.Replace("..." + index.ToString(), item);
                                index++;
                            }
                            ws.Cells[c.Cell].Value = text;
                        }
                        else if (c.Text == "ER_HEADER_REPORT_MONTH_YEAR")
                        {
                            string[] dateList = monthYearNow.Split("/");
                            string text = request.Lang == "vi" ?
                                                            _mls.Single(x => x.KEY == c.Text).VI :
                                                            (request.Lang == "en" ? _mls.Single(x => x.KEY == c.Text).EN : _mls.Single(x => x.KEY == c.Text).VI);
                            int index = 0;
                            foreach (var item in dateList)
                            {
                                text = text.Replace("..." + index.ToString(), item);
                                index++;
                            }
                            ws.Cells[c.Cell].Value = text;
                        }
                        else if (c.Text == "ER_HEADER_REPORT_FROM_DATE_TO_DATE")
                        {
                            string text = request.Lang == "vi" ?
                                                            _mls.Single(x => x.KEY == c.Text).VI :
                                                            (request.Lang == "en" ? _mls.Single(x => x.KEY == c.Text).EN : _mls.Single(x => x.KEY == c.Text).VI);
                            text = text.Replace("(dd/MM/YYYY)FROM", request.DateStart!.Value.ToString("dd/MM/yyyy"));
                            text = text.Replace("(dd/MM/YYYY)TO", request.DateEnd!.Value.ToString("dd/MM/yyyy"));

                            ws.Cells[c.Cell].Value = text;
                        }
                        else
                        {
                            string text = request.Lang == "vi" ?
                                                            _mls.Single(x => x.KEY == c.Text).VI :
                                                            (request.Lang == "en" ? _mls.Single(x => x.KEY == c.Text).EN : _mls.Single(x => x.KEY == c.Text).VI);
                            if (text.Contains("...(YYYY)"))
                            {
                                text = text.Replace("...(YYYY)", request.Year.ToString());
                            }
                            if (text.Contains("...(YYYY-1)"))
                            {
                                text = text.Replace("...(YYYY-1)", (request.Year - 1).ToString());
                            }
                            if (text.Contains("...(YYYY+1)"))
                            {
                                text = text.Replace("...(YYYY+1)", (request.Year + 1).ToString());
                            }
                            ws.Cells[c.Cell].Value = text;
                        }
                    });
                }

                // Header title no translate
                if (config.HeaderNoTranslates != null)
                {
                    config.HeaderNoTranslates.ForEach(c =>
                    {
                        if (c.Text.Contains("...(YYYY)"))
                        {
                            c.Text = c.Text.Replace("...(YYYY)", request.Year.ToString());
                        }
                        else if (c.Text.Contains("...(YYYY-1)"))
                        {
                            c.Text = c.Text.Replace("...(YYYY-1)", (request.Year - 1).ToString());
                        }
                        else if (c.Text.Contains("...(YYYY+1)"))
                        {
                            c.Text = c.Text.Replace("...(YYYY+1)", (request.Year + 1).ToString());
                        }
                        ws.Cells[c.Cell].Value = c.Text;
                    });
                }

                // Auto Fit Column
                if (config.RangeAutoFit != null)
                {
                    ws.Cells[config.RangeAutoFit].AutoFitColumns();
                }

                // Set row height
                if (config.RowHeighs != null)
                {
                    config.RowHeighs.ForEach(p =>
                    {
                        ws.Row(p.RowNumber).Height = p.Height;
                    });
                }

                // Set column width
                if (config.ColumnWidths != null)
                {
                    config.ColumnWidths.ForEach(p =>
                    {
                        ws.Column(p.ColumnNumber).Width = p.Width;
                    });
                }

                // Set font Bold
                if (config.FontBolds != null)
                {
                    config.FontBolds.ForEach(p =>
                    {
                        ws.Cells[p].Style.Font.Bold = true;
                    });
                }

                // Set font Italic
                if (config.FontItalic != null)
                {
                    config.FontItalic.ForEach(p =>
                    {
                        ws.Cells[p].Style.Font.Italic = true;
                    });
                }

                // Vertical Alignment Center
                if (config.VerticalAlignmentCenter != null)
                {
                    config.VerticalAlignmentCenter.ForEach(p =>
                    {
                        ws.Cells[p].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    });
                }

                // Set Wrap Text
                if (config.WrapText != null)
                {
                    config.WrapText.ForEach(p =>
                    {
                        ws.Cells[p].Style.WrapText = true;
                    });
                }

                // Set border header
                ws.Cells[config.BorderHeader].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[config.BorderHeader].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[config.BorderHeader].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[config.BorderHeader].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                // HEADER
                if (config.HeaderGridTitles != null)
                {
                    config.HeaderGridTitles.ForEach(c =>
                    {
                        if (c.Text == "")
                        {
                            ws.Cells[c.Cell].Value = "";
                        }
                        else
                        {
                            string text = request.Lang == "vi" ?
                                                                _mls.Single(x => x.KEY == c.Text).VI :
                                                                (request.Lang == "en" ? _mls.Single(x => x.KEY == c.Text).EN : _mls.Single(x => x.KEY == c.Text).VI);
                            if (text.Contains("...(YYYY)"))
                            {
                                text = text.Replace("...(YYYY)", request.Year.ToString());
                            }
                            if (text.Contains("...(YYYY-1)"))
                            {
                                text = text.Replace("...(YYYY-1)", (request.Year - 1).ToString());
                            }
                            if (text.Contains("...(YYYY+1)"))
                            {
                                text = text.Replace("...(YYYY+1)", (request.Year + 1).ToString());
                            }
                            ws.Cells[c.Cell].Value = text;
                        }
                    });
                }
                // Set uppercase
                if (config.UpperCase != null)
                {
                    config.UpperCase.ForEach(p =>
                    {
                        ws.Cells[p].Value = ws.Cells[p].Text.ToUpper();
                    });
                }

                // Set lowercase
                if (config.LowerCase != null)
                {
                    config.LowerCase.ForEach(p =>
                    {
                        ws.Cells[p].Value = ws.Cells[p].Text.ToLower();
                    });
                }

                if (config.GridContents != null)
                {
                    config.GridContents.ForEach(p =>
                    {
                        ws.Cells[p.Cell].Value = p.Text;
                    });
                }

                var rangeAddress = "";
                switch (config.ClassName)
                {
                    case "LABOR_REPORT":
                        {
                            var referenceData = ds.Tables[0].ToList<LABOR_REPORT>();
                            rangeAddress = ExcelCellBase.GetAddress(config.HeaderRow + 1, 1, config.HeaderRow + (referenceData.Count != 0 ? referenceData.Count : 1), config.HeaderGridTitles!.Count);
                            var range = ws.Cells[rangeAddress];

                            // Set borders content
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            // Đổ dữ liệu trả về từ SQL
                            range.LoadFromCollection(referenceData, false);
                            ws.Cells[ws.Dimension.Address].AutoFitColumns();
                            break;
                        }


                    case "BIRTHDAY_REPORT":
                        {
                            var referenceData = ds.Tables[0].ToList<BIRTHDAY_REPORT>();
                            rangeAddress = ExcelCellBase.GetAddress(config.HeaderRow + 1, 1, config.HeaderRow + (referenceData.Count != 0 ? referenceData.Count : 1), config.HeaderGridTitles!.Count);
                            var range = ws.Cells[rangeAddress];

                            // Set borders content
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            // Đổ dữ liệu trả về từ SQL
                            range.LoadFromCollection(referenceData, false);
                            ws.Cells[ws.Dimension.Address].AutoFitColumns();
                            break;
                        }

                    case "INSURANCE_REPORT":
                        {
                            var referenceData = ds.Tables[0].ToList<INSURANCE_REPORT>();
                            rangeAddress = ExcelCellBase.GetAddress(config.HeaderRow + 1, 1, config.HeaderRow + (referenceData.Count != 0 ? referenceData.Count : 1), config.HeaderGridTitles!.Count - 1);
                            var range = ws.Cells[rangeAddress];

                            // Set borders content
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws.Cells[rangeAddress].Style.Numberformat.Format = "###,###,###0";
                            // Đổ dữ liệu trả về từ SQL
                            range.LoadFromCollection(referenceData, false);
                            ws.Cells[ws.Dimension.Address].AutoFitColumns();
                            break;
                        }
                    case "TRANSFER_PAYROLL_REPORT":
                        {
                            var referenceData = ds.Tables[0].ToList<TRANSFER_PAYROLL_REPORT>();
                            rangeAddress = ExcelCellBase.GetAddress(config.HeaderRow + 1, 1, config.HeaderRow + (referenceData.Count != 0 ? referenceData.Count : 1), config.HeaderGridTitles!.Count);
                            var range = ws.Cells[rangeAddress];
                            ws.Cells[rangeAddress].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Alignment is center
                            ws.Cells[rangeAddress].Style.Numberformat.Format = "###,###,###0";
                            // Set borders content
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            // Đổ dữ liệu trả về từ SQL
                            range.LoadFromCollection(referenceData, false);
                            ws.Cells[ws.Dimension.Address].AutoFitColumns();

                            // Hiển thị tổng kết quả cuối cùng 
                            int lastRow = config.HeaderRow + (referenceData.Count != 0 ? referenceData.Count + 1 : 1);
                            ws.Cells[lastRow, 1, lastRow, 4].Merge = true;
                            ws.Cells[lastRow, 1].Value = (request.Lang == "vi" ? _mls.Single(x => x.KEY == "ER_TOTAL").VI : (request.Lang == "en" ? _mls.Single(x => x.KEY == "ER_TOTAL").EN : _mls.Single(x => x.KEY == "ER_TOTAL").VI));
                            ws.Cells[lastRow, 5].Value = ds.Tables[1].Rows[0]["TOTAL_RESULT"];
                            ws.Cells[lastRow, 5].Style.Numberformat.Format = "###,###,###,###0";
                            ws.Cells[lastRow, 6].Value = "VND";
                            ws.Cells[lastRow, 1, lastRow, config.HeaderGridTitles.Count].Style.Font.Bold = true;
                            ws.Cells[lastRow, 1, lastRow, config.HeaderGridTitles.Count].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ws.Cells[lastRow, 1, lastRow, config.HeaderGridTitles.Count].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            ws.Cells[lastRow, 1, lastRow, config.HeaderGridTitles.Count].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            ws.Cells[lastRow, 1, lastRow, config.HeaderGridTitles.Count].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            break;
                        }
                    case "BM1_REPORT":
                        {
                            int sumCol1 = 0;
                            int sumCol2 = 0;
                            int sumCol3 = 0;
                            int sumCol4 = 0;
                            int sumCol5 = 0;
                            int sumCol6 = 0;
                            int sumCol7 = 0;
                            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                var referenceData = ds.Tables[0].Rows[i].ItemArray;
                                ws.Cells[i + 9, 4].Value = referenceData[2];
                                ws.Cells[i + 9, 5].Value = referenceData[3];
                                ws.Cells[i + 9, 7].Value = referenceData[4];
                                ws.Cells[i + 9, 8].Value = referenceData[5];
                                ws.Cells[i + 9, 9].Value = referenceData[6];
                                ws.Cells[i + 9, 11].Value = referenceData[7];
                                ws.Cells[i + 9, 13].Value = referenceData[8];
                                if (Int32.TryParse(referenceData[2]!.ToString(), out int col1Value))
                                {
                                    sumCol1 += col1Value;

                                }
                                if (Int32.TryParse(referenceData[3]!.ToString(), out int col2Value))
                                {
                                    sumCol2 += col2Value;

                                }
                                if (Int32.TryParse(referenceData[4]!.ToString(), out int col3Value))
                                {
                                    sumCol3 += col3Value;
                                }
                                if (Int32.TryParse(referenceData[5]!.ToString(), out int col4Value))
                                {
                                    sumCol4 += col4Value;
                                }
                                if (Int32.TryParse(referenceData[6]!.ToString(), out int col5Value))
                                {
                                    sumCol5 += col5Value;
                                }
                                if (Int32.TryParse(referenceData[7]!.ToString(), out int col6Value))
                                {
                                    sumCol6 += col6Value;
                                }
                                if (Int32.TryParse(referenceData[8]!.ToString(), out int col7Value))
                                {
                                    sumCol7 += col7Value;
                                }
                            }
                            ws.Cells[13, 4].Value = sumCol1;
                            ws.Cells[13, 5].Value = sumCol2;
                            ws.Cells[13, 7].Value = sumCol3;
                            ws.Cells[13, 8].Value = sumCol4;
                            ws.Cells[13, 9].Value = sumCol5;
                            ws.Cells[13, 11].Value = sumCol6;
                            ws.Cells[13, 13].Value = sumCol7;
                            ws.Cells[18, 2].Value = request.EmployeeDeclare;
                            ws.Cells[18, 10].Value = request.EmployeeRepresentative;

                            break;
                        }
                    case "BM2_REPORT":
                        {
                            var referenceData = ds.Tables[0].Rows[0].ItemArray;
                            //ws.Cells[28, 6].Value = referenceData[0];
                            //ws.Cells[28, 6].Value = referenceData[1];
                            ws.Cells[18, 5].Value = referenceData[2];
                            ws.Cells[19, 5].Value = referenceData[3];
                            ws.Cells[21, 5].Value = referenceData[4];
                            ws.Cells[24, 4].Value = referenceData[5];
                            ws.Cells[24, 6].Value = referenceData[6];
                            ws.Cells[25, 5].Value = referenceData[7];
                            ws.Cells[26, 5].Value = referenceData[9];
                            ws.Cells[28, 5].Value = referenceData[11];
                            ws.Cells[32, 2].Value = request.EmployeeDeclare;
                            ws.Cells[32, 3].Value = request.EmployeeRepresentative;
                            ws.Cells[9, 1, 28, 6].Style.Numberformat.Format = "###,###,###,###0.00";
                            ws.Cells[ws.Dimension.Address].AutoFitColumns();

                            ws.Cells[18, 5].Style.Numberformat.Format = "General";
                            ws.Column(6).Width = 18;
                            break;
                        }
                    case "BM3_REPORT":
                        {
                            var referenceData = ds.Tables[0].Rows[0].ItemArray;
                            ws.Cells[21, 5].Value = referenceData[0];
                            ws.Cells[22, 5].Value = referenceData[1];
                            ws.Cells[23, 5].Value = referenceData[2];
                            ws.Cells[24, 5].Value = referenceData[3];
                            ws.Cells[27, 5].Value = referenceData[4];
                            ws.Cells[28, 5].Value = referenceData[5];
                            ws.Cells[29, 5].Value = referenceData[6];
                            ws.Cells[31, 5].Value = referenceData[7];
                            ws.Cells[32, 5].Value = referenceData[8];
                            ws.Cells[33, 5].Value = referenceData[9];
                            ws.Cells[36, 5].Value = referenceData[10];
                            ws.Cells[37, 5].Value = referenceData[11];

                            ws.Cells[44, 2].Value = request.EmployeeDeclare;
                            ws.Cells[44, 5].Value = request.EmployeeRepresentative;
                            ws.Cells[9, 1, 37, 6].Style.Numberformat.Format = "###,###,###,###0";
                            ws.Cells[ws.Dimension.Address].AutoFitColumns();
                            break;
                        }
                    case "BM4_REPORT":
                        {
                            var referenceData = ds.Tables[0].Rows[0].ItemArray;
                            ws.Cells[9, 4].Value = referenceData[4];
                            ws.Cells[9, 6].Value = referenceData[4];
                            ws.Cells[10, 4].Value = referenceData[0];
                            ws.Cells[10, 6].Value = referenceData[0];
                            ws.Cells[11, 4].Value = referenceData[1];
                            ws.Cells[11, 6].Value = referenceData[1];
                            ws.Cells[12, 4].Value = referenceData[2];
                            ws.Cells[12, 6].Value = referenceData[2];
                            ws.Cells[13, 4].Value = referenceData[3];
                            ws.Cells[13, 6].Value = referenceData[3];
                            ws.Cells[15, 4].Value = referenceData[5];
                            ws.Cells[15, 6].Value = referenceData[5];
                            ws.Cells[15, 5].Value = referenceData[6];
                            ws.Cells[15, 7].Value = referenceData[6];
                            ws.Cells[16, 4].Value = referenceData[7];
                            ws.Cells[16, 6].Value = referenceData[7];
                            ws.Cells[17, 4].Value = referenceData[8];
                            ws.Cells[17, 6].Value = referenceData[8];
                            ws.Cells[23, 4].Value = referenceData[9];
                            ws.Cells[23, 6].Value = referenceData[9];
                            ws.Cells[24, 4].Value = referenceData[10];
                            ws.Cells[24, 6].Value = referenceData[10];
                            ws.Cells[25, 4].Value = referenceData[11];
                            ws.Cells[25, 6].Value = referenceData[11];
                            ws.Cells[26, 4].Value = referenceData[12];
                            ws.Cells[26, 6].Value = referenceData[12];
                            ws.Cells[29, 4].Value = referenceData[13];
                            ws.Cells[29, 6].Value = referenceData[13];
                            ws.Cells[30, 4].Value = referenceData[14];
                            ws.Cells[30, 6].Value = referenceData[14];
                            ws.Cells[31, 4].Value = referenceData[15];
                            ws.Cells[31, 6].Value = referenceData[15];
                            ws.Cells[33, 4].Value = referenceData[16];
                            ws.Cells[33, 6].Value = referenceData[16];
                            ws.Cells[34, 4].Value = referenceData[17];
                            ws.Cells[34, 6].Value = referenceData[17];
                            ws.Cells[35, 4].Value = referenceData[18];
                            ws.Cells[35, 6].Value = referenceData[18];

                            ws.Cells[47, 2].Value = request.EmployeeDeclare;
                            ws.Cells[47, 5].Value = request.EmployeeRepresentative;
                            ws.Cells[7, 4, 41, 8].Style.Numberformat.Format = "###,###,###,###0";
                            ws.Cells[ws.Dimension.Address].AutoFitColumns();
                            break;
                        }
                    case "BM5_REPORT":
                        {
                            for (int i = 1; i <= 26; i++)
                            {
                                ws.Cells[config.HeaderRow + 1, i, config.HeaderRow + 1, i].Value = i;
                            }
                            var referenceData = ds.Tables[0].Rows[0].ItemArray;
                            ws.Cells[10, 14].Value = referenceData[0];
                            ws.Cells[10, 16].Value = referenceData[1];
                            ws.Cells[10, 19].Value = referenceData[2];
                            ws.Cells[10, 21].Value = referenceData[3];
                            ws.Cells[10, 22].Value = referenceData[4];
                            ws.Cells[10, 23].Value = referenceData[5];
                            ws.Cells[10, 1, 11, 26].Style.Numberformat.Format = "###,###,###,###0.00";
                            ws.Cells[10, 1, 10, 26].AutoFitColumns();
                            break;
                        }
                    case "BM6_REPORT":
                        {
                            for (int i = 1; i <= 21; i++)
                            {
                                ws.Cells[config.HeaderRow + 1, i, config.HeaderRow + 1, i].Value = i;
                            }
                            var referenceData = ds.Tables[0].Rows[0].ItemArray;
                            ws.Cells[9, 4].Value = referenceData[0];
                            ws.Cells[9, 7].Value = referenceData[1];
                            ws.Cells[9, 12].Value = referenceData[2];
                            ws.Cells[9, 15].Value = referenceData[3];
                            ws.Cells[9, 18].Value = referenceData[4];
                            //ws.Cells[9, 1, 10, 22].Style.Numberformat.Format = "###,###,###,###0.000";
                            break;
                        }
                    case "DOCUMENT_REPORT":
                        {
                            var referenceData = ds.Tables[0].ToList<DOCUMENT_REPORT>();
                            rangeAddress = ExcelCellBase.GetAddress(config.HeaderRow + 1, 1, config.HeaderRow + (referenceData.Count != 0 ? referenceData.Count : 1), config.HeaderGridTitles!.Count);
                            var range = ws.Cells[rangeAddress];

                            // Set borders content
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            var getListOrg = ds.Tables[1].Rows[0]["LIST_ORG_NAME"];

                            ws.Cells[5, 1].Value = $"{getListOrg.ToString()!.ToUpper()}";

                            // Đổ dữ liệu trả về từ SQL
                            range.LoadFromCollection(referenceData, false);
                            ws.Cells[ws.Dimension.Address].AutoFitColumns();
                            break;
                        }
                    case "PAYROLL_ALLOWANCE_REPORT":
                        {
                            var referenceData = ds.Tables[0].ToList<PAYROLL_ALLOWANCE_REPORT>();
                            rangeAddress = ExcelCellBase.GetAddress(config.HeaderRow + 1, 1, config.HeaderRow + (referenceData.Count != 0 ? referenceData.Count : 1), config.HeaderGridTitles!.Count);
                            var range = ws.Cells[rangeAddress];

                            // thêm dấu phân tách cho các bản ghi
                            ws.Cells[rangeAddress].Style.Numberformat.Format = "###,###,###,###0";

                            // Set borders content
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            range.LoadFromCollection(referenceData, false);

                            int lastRow = config.HeaderRow + (referenceData.Count != 0 ? referenceData.Count + 1 : 1);
                            ws.Cells[lastRow, 1, lastRow, 2].Merge = true;

                            var totalReferenceData = ds.Tables[1].ToList<PAYROLL_ALLOWANCE_REPORT>();
                            rangeAddress = ExcelCellBase.GetAddress(lastRow, 1, lastRow, config.HeaderGridTitles.Count);
                            var totalRange = ws.Cells[rangeAddress];

                            // Set borders content
                            totalRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            totalRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            totalRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            totalRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            totalRange.LoadFromCollection(totalReferenceData, false);

                            ws.Cells[ws.Dimension.Address].AutoFitColumns();

                            ws.Cells[lastRow, 1, lastRow, config.HeaderGridTitles.Count].Style.Numberformat.Format = "###,###,###,###0";

                            ws.Cells[lastRow, 1, lastRow, config.HeaderGridTitles.Count].Style.Font.Bold = true;

                            ws.Cells[lastRow, 1].Value = (request.Lang == "vi" ? _mls.Single(x => x.KEY == "ER_TOTAL_SUM").VI : (request.Lang == "en" ? _mls.Single(x => x.KEY == "ER_TOTAL_SUM").EN : _mls.Single(x => x.KEY == "ER_TOTAL_SUM").VI));
                            ws.Cells[lastRow, 1, lastRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            var getMonth = ds.Tables[2].Rows[0]["MONTH"];

                            var getYear = ds.Tables[3].Rows[0]["YEAR"];

                            ws.Cells[3, 1].Value = $"THANH TOÁN TIỀN LƯƠNG VÀ PHỤ CẤP - THÁNG {getMonth}/{getYear}";

                            var select_5 = ds.Tables[4].Rows[0]["QUANTITY_WORK_DAY"];
                            dynamic getQuantityWorkDay = 0;
                            if (select_5 != null && select_5.GetType().ToString().ToUpper() == "SYSTEM.DOUBLE") getQuantityWorkDay = select_5;     // fix lỗi out of range

                            ws.Cells[lastRow + 1, 2].Value = $"Ghi chú: Tháng {getMonth}/{getYear} có {getQuantityWorkDay} ngày làm việc thực tế được hưởng lương";
                            ws.Cells[lastRow + 1, 2, lastRow + 1, config.HeaderGridTitles.Count].Merge = true;
                            ws.Cells[lastRow + 1, 2, lastRow + 1, config.HeaderGridTitles.Count].Style.Font.Italic = true;

                            var select_1 = ds.Tables[1].ToList<PAYROLL_ALLOWANCE_REPORT>();
                            double actualAmountReceived = 0;
                            if (select_1.Count() > 0 && select_1 != null) actualAmountReceived = select_1[0].CSUM5;     // fix lỗi out of range

                            ws.Cells[lastRow + 2, 1].Value = $"Thực lĩnh: {UppercaseFirstCharacter(ConvertNumberToText(actualAmountReceived))}";
                            ws.Cells[lastRow + 2, 1, lastRow + 2, config.HeaderGridTitles.Count].Merge = true;
                            ws.Cells[lastRow + 2, 1, lastRow + 2, config.HeaderGridTitles.Count].Style.Font.Bold = true;
                            ws.Cells[lastRow + 2, 1, lastRow + 2, config.HeaderGridTitles.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            ws.Cells[lastRow + 6, 2].Value = "LẬP BIỂU";
                            ws.Cells[lastRow + 6, 2, lastRow + 6, 2].Style.Font.Bold = true;

                            ws.Cells[lastRow + 6, 5].Value = "BAN TCNS";
                            ws.Cells[lastRow + 6, 5, lastRow + 6, 5].Style.Font.Bold = true;

                            ws.Cells[lastRow + 6, 10].Value = "BAN TCKT";
                            ws.Cells[lastRow + 6, 10, lastRow + 6, 10].Style.Font.Bold = true;

                            ws.Cells[lastRow + 5, 12].Value = "Hà Nội, ngày ... tháng ... năm ...";
                            ws.Cells[lastRow + 5, 12, lastRow + 5, 14].Style.Font.Italic = true;

                            ws.Cells[lastRow + 6, 12].Value = "DUYỆT";
                            ws.Cells[lastRow + 6, 12, lastRow + 6, 14].Merge = true;
                            ws.Cells[lastRow + 6, 12, lastRow + 6, 14].Style.Font.Bold = true;
                            ws.Cells[lastRow + 6, 12, lastRow + 6, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            ws.Cells[lastRow + 10, 1].Value = "     Vũ Vân Huyền";
                            ws.Cells[lastRow + 10, 1, lastRow + 10, 1].Style.Font.Bold = true;

                            var getListOrg = ds.Tables[5].Rows[0]["LIST_ORG_NAME"];

                            ws.Cells[4, 1].Value = $"{getListOrg.ToString()!.ToUpper()}";

                            // set center for column "STT"
                            ws.Cells[9, 1, lastRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                            // set name for sheet
                            // cái tên ở góc dưới bên trái trong file excel ấy
                            ws.Name = "BC tiền lương và phụ cấp";

                            // set default for round
                            // to column F, G, H, I
                            ws.Cells[9, 6, lastRow, 9].Style.Numberformat.Format = "General";

                            // change string "STT" to "TT" at cell "A6"
                            ws.Cells["A6"].Value = "TT";

                            // set width of column
                            ws.Column(1).Width = 4.17;
                            // ws.Column(4).Width = 9.8;
                            ws.Column(5).Width = 5.69;
                            ws.Column(6).Width = 6.1;
                            ws.Column(7).Width = 5.5;
                            ws.Column(8).Width = 6.5;
                            ws.Column(9).Width = 4;

                            break;
                        }
                    case "PAYROLL_KSPN_REPORT":
                        {
                            for (int i = 1; i <= 16; i++)
                            {
                                ws.Cells[config.HeaderRow + 1, i, config.HeaderRow + 1, i].Value = i;
                            }
                            var referenceData = ds.Tables[0].ToList<PAYROLL_KSPN_REPORT>();
                            rangeAddress = ExcelCellBase.GetAddress(config.HeaderRow + 1, 1, config.HeaderRow + (referenceData.Count != 0 ? referenceData.Count : 1), config.HeaderGridTitles!.Count);
                            var range = ws.Cells[rangeAddress];
                            // thêm dấu phân tách cho các bản ghi
                            ws.Cells[rangeAddress].Style.Numberformat.Format = "###,###,###,###0";

                            // Set borders content
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            // Đổ dữ liệu trả về từ SQL
                            range.LoadFromCollection(referenceData, false);
                            ws.Cells[ws.Dimension.Address].AutoFitColumns();

                            // Tổng
                            int lastRow = config.HeaderRow + (referenceData.Count != 0 ? referenceData.Count + 1 : 1);
                            var totalReferenceData = ds.Tables[1].ToList<PAYROLL_KSPN_REPORT>();
                            rangeAddress = ExcelCellBase.GetAddress(lastRow, 1, lastRow, config.HeaderGridTitles.Count);
                            var totalRange = ws.Cells[rangeAddress];
                            totalRange.LoadFromCollection(totalReferenceData, false);

                            ws.Cells[lastRow, 1, lastRow, 2].Merge = true;
                            ws.Cells[lastRow, 1].Value = (request.Lang == "vi" ? _mls.Single(x => x.KEY == "ER_TOTAL_SUM").VI : (request.Lang == "en" ? _mls.Single(x => x.KEY == "ER_TOTAL_SUM").EN : _mls.Single(x => x.KEY == "ER_TOTAL_SUM").VI));

                            ws.Cells[lastRow, 2].Style.Font.Bold = true;
                            ws.Cells[lastRow, 1, lastRow, config.HeaderGridTitles.Count].Style.Font.Bold = true;
                            ws.Cells[lastRow, 1, lastRow, config.HeaderGridTitles.Count].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ws.Cells[lastRow, 1, lastRow, config.HeaderGridTitles.Count].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            ws.Cells[lastRow, 1, lastRow, config.HeaderGridTitles.Count].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            ws.Cells[lastRow, 1, lastRow, config.HeaderGridTitles.Count].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws.Cells[lastRow, 1, lastRow, config.HeaderGridTitles.Count].Style.Numberformat.Format = "###,###,###,###0";

                            // Footer
                            ws.Cells[lastRow + 2, 13].Value = "Vũng Tàu, ngày ... tháng ... năm ...";
                            ws.Cells[lastRow + 2, 13, lastRow + 2, 15].Style.Font.Italic = true;
                            ws.Cells[lastRow + 2, 13, lastRow + 2, 15].Merge = true;
                            ws.Cells[lastRow + 2, 13, lastRow + 2, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            ws.Cells[lastRow + 3, 3].Value = "Kế toán";
                            ws.Cells[lastRow + 3, 3, lastRow + 3, 4].Style.Font.Bold = true;
                            ws.Cells[lastRow + 3, 3, lastRow + 3, 4].Merge = true;
                            ws.Cells[lastRow + 3, 3, lastRow + 3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            ws.Cells[lastRow + 3, 13].Value = "Giám đốc";
                            ws.Cells[lastRow + 3, 13, lastRow + 3, 15].Style.Font.Bold = true;
                            ws.Cells[lastRow + 3, 13, lastRow + 3, 15].Merge = true;
                            ws.Cells[lastRow + 3, 13, lastRow + 3, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            break;
                        }
                    default:
                        break;
                }

                // header title
                if (config.FooterTitles != null)
                {
                    config.FooterTitles.ForEach(c =>
                    {
                        if (c.Text == "")
                        {
                            ws.Cells[c.Cell].Value = "";
                        }
                        else if (c.Text == "ER_FOOTER_PLACE_DATE_NOW")
                        {
                            string[] dateList = now.ToString("dd/MM/yyyy").Split("/");
                            string text = request.Lang == "vi" ?
                                                            _mls.Single(x => x.KEY == c.Text).VI :
                                                            (request.Lang == "en" ? _mls.Single(x => x.KEY == c.Text).EN : _mls.Single(x => x.KEY == c.Text).VI);
                            int index = 0;
                            foreach (var item in dateList)
                            {
                                text = text.Replace("..." + index.ToString(), item);
                                index++;
                            }
                            ws.Cells[c.Cell].Value = text;
                        }
                        else
                        {
                            string text = request.Lang == "vi" ?
                                                            _mls.Single(x => x.KEY == c.Text).VI :
                                                            (request.Lang == "en" ? _mls.Single(x => x.KEY == c.Text).EN : _mls.Single(x => x.KEY == c.Text).VI);
                            if (text.Contains("...(YYYY)"))
                            {
                                text = text.Replace("...(YYYY)", request.Year.ToString());
                            }
                            if (text.Contains("...(YYYY-1)"))
                            {
                                text = text.Replace("...(YYYY-1)", (request.Year - 1).ToString());
                            }
                            if (text.Contains("...(YYYY+1)"))
                            {
                                text = text.Replace("...(YYYY+1)", (request.Year + 1).ToString());
                            }
                            ws.Cells[c.Cell].Value = text;
                        }
                    });
                }

                // Header title no translate
                if (config.FooterNoTranslates != null)
                {
                    config.FooterNoTranslates.ForEach(c =>
                    {
                        if (c.Text.Contains("...(YYYY)"))
                        {
                            c.Text = c.Text.Replace("...(YYYY)", request.Year.ToString());
                        }
                        else if (c.Text.Contains("...(YYYY-1)"))
                        {
                            c.Text = c.Text.Replace("...(YYYY-1)", (request.Year - 1).ToString());
                        }
                        else if (c.Text.Contains("...(YYYY+1)"))
                        {
                            c.Text = c.Text.Replace("...(YYYY+1)", (request.Year + 1).ToString());
                        }
                        ws.Cells[c.Cell].Value = c.Text;
                    });
                }

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

        public async Task<FormatedResponse> GetListReport()
        {
            var response = await (
                            from l in _dbContext.SysFunctions.AsNoTracking().Where(f => f.PATH_FULL_MATCH == true)
                            from a in _dbContext.AdProgramss.AsNoTracking().Where(a => a.CODE == l.CODE)
                            from m in _dbContext.SysModules.AsNoTracking().Where(m => m.CODE == "re")
                            orderby l.ID ascending
                            select new
                            {
                                Id = l.ID,
                                ModuleId = l.MODULE_ID,
                                ModuleCode = m.CODE,
                                GroupId = l.GROUP_ID,
                                Code = l.CODE,
                                Name = a.NAME,
                                Path = l.PATH,
                                PathFullMatch = l.PATH_FULL_MATCH,
                                StoreName = a.STORE_EXECUTE_IN,
                                Description = a.DESCRIPTION,
                            }).ToListAsync();
            return new() { InnerBody = response };
        }

        public string ConvertNumberToText(double inputNumber, bool suffix = true)
        {
            if (inputNumber == 0)
            {
                return "Không đồng";
            }

            string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };
            bool isNegative = false;

            // -12345678.3445435 => "-12345678"
            string sNumber = inputNumber.ToString("#");
            double number = Convert.ToDouble(sNumber);
            if (number < 0)
            {
                number = -number;
                sNumber = number.ToString();
                isNegative = true;
            }


            int ones, tens, hundreds;

            int positionDigit = sNumber.Length;   // last -> first

            string result = " ";


            if (positionDigit == 0)
                result = unitNumbers[0] + result;
            else
            {
                // 0:       ###
                // 1: nghìn ###,###
                // 2: triệu ###,###,###
                // 3: tỷ    ###,###,###,###
                int placeValue = 0;

                while (positionDigit > 0)
                {
                    // Check last 3 digits remain ### (hundreds tens ones)
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                    positionDigit--;
                    if (positionDigit > 0)
                    {
                        tens = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            hundreds = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                            positionDigit--;
                        }
                    }

                    if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                        result = placeValues[placeValue] + result;

                    placeValue++;
                    if (placeValue > 3) placeValue = 1;

                    if ((ones == 1) && (tens > 1))
                        result = "một " + result;
                    else
                    {
                        if ((ones == 5) && (tens > 0))
                            result = "lăm " + result;
                        else if (ones > 0)
                            result = unitNumbers[ones] + " " + result;
                    }
                    if (tens < 0)
                        break;
                    else
                    {
                        if ((tens == 0) && (ones > 0)) result = "lẻ " + result;
                        if (tens == 1) result = "mười " + result;
                        if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                    }
                    if (hundreds < 0) break;
                    else
                    {
                        if ((hundreds > 0) || (tens > 0) || (ones > 0))
                            result = unitNumbers[hundreds] + " trăm " + result;
                    }
                    result = " " + result;
                }
            }
            result = result.Trim();
            if (isNegative) result = "Âm " + result;
            return result + (suffix ? " đồng chẵn" : "");
        }

        public static string UppercaseFirstCharacter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
