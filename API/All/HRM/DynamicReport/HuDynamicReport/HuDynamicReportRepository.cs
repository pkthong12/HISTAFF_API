using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System.Linq.Dynamic.Core;
using API.All.HRM.DynamicReport.HuDynamicReport;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq.Expressions;
using System.Text.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using MimeKit;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.Style;
using API.All.SYSTEM.CoreAPI.Xlsx;
using CORE.Extension;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml.Vml.Office;
using Aspose.Cells.Tables;
using DocumentFormat.OpenXml.VariantTypes;

namespace API.Controllers.HuDynamicReport
{
    public class HuDynamicReportRepository : IHuDynamicReportRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        private IGenericRepository<HU_DYNAMIC_REPORT, HuDynamicReportDTO> _genericRepository;
        private readonly GenericReducer<HU_DYNAMIC_REPORT, HuDynamicReportDTO> _genericReducer;
        List<MlsData> _mlsData;

        public HuDynamicReportRepository(FullDbContext context, GenericUnitOfWork uow, IWebHostEnvironment env, IOptions<AppSettings> options)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_DYNAMIC_REPORT, HuDynamicReportDTO>();
            _genericReducer = new();
            _env = env;
            _appSettings = options.Value;
            _mlsData = _dbContext.SysLanguages.Select(x => new MlsData()
            {
                Key = x.KEY,
                Vi = x.VI,
                En = x.EN
            }).OrderBy(x => x.Key).ToList();
        }

        public async Task<GenericPhaseTwoListResponse<HuDynamicReportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuDynamicReportDTO> request)
        {
            var joined = from p in _dbContext.HuDynamicReports.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuDynamicReportDTO
                         {
                             Id = p.ID
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            var response = await _genericRepository.ReadAll();
            return response;
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, long value)
        {
            var response = await _genericRepository.ReadAllByKey(key, value);
            return response;
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, string value)
        {
            var response = await _genericRepository.ReadAllByKey(key, value);
            return response;
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<HU_DYNAMIC_REPORT>
                    {
                        (HU_DYNAMIC_REPORT)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new HuDynamicReportDTO
                              {
                                  Id = l.ID,
                                  Json = l.JSON,
                                  Expression = l.EXPRESSION,
                                  ReportName = l.REPORT_NAME,
                                  CreatedDate = l.CREATED_DATE,
                                  CreatedBy = l.CREATED_BY,
                                  UpdatedBy = l.UPDATED_BY,
                                  UpdatedDate = l.UPDATED_DATE,
                                  ViewName = l.VIEW_NAME,
                                  SelectedColumns = l.SELECTED_COLUMNS,
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuDynamicReportDTO dto, string sid)
        {
            try
            {
                var response = await _genericRepository.Create(_uow, dto, sid);
                return response;
            }
            catch
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            
        }

        //public async Task<FormatedResponse> GetAllByFid(long? fid)
        //{
        //    try
        //    {
        //        var response = await _dbContext.HuDynamicReports.Where(x => x.FID == fid).ToListAsync();
        //        return new FormatedResponse() { InnerBody = response, StatusCode = EnumStatusCode.StatusCode200 };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
        //    }
        //}

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuDynamicReportDTO> dtos, string sid)
        {
            var add = new List<HuDynamicReportDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuDynamicReportDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuDynamicReportDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(_uow, dtos, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, long id)
        {
            var response = await _genericRepository.Delete(_uow, id);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<long> ids)
        {
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }

        public virtual async Task<FormatedResponse> GetViewList()
        {
            try
            {
                var list = _dbContext.Model.GetEntityTypes().Where(x => x.ClrType.ToString().StartsWith("API.Entities.REPORT_DATA_")).ToList();

                List<string> res = new();
                list.ForEach(e =>
                {
                    res.Add(e.Name);
                });

                await Task.Run(() => true);
                return new() { InnerBody = res };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public virtual async Task<FormatedResponse> GetColumnList(GetColumnListRequest request)
        {
            try
            {

                var type = _dbContext.Model.FindEntityType(request.ViewName);
                await Task.Run(() => true);

                if (type != null)
                {

                    List<GetColumnListResponse> res = new();

                    var props = type.GetProperties();
                    props?.ToList().ForEach(p =>
                    {
                        if(p.Name != "ORG_ID")
                        {
                            res.Add(new() { ColumnName = p.Name, NetType = p.PropertyInfo?.Name ?? "", ColumnType = p.PropertyInfo!.PropertyType.Name == "Nullable`1" ? "DateTime" : p.PropertyInfo!.PropertyType!.Name });
                        }
                    });

                    return new() { InnerBody = res };

                }
                else
                {
                    return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE };
                }

            }
            catch (Exception ex)
            {
                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public virtual async Task<FormatedResponse> ReadAllByViewName(string? viewName)
        {
            try
            {
                if (!string.IsNullOrEmpty(viewName))
                {
                    var listEntity = await _dbContext.HuDynamicReports.Where(x => x.VIEW_NAME == viewName).ToListAsync();
                    var result = CoreMapper<HuDynamicReportDTO,HU_DYNAMIC_REPORT>.ListEntityToListDTO(listEntity);

                    if (listEntity.Count > 0)
                    {

                        return new() { InnerBody = result };

                    }
                    else
                    {
                        return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE };
                    }
                }
                else
                {
                    return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE };
                }
                

            }
            catch (Exception ex)
            {
                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<MemoryStream> GetListByConditionToExport(DynamicReportDTO? request1)
        {
            try
            {
                if(request1?.Id != null && request1?.Id != 0)
                {
                    var entity = await _dbContext.HuDynamicReports.Where(x => x.ID == request1!.Id).FirstOrDefaultAsync();
                    if (entity != null)
                    {
                        var jsonDocument = JsonDocument.Parse(entity.FORM!);
                        var jsonExpressionParser = new JsonExpressionParser();
                        IPredicateBuilder<object> predicate;
                        Func<object, bool> predicated;

                        switch (entity.VIEW_NAME)
                        {
                            case "API.Entities.REPORT_DATA_STAFF_PROFILE":
                                predicate = new JsonExpressionParser<REPORT_DATA_STAFF_PROFILE>();
                                predicated = predicate.BuildPredicate();
                                Func<REPORT_DATA_STAFF_PROFILE, bool> typedPredicate = item => predicated(item);
                                typedPredicate = jsonExpressionParser.ParsePredicateOf<REPORT_DATA_STAFF_PROFILE>(jsonDocument);
                                List<REPORT_DATA_STAFF_PROFILE> result;
                                List<REPORT_DATA_STAFF_PROFILE> resultTemp = new List<REPORT_DATA_STAFF_PROFILE>();
                                List<REPORT_DATA_STAFF_PROFILE> resultTemp2 = new List<REPORT_DATA_STAFF_PROFILE>();
                                List<string> arraySplit = new List<string>();
                                if (request1?.WorkStatus != null)
                                {
                                    foreach (var item in request1?.WorkStatus!)
                                    {
                                        if (item != null)
                                        {
                                            var sys = _dbContext.SysOtherLists.Where(x => x.ID == Int64.Parse(item)).FirstOrDefault();
                                            arraySplit.Add(sys?.NAME!);
                                        }

                                    }
                                }


                                if (typedPredicate != null)
                                {
                                    result = _dbContext.ReportDataStaffProfiles.Where(typedPredicate).ToList();
                                }
                                else
                                {
                                    result = _dbContext.ReportDataStaffProfiles.ToList();
                                }
                                if (arraySplit != null)
                                {
                                    for (var i = 0; i < arraySplit!.Count; i++)
                                    {
                                        if (arraySplit[i] != null)
                                        {
                                            var listTemp = result.Where(x => x.WORK_STATUS_ID == arraySplit[i]).ToList();
                                            if (listTemp.Any())
                                            {
                                                resultTemp.AddRange(listTemp);
                                            }
                                        }

                                    }
                                }



                                //return new FormatedResponse() { InnerBody = result, StatusCode = EnumStatusCode.StatusCode200 };

                                DynamicReportDTO request = new DynamicReportDTO()
                                {
                                    Id = request1!.Id,
                                    ReportStaffProfile = arraySplit != null ? resultTemp : result
                                };
                                if (request1?.OrgIds != null && request1.OrgIds.Length > 0)
                                {
                                    for (var i = 0; i < request1.OrgIds.Length; i++)
                                    {
                                        var list = request.ReportStaffProfile.Where(x => x.ORG_ID == request1.OrgIds[i]).ToList();
                                        if (list.Any())
                                        {
                                            resultTemp2.AddRange(list);
                                        }
                                    }
                                    request.ReportStaffProfile = resultTemp2;


                                }
                                var fileReturn = await ExportExelDynamicReport(request);
                                return fileReturn;
                            default:
                                throw new ArgumentException("Invalid condition");
                        }


                        //Func<REPORT_DATA_STAFF_PROFILE, bool> predicate = jsonExpressionParser.ParsePredicateOf<REPORT_DATA_STAFF_PROFILE>(jsonDocument);

                        //predicated = jsonExpressionParser.ParsePredicateOf




                    }
                    return null!;
                } else
                {
                    
                    var jsonDocument = JsonDocument.Parse(request1!.QueryForm!);
                    var jsonExpressionParser = new JsonExpressionParser();
                    IPredicateBuilder<object> predicate;
                    Func<object, bool> predicated;

                    switch (request1!.ViewName)
                    {
                        case "API.Entities.REPORT_DATA_STAFF_PROFILE":
                            predicate = new JsonExpressionParser<REPORT_DATA_STAFF_PROFILE>();
                            predicated = predicate.BuildPredicate();
                            Func<REPORT_DATA_STAFF_PROFILE, bool> typedPredicate = item => predicated(item);
                            typedPredicate = jsonExpressionParser.ParsePredicateOf<REPORT_DATA_STAFF_PROFILE>(jsonDocument);
                            List<REPORT_DATA_STAFF_PROFILE> result;
                            List<REPORT_DATA_STAFF_PROFILE> resultTemp = new List<REPORT_DATA_STAFF_PROFILE>();
                            List<REPORT_DATA_STAFF_PROFILE> resultTemp2 = new List<REPORT_DATA_STAFF_PROFILE>();
                            List<string> arraySplit = new List<string>();
                            if (request1?.WorkStatus != null)
                            {
                                foreach (var item in request1?.WorkStatus!)
                                {
                                    if (item != null)
                                    {
                                        var sys = _dbContext.SysOtherLists.Where(x => x.ID == Int64.Parse(item)).FirstOrDefault();
                                        arraySplit.Add(sys?.NAME!);
                                    }

                                }
                            }


                            if (typedPredicate != null)
                            {
                                result = _dbContext.ReportDataStaffProfiles.Where(typedPredicate).ToList();
                            }
                            else
                            {
                                result = _dbContext.ReportDataStaffProfiles.ToList();
                            }
                            if (arraySplit != null)
                            {
                                for (var i = 0; i < arraySplit!.Count; i++)
                                {
                                    if (arraySplit[i] != null)
                                    {
                                        var listTemp = result.Where(x => x.WORK_STATUS_ID == arraySplit[i]).ToList();
                                        if (listTemp.Any())
                                        {
                                            resultTemp.AddRange(listTemp);
                                        }
                                    }

                                }
                            }



                            //return new FormatedResponse() { InnerBody = result, StatusCode = EnumStatusCode.StatusCode200 };

                            DynamicReportDTO request = new DynamicReportDTO()
                            {
                                Id = request1!.Id,
                                ReportStaffProfile = arraySplit != null ? resultTemp : result,
                                ViewName = request1?.ViewName,
                                ReportNameToSave = request1?.ReportNameToSave,
                                ColArrayChanged = request1?.ColArrayChanged,
                                PrefixTrans = request1?.PrefixTrans,
                            };
                            if (request1?.OrgIds != null && request1.OrgIds.Length > 0)
                            {
                                for (var i = 0; i < request1.OrgIds.Length; i++)
                                {
                                    var list = request.ReportStaffProfile.Where(x => x.ORG_ID == request1.OrgIds[i]).ToList();
                                    if (request.ReportStaffProfile.Any())
                                    {
                                        resultTemp2.AddRange(list);
                                    }
                                }
                                request.ReportStaffProfile = resultTemp2;


                            }
                            var fileReturn = await ExportExelDynamicReport(request);
                            return fileReturn;
                        default:
                            throw new ArgumentException("Invalid condition");
                    }


                        //Func<REPORT_DATA_STAFF_PROFILE, bool> predicate = jsonExpressionParser.ParsePredicateOf<REPORT_DATA_STAFF_PROFILE>(jsonDocument);

                        //predicated = jsonExpressionParser.ParsePredicateOf
                    return null!;
                }
                
            }
            catch (Exception ex)
            {
                return null!;
                
            }
        }

        public async Task<MemoryStream> ExportExelDynamicReport(DynamicReportDTO? request)
        {
            try
            {
                if (request!.Id != null && request!.Id != 0)
                {
                    var entity = await _dbContext.HuDynamicReports.Where(x => x.ID == request!.Id).FirstOrDefaultAsync();
                    var type = _dbContext.Model.FindEntityType(entity!.VIEW_NAME!);
                    var matching = entity?.SELECTED_COLUMNS!.Split(";");
                    List<object> listObject = new List<object>();
                    if (request?.ReportStaffProfile?.Count() > 0) //Other report types may be added in the future 
                    {
                        listObject.AddRange(ConvertList(request.ReportStaffProfile));
                    }

                    string _sample = $"BlankWorkbook.xlsx";
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
                    string _path_sample = Path.Combine(location, _sample);
                    //string destPath = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments, $"StorageFile");
                    //if (!Directory.Exists(destPath))
                    //{
                    //    Directory.CreateDirectory(destPath);
                    //}
                    FileInfo file = new FileInfo(_path_sample);
                    using (ExcelPackage package = new ExcelPackage(file))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];

                        worksheet.Cells[2, 1].Value = entity!.REPORT_NAME!;
                        worksheet.Row(2).Style.Font.Bold = true;
                        worksheet.Row(2).Style.Font.Size = 25;
                        worksheet.Cells[2, 1, 2, 5].Merge = true;
                        int begin_row = 4;

                        for (int i = 0; i < matching!.Length; i++)
                        {
                            worksheet.Cells[begin_row, i + 1].Value = Trans(entity.PREFIX_TRANS + matching[i], "vi");
                            worksheet.Cells[begin_row, i + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[begin_row, i + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[begin_row, i + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[begin_row, i + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Row(begin_row).Style.Font.Bold = true;
                            for (var j = 0; j < listObject.Count; j++)
                            {
                                var item = listObject[j].GetType().GetProperties().Where(x => x.Name == matching[i]).FirstOrDefault();
                                var typeItem = item!.PropertyType.Name;
                                object propValue = item!.GetValue(listObject[j], null)!;
                                if (propValue != null)
                                {
                                    if (typeItem == "String" || typeItem == "Number")
                                    {

                                        worksheet.Cells[begin_row + j + 1, i + 1].Value = propValue;

                                    }
                                    else
                                    {
                                        worksheet.Cells[begin_row + j + 1, i + 1].Value = DateTime.Parse(propValue.ToString()!).ToString("dd/MM/yyyy");
                                    }
                                }


                                worksheet.Cells[begin_row + j + 1, i + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[begin_row + j + 1, i + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[begin_row + j + 1, i + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[begin_row + j + 1, i + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                worksheet.Column(i + 1).AutoFit();
                            }

                        }

                        var stream = new MemoryStream(package.GetAsByteArray());

                        return await Task.Run(() => stream);
                    }
                }
                else
                {
                    var type = _dbContext.Model.FindEntityType(request!.ViewName!);
                    var matching = request?.ColArrayChanged!.Split(";");
                    List<object> listObject = new List<object>();
                    if (request?.ReportStaffProfile?.Count() > 0) //Other report types may be added in the future 
                    {
                        listObject.AddRange(ConvertList(request.ReportStaffProfile));
                    }

                    string _sample = $"BlankWorkbook.xlsx";
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
                    string _path_sample = Path.Combine(location, _sample);
                    //string destPath = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments, $"StorageFile");
                    //if (!Directory.Exists(destPath))
                    //{
                    //    Directory.CreateDirectory(destPath);
                    //}
                    FileInfo file = new FileInfo(_path_sample);
                    using (ExcelPackage package = new ExcelPackage(file))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];

                        worksheet.Cells[2, 1].Value = request!.ReportNameToSave!;
                        worksheet.Cells[2, 1, 2, 5].Merge = true;
                        worksheet.Row(2).Style.Font.Bold = true;
                        worksheet.Row(2).Style.Font.Size = 25;
                        int begin_row = 4;

                        for (int i = 0; i < matching!.Length; i++)
                        {
                            worksheet.Cells[begin_row, i + 1].Value = Trans(request.PrefixTrans + matching[i], "vi");
                            worksheet.Cells[begin_row, i + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[begin_row, i + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[begin_row, i + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[begin_row, i + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Row(begin_row).Style.Font.Bold = true;
                            for (var j = 0; j < listObject.Count; j++)
                            {
                                var item = listObject[j].GetType().GetProperties().Where(x => x.Name == matching[i]).FirstOrDefault();
                                var typeItem = item!.PropertyType.Name;
                                object propValue = item!.GetValue(listObject[j], null)!;
                                if (propValue != null)
                                {
                                    if (typeItem == "String" || typeItem == "Number")
                                    {

                                        worksheet.Cells[begin_row + j + 1, i + 1].Value = propValue;

                                    }
                                    else
                                    {
                                        worksheet.Cells[begin_row + j + 1, i + 1].Value = DateTime.Parse(propValue.ToString()!).ToString("dd/MM/yyyy");
                                    }
                                }


                                worksheet.Cells[begin_row + j + 1, i + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[begin_row + j + 1, i + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[begin_row + j + 1, i + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[begin_row + j + 1, i + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                worksheet.Column(i + 1).AutoFit();
                            }

                        }

                        var stream = new MemoryStream(package.GetAsByteArray());

                        return await Task.Run(() => stream);
                    }
                }
                
            }
            catch (Exception ex)
            {
                return null!;
            }
        }

        public static List<object> ConvertList<T>(List<T> inputList)
        {
            return inputList.Cast<object>().ToList();
        }

        private string Trans(string key, string lang)
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

