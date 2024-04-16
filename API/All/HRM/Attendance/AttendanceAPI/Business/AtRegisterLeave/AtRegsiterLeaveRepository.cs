using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Microsoft.EntityFrameworkCore;
using Azure;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Common.Extensions;
using Common.Interfaces;
using Common.DataAccess;
using System.Reflection.Emit;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProfileDAL.ViewModels;
using System;
using CORE.Services.File;
using DocumentFormat.OpenXml.Wordprocessing;
using OfficeOpenXml.Style.XmlAccess;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Dynamic;

namespace API.Controllers.AtRegisterLeave
{
    public class AtRegisterLeaveRepository : IAtRegisterLeaveRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_REGISTER_LEAVE, AtRegisterLeaveDTO> _genericRepository;
        private readonly GenericReducer<AT_REGISTER_LEAVE, AtRegisterLeaveDTO> _genericReducer;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;
        protected AbsQueryDataTemplate QueryData;
        public AtRegisterLeaveRepository(FullDbContext context, GenericUnitOfWork uow, IWebHostEnvironment env,
            IOptions<AppSettings> options,
            IFileService fileService)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_REGISTER_LEAVE, AtRegisterLeaveDTO>();
            _genericReducer = new();
            _env = env;
            _appSettings = options.Value;
            _fileService = fileService;
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }

        public async Task<GenericPhaseTwoListResponse<AtRegisterLeaveDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtRegisterLeaveDTO> request)
        {

            DateTime endDate = new DateTime(3000, 1, 1);
            DateTime startDate = new DateTime();
            if (request.Filter != null)
            {
                if (request.Filter.PeriodId != null)
                {
                    var objPerriod = _dbContext.AtSalaryPeriods.Where(x => x.ID == request.Filter.PeriodId).FirstOrDefault();
                    endDate = objPerriod.END_DATE;
                    startDate = objPerriod.START_DATE;
                    request.Filter.PeriodId = null;
                }
                if (request.Filter.Id != null)
                {
                    request.Filter.Id = null;
                }
            }


            var joined = from p in _dbContext.AtRegisterLeaves.AsNoTracking().Where(x => x.DATE_START!.Value.Date >= startDate.Date && x.DATE_END!.Value.Date <= endDate.Date
                         || (startDate.Date >= x.DATE_START.Value.Date && startDate.Date <= x.DATE_END!.Value.Date) 
                         || (endDate.Date >= x.DATE_START.Value.Date && startDate.Date <= x.DATE_END!.Value.Date))
                         from e in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                         //from ecv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from ot in _dbContext.AtTimeTypes.Where(x => x.ID == p.TYPE_ID).DefaultIfEmpty()
                         from to in _dbContext.SysOtherLists.Where(x => x.ID == p.TYPE_OFF).DefaultIfEmpty()
                         from po in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.AsNoTracking().Where(x => x.ID == po.JOB_ID).DefaultIfEmpty()

                         select new AtRegisterLeaveDTO
                         {
                             Id = p.ID,
                             EmployeeId = p.EMPLOYEE_ID,
                             EmployeeCode = e.CODE,
                             JobOrderNum = (int)(j.ORDERNUM ?? 999),
                             EmployeeName = e.Profile.FULL_NAME,
                             DateStart = p.DATE_START,
                             DateEnd = p.DATE_END,
                             TypeId = p.TYPE_ID,
                             TypeName = ot.NAME,
                             FileName = p.FILE_NAME,
                             Reason = p.REASON,
                             Note = p.NOTE,
                             OrgId = e.ORG_ID,
                             OrgName = o.NAME,
                             PositionName = po.NAME,
                             TypeOff = p.TYPE_OFF,
                             TypeOffName = to.NAME
                         };
            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, long value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return new() { InnerBody = resposne };
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, string value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return new() { InnerBody = resposne };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<AT_REGISTER_LEAVE>
                    {
                        (AT_REGISTER_LEAVE)response
                    };
                var joined = (from p in list
                              select new AtRegisterLeaveDTO
                              {
                                  Id = p.ID,
                                  EmployeeId = p.EMPLOYEE_ID,
                                  DateStart = p.DATE_START,
                                  DateEnd = p.DATE_END,
                                  TypeId = p.TYPE_ID,
                                  FileName = p.FILE_NAME,
                                  Reason = p.REASON,
                                  Note = p.NOTE,
                                  TypeOff = p.TYPE_OFF,
                                  IsEachDay= p.IS_EACH_DAY
                              }).FirstOrDefault();


                if (joined != null)
                {
                    if (res.StatusCode == EnumStatusCode.StatusCode200)
                    {
                        var child = (from e in _dbContext.HuEmployees.Where(x => x.ID == joined.EmployeeId).DefaultIfEmpty()
                                     from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                     from ecv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                     from ot in _dbContext.AtTimeTypes.Where(x => x.ID == joined.TypeId).DefaultIfEmpty()
                                     from to in _dbContext.SysOtherLists.Where(x => x.ID == joined.TypeOff).DefaultIfEmpty()
                                     from po in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                     select new
                                     {
                                         EmployeeCode = e.CODE,
                                         EmployeeName = ecv.FULL_NAME,
                                         TypeName = ot.NAME,
                                         OrgId = e.ORG_ID,
                                         OrgName = o.NAME,
                                         PositionName = po.NAME,
                                         TypeOffName = to.NAME
                                     }).FirstOrDefault();
                        joined.EmployeeCode = child?.EmployeeCode;
                        joined.EmployeeName = child?.EmployeeName;
                        joined.TypeName = child?.TypeName;
                        joined.OrgId = child?.OrgId;
                        joined.OrgName = child?.OrgName;
                        joined.PositionName = child?.PositionName;
                        joined.TypeOffName = child?.TypeOffName;
                        return new FormatedResponse() { InnerBody = joined };
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.GET_LIST_BY_KEY_RESPOSNE_IS_NOT_OF_STATUS_CODE_200, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
                else
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.JOINED_QUERY_AFTER_GET_BY_ID_WAS_NULL, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }



        public async Task<FormatedResponse> GetByIdVer2(long id)
        {
            try
            {
                var lstDetail = _dbContext.AtRegisterLeaveDetails.Where(x => x.REGISTER_ID == id).ToList();
                var joined = await (from p in _dbContext.AtRegisterLeaves.AsNoTracking().Where(x => x.ID == id)
                              from e in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                              from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                              from ecv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                              from ot in _dbContext.AtTimeTypes.Where(x => x.ID == p.TYPE_ID).DefaultIfEmpty()
                              from to in _dbContext.SysOtherLists.Where(x => x.ID == p.TYPE_OFF).DefaultIfEmpty()
                              from po in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()

                              select new
                              {
                                  Id = p.ID,
                                  EmployeeId = p.EMPLOYEE_ID,
                                  EmployeeCode = e.CODE,
                                  EmployeeName = ecv.FULL_NAME,
                                  DateStart = p.DATE_START,
                                  DateEnd = p.DATE_END,
                                  TypeId = p.TYPE_ID,
                                  TypeName = ot.NAME,
                                  FileName = p.FILE_NAME,
                                  Reason = p.REASON,
                                  Note = p.NOTE,
                                  OrgId = e.ORG_ID,
                                  OrgName = o.NAME,
                                  PositionName = po.NAME,
                                  TypeOff = p.TYPE_OFF,
                                  TypeOffName = to.NAME,
                                  IsEachDay = p.IS_EACH_DAY
                              }).ToListAsync();

                dynamic objReturn = new ExpandoObject();

                if (joined != null && joined.Any())
                {
                    var firstItem = joined.First();

                    objReturn.employeeId = firstItem.EmployeeId;
                    objReturn.id = firstItem.Id;
                    objReturn.employeeName = firstItem.EmployeeName;
                    objReturn.positionName = firstItem.PositionName;
                    objReturn.typeId = firstItem.TypeId;
                    objReturn.typeOff = firstItem.TypeOff;
                    objReturn.dateStart = firstItem.DateStart;
                    objReturn.dateEnd = firstItem.DateEnd;
                    objReturn.reason = firstItem.Reason;
                    objReturn.note = firstItem.Note;
                    objReturn.isEachDay = firstItem.IsEachDay;
                    objReturn.orgName = firstItem.OrgName;
                    objReturn.employeeCode = firstItem.EmployeeCode;

                    for (int i = 0; i < lstDetail.Count; i++)
                    {
                        if (objReturn != null)
                        {
                            ((IDictionary<string, object>)objReturn)["dateStart" + (i + 1)] = lstDetail[i].LEAVE_DATE;
                            ((IDictionary<string, object>)objReturn)["dType" + (i + 1)] = lstDetail[i].TYPE_OFF;
                            ((IDictionary<string, object>)objReturn)["shiftCode" + (i + 1)] = _dbContext.AtTimeTypes.FirstOrDefault(x => x.ID == lstDetail[i].MANUAL_ID)?.CODE;
                            ((IDictionary<string, object>)objReturn)["day" + (i + 1)] = lstDetail[i].NUMBER_DAY;
                        }
                    }
                }

                return new FormatedResponse() { InnerBody = objReturn };
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error
                Console.WriteLine($"An error occurred: {ex.Message}");
                // You can also throw the exception or return an error response if needed

                // Return a default or error response
                return new FormatedResponse() { InnerBody = null };
            }
        }
        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtRegisterLeaveDTO dto, string sid)
        {

            var response = await _genericRepository.Create(_uow, dto, sid);

            return new FormatedResponse()
            {
                MessageCode = response.MessageCode,
                InnerBody = response,
                StatusCode = response.StatusCode
            };
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtRegisterLeaveDTO> dtos, string sid)
        {
            var add = new List<AtRegisterLeaveDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtRegisterLeaveDTO dto, string sid, bool patchMode = true)
        {
            try
            {
                _uow.CreateTransaction();
                List<UploadFileResponse> uploadFiles = new();
                // First of all we need to upload all the attachments
                if (dto.FirstAttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = dto.FirstAttachmentBuffer.ClientFileName,
                        ClientFileType = dto.FirstAttachmentBuffer.ClientFileType,
                        ClientFileData = dto.FirstAttachmentBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(AtRegisterLeaveDTO).GetProperty("FileName");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment" };
                    }

                }

                // Check đóng/mở kỳ công 
                // get month + year datestart, dateend
                int monthStartDate = dto.DateStart!.Value.Month;
                int monthEndDate = dto.DateEnd!.Value.Month;
                int yearStartDate = dto.DateStart!.Value.Year;
                int yearEndDate = dto.DateEnd!.Value.Year;

                if (yearStartDate == yearEndDate)
                {
                    for (int i = monthStartDate; i <= monthEndDate; i++)
                    {
                        var salPeriod = await _dbContext.AtSalaryPeriods.Where(s => s.MONTH == i && s.YEAR == yearStartDate).FirstOrDefaultAsync();
                        var org = await _dbContext.HuEmployees.Where(e => e.ID == dto.EmployeeId).FirstOrDefaultAsync();
                        var checkLockOrgDateStart = (_dbContext.AtOrgPeriods.Where(p => p.ORG_ID == org!.ORG_ID && p.STATUSCOLEX == 1 && p.PERIOD_ID == salPeriod!.ID)).Count();
                        if (checkLockOrgDateStart != 0)
                        {
                            return new FormatedResponse() { MessageCode = CommonMessageCode.THE_WORK_PERIOD_HAS_CEASED_TO_APPLY, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                    }
                }

                var listCheck = _dbContext.AtRegisterLeaves.Where(x => x.EMPLOYEE_ID == dto.EmployeeId && x.ID != dto.Id).ToList();
                bool isValid = true;
                foreach (var itemCheck in listCheck)
                {
                    if (dto.DateStart <= dto.DateEnd && dto.DateEnd < itemCheck.DATE_START)
                    {
                        isValid = true;
                    }
                    else if (dto.DateStart <= dto.DateEnd && dto.DateStart > itemCheck.DATE_END)
                    {
                        isValid = true;
                    }
                    else
                    {
                        isValid = false; break;
                    }
                }
                //foreach (var itemCheck in listCheck)
                //{
                //    if (dto.DATE_START <= dto.DATE_END && dto.DATE_END < itemCheck.DATE_START)
                //    {
                //        isValid = true;
                //    }
                //    else if (dto.DATE_START <= dto.DATE_END && dto.DATE_START > itemCheck.DATE_END)
                //    {
                //        isValid = true;
                //    }
                //    else
                //    {
                //        isValid = false; break;
                //    }
                //}
                if (isValid == false)
                {
                    return new FormatedResponse()
                    {
                        MessageCode = CommonMessageCode.EMPLOYEE_HAVE_EXIST_TIME_REGISTER,
                        InnerBody = dto,
                        StatusCode = EnumStatusCode.StatusCode500
                    };
                }
                var checkValidate = await QueryData.ExecuteList("CHECK_VALIDATE_REGISTER_LEAVE",
                 new
                 {
                     P_EMPLOYEEID = dto.EmployeeId,
                     P_LEAVEFROM = dto.DateStart.Value.ToString("dd/MM/yyyy"),
                     P_LEAVETO = dto.DateEnd.Value.ToString("dd/MM/yyyy"),
                     P_MANUALID = dto.TypeId,
                     P_LEAVEID = 0,
                     P_LEAVE_TYPE = dto.TypeOff,
                     P_DAYNUM = 0,
                 }, false);
                if (checkValidate != null)
                {
                    var ListValidates = checkValidate.Select(c => (string?)((dynamic)c).MESSAGECODE).FirstOrDefault();
                    if (ListValidates != null && ListValidates != "")
                    {
                        return new FormatedResponse()
                        {
                            MessageCode = ListValidates,
                            InnerBody = dto,
                            StatusCode = EnumStatusCode.StatusCode500
                        };
                    }
                    //var StatusCode = ListValidates.STATUSCODE;

                }

                var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
                var lstDetail = _dbContext.AtRegisterLeaveDetails.Where(x => x.REGISTER_ID == dto.Id).ToList();

                _dbContext.RemoveRange(lstDetail);

                for (int i = 0; i <= (dto.DateEnd - dto.DateStart).Value.Days; i++)
                {
                    var dataDetail = new AT_REGISTER_LEAVE_DETAIL();
                    dataDetail.REGISTER_ID = (long)dto.Id;
                    dataDetail.LEAVE_DATE = dto.DateStart.Value.AddDays(i);
                    await _dbContext.AtRegisterLeaveDetails.AddAsync(dataDetail);
                }
                await _dbContext.SaveChangesAsync();
                _uow.Commit();
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

        }


        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtRegisterLeaveDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(_uow, dtos, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, long id)
        {
            var response = await _genericRepository.Delete(_uow, id);
            var lstDetail = _dbContext.AtRegisterLeaveDetails.Where(x => x.REGISTER_ID == id).ToList();

            _dbContext.RemoveRange(lstDetail);
            _dbContext.SaveChanges();
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

            var lstDetail = _dbContext.AtRegisterLeaveDetails.Where(x => ids.Contains(x.REGISTER_ID)).ToList();

            _dbContext.RemoveRange(lstDetail);
            _dbContext.SaveChanges();
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }


        public async Task<FormatedResponse> CreateAsync(GenericUnitOfWork _uow, AtRegisterLeaveDTO dto, string sid)
        {
            try
            {
                // check nhan vien thu viec
                foreach (var e in dto.EmployeeIds!)
                {
                    var contractType = await (from c in _dbContext.HuContracts.AsNoTracking().Where(c => c.EMPLOYEE_ID == e && c.START_DATE <= dto.DateStart && dto.DateStart <= c.EXPIRE_DATE).DefaultIfEmpty()
                                              from t in _dbContext.HuContractTypes.AsNoTracking().Where(t => t.ID == c.CONTRACT_TYPE_ID).DefaultIfEmpty()
                                              from s in _dbContext.SysContractTypes.AsNoTracking().Where(s => s.ID == t.TYPE_ID).DefaultIfEmpty()
                                              orderby c.ID descending
                                              select new
                                              {
                                                  Code = s.CODE
                                              }).FirstOrDefaultAsync();
                    var manual = await _dbContext.AtTimeTypes.Where(t => t.ID == dto.TypeId).FirstOrDefaultAsync();
                    if (contractType != null && contractType!.Code == "HDTV")
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMPLOYEE_HAVE_PROBATIONARY_CONTRACT, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }

                _uow.CreateTransaction();
                List<UploadFileResponse> uploadFiles = new();
                // First of all we need to upload all the attachments
                if (dto.FirstAttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = dto.FirstAttachmentBuffer.ClientFileName,
                        ClientFileType = dto.FirstAttachmentBuffer.ClientFileType,
                        ClientFileData = dto.FirstAttachmentBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(AtRegisterLeaveDTO).GetProperty("FileName");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment" };
                    }

                }

                // Check đóng/mở kỳ công 
                foreach (var empID in dto.EmployeeIds)
                {
                    // get month + year datestart, dateend
                    int monthStartDate = dto.DateStart!.Value.Month;
                    int monthEndDate = dto.DateEnd!.Value.Month;
                    int yearStartDate = dto.DateStart!.Value.Year;
                    int yearEndDate = dto.DateEnd!.Value.Year;

                    if (yearStartDate == yearEndDate)
                    {
                        for (int i = monthStartDate; i <= monthEndDate; i++)
                        {
                            var salPeriod = await _dbContext.AtSalaryPeriods.Where(s => s.MONTH == i && s.YEAR == yearStartDate).FirstOrDefaultAsync();
                            var org = await _dbContext.HuEmployees.Where(e => e.ID == empID).FirstOrDefaultAsync();
                            var checkLockOrgDateStart = (_dbContext.AtOrgPeriods.Where(p => p.ORG_ID == org!.ORG_ID && p.STATUSCOLEX == 1 && p.PERIOD_ID == salPeriod!.ID)).Count();
                            if (checkLockOrgDateStart != 0)
                            {
                                return new FormatedResponse() { MessageCode = CommonMessageCode.THE_WORK_PERIOD_HAS_CEASED_TO_APPLY, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                            }
                        }
                    }
                }

                foreach (var item in dto.EmployeeIds)
                {
                    var data = new AT_REGISTER_LEAVE();
                    data.TYPE_ID = dto.TypeId;
                    data.TYPE_OFF = dto.TypeOff;
                    data.DATE_END = dto.DateEnd;
                    data.DATE_START = (DateTime)dto.DateStart;
                    data.NOTE = dto.Note;
                    data.REASON = dto.Reason;
                    data.EMPLOYEE_ID = item;
                    data.FILE_NAME = dto.FileName;
                    data.CREATED_BY = sid;
                    data.CREATED_DATE = DateTime.Now;
                    data.UPDATED_BY = sid;
                    data.UPDATED_DATE = DateTime.Now;
                    // check trùng ngày
                    var listCheck = _dbContext.AtRegisterLeaves.Where(x => x.EMPLOYEE_ID == data.EMPLOYEE_ID).ToList();
                    bool isValid = true;
                    foreach (var itemCheck in listCheck)
                    {
                        if (data.DATE_START <= data.DATE_END && data.DATE_END < itemCheck.DATE_START)
                        {
                            isValid = true;
                        }
                        else if (data.DATE_START <= data.DATE_END && data.DATE_START > itemCheck.DATE_END)
                        {
                            isValid = true;
                        }
                        else
                        {
                            isValid = false; break;
                        }
                    }
                    if (isValid == false)
                    {
                        return new FormatedResponse()
                        {
                            MessageCode = CommonMessageCode.EMPLOYEE_HAVE_EXIST_TIME_REGISTER,
                            InnerBody = dto,
                            StatusCode = EnumStatusCode.StatusCode500
                        };
                    }
                    var checkValidate = await QueryData.ExecuteList("CHECK_VALIDATE_REGISTER_LEAVE",
                     new
                     {
                         P_EMPLOYEEID = item,
                         P_LEAVEFROM = dto.DateStart.Value.ToString("dd/MM/yyyy"),
                         P_LEAVETO = dto.DateEnd.Value.ToString("dd/MM/yyyy"),
                         P_MANUALID = dto.TypeId,
                         P_LEAVEID = 0,
                         P_LEAVE_TYPE = dto.TypeOff,
                         P_DAYNUM = 0,
                     }, false);
                    if (checkValidate != null)
                    {
                        var ListValidates = checkValidate.Select(c => (string?)((dynamic)c).MESSAGECODE).FirstOrDefault();
                        if (ListValidates != null && ListValidates != "")
                        {
                            return new FormatedResponse()
                            {
                                MessageCode = ListValidates,
                                InnerBody = dto,
                                StatusCode = EnumStatusCode.StatusCode500
                            };
                        }
                        //var StatusCode = ListValidates.STATUSCODE;

                    }
                    await _dbContext.AtRegisterLeaves.AddAsync(data);
                    await _dbContext.SaveChangesAsync();

                    var obj = _dbContext.AtRegisterLeaves.Where(x => x.EMPLOYEE_ID == data.EMPLOYEE_ID && x.DATE_START == data.DATE_START && x.DATE_END == data.DATE_END && x.TYPE_OFF == data.TYPE_OFF && x.TYPE_ID == data.TYPE_ID).FirstOrDefault();
                    if (obj != null)
                    {
                        for (int i = 0; i <= (obj.DATE_END - obj.DATE_START).Value.Days; i++)
                        {
                            var dataDetail = new AT_REGISTER_LEAVE_DETAIL();
                            dataDetail.REGISTER_ID = obj.ID;
                            dataDetail.LEAVE_DATE = obj.DATE_START.Value.AddDays(i);
                            await _dbContext.AtRegisterLeaveDetails.AddAsync(dataDetail);
                        }
                        await _dbContext.SaveChangesAsync();
                    }
                }
                _uow.Commit();
                return new() { InnerBody = dto, MessageCode = CommonMessageCode.CREATE_SUCCESS };

            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }



        public async Task<FormatedResponse> CreateVer2Async(GenericUnitOfWork _uow, DynamicDTO model, string sid)
        {
            try
            {
                // check nhan vien thu viec

                var contractType = await (from c in _dbContext.HuContracts.AsNoTracking().Where(c => c.EMPLOYEE_ID == (long)model["employeeId"] && c.START_DATE <= DateTime.Parse(model["dateStart"].ToString()) && DateTime.Parse(model["dateEnd"].ToString()) <= c.EXPIRE_DATE).DefaultIfEmpty()
                                          from t in _dbContext.HuContractTypes.AsNoTracking().Where(t => t.ID == c.CONTRACT_TYPE_ID).DefaultIfEmpty()
                                          from s in _dbContext.SysContractTypes.AsNoTracking().Where(s => s.ID == t.TYPE_ID).DefaultIfEmpty()
                                          orderby c.ID descending
                                          select new
                                          {
                                              Code = s.CODE
                                          }).FirstOrDefaultAsync();
                if (contractType != null && contractType!.Code == "HDTV")
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMPLOYEE_HAVE_PROBATIONARY_CONTRACT, StatusCode = EnumStatusCode.StatusCode400 };
                }

                // check nhan vien da dc xep ca hay chua 
                DateTime startDate = DateTime.Parse(model["dateStart"].ToString()!).Date;
                DateTime endDate = DateTime.Parse(model["dateEnd"].ToString()!).Date;
                TimeSpan range = endDate.Subtract(startDate);
                var checkExisistShift = await _dbContext.AtWorksigns.Where(w => w.EMPLOYEE_ID == (long)model["employeeId"] && DateTime.Parse(model["dateStart"].ToString()!).Date <= w.WORKINGDAY!.Value.Date && w.WORKINGDAY!.Value.Date <= DateTime.Parse(model["dateEnd"].ToString()!).Date).CountAsync();
                if (checkExisistShift != range.TotalDays + 1)
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMPLOYEE_NOT_HAVE_SHIFT, StatusCode = EnumStatusCode.StatusCode400 };
                }


                // get month + year datestart, dateend
                int monthStartDate = DateTime.Parse(model["dateStart"].ToString()).Month;
                int monthEndDate = DateTime.Parse(model["dateEnd"].ToString()).Month;
                int yearStartDate = DateTime.Parse(model["dateStart"].ToString()).Year;
                int yearEndDate = DateTime.Parse(model["dateEnd"].ToString()).Year;

                if (yearStartDate == yearEndDate)
                {
                    for (int i = monthStartDate; i <= monthEndDate; i++)
                    {
                        var salPeriod = await _dbContext.AtSalaryPeriods.Where(s => s.MONTH == i && s.YEAR == yearStartDate).FirstOrDefaultAsync();
                        var org = await _dbContext.HuEmployees.Where(e => e.ID == (long)model["employeeId"]).FirstOrDefaultAsync();
                        var checkLockOrgDateStart = (_dbContext.AtOrgPeriods.Where(p => p.ORG_ID == org!.ORG_ID && p.STATUSCOLEX == 1 && p.PERIOD_ID == salPeriod!.ID)).Count();
                        if (checkLockOrgDateStart != 0)
                        {
                            return new FormatedResponse() { MessageCode = CommonMessageCode.THE_WORK_PERIOD_HAS_CEASED_TO_APPLY, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                    }
                }

                var data = new AT_REGISTER_LEAVE();
                data.TYPE_ID = (long?)model["typeId"];
                data.TYPE_OFF = (long?)model["typeOff"];
                data.DATE_END = DateTime.Parse(model["dateEnd"].ToString());
                data.DATE_START = DateTime.Parse(model["dateStart"].ToString());
                data.NOTE = (string?)model["note"];
                data.REASON = (string?)model["reason"];
                data.EMPLOYEE_ID = (long)model["employeeId"];
                data.IS_EACH_DAY = (bool)model["isEachDay"];
                data.CREATED_BY = sid;
                data.CREATED_DATE = DateTime.Now;
                data.UPDATED_BY = sid;
                data.UPDATED_DATE = DateTime.Now;

                var listCheck = _dbContext.AtRegisterLeaves.Where(x => x.EMPLOYEE_ID == data.EMPLOYEE_ID).ToList();
                bool isValid = true;
                foreach (var itemCheck in listCheck)
                {
                    if (data.DATE_START <= data.DATE_END && data.DATE_END < itemCheck.DATE_START)
                    {
                        isValid = true;
                    }
                    else if (data.DATE_START <= data.DATE_END && data.DATE_START > itemCheck.DATE_END)
                    {
                        isValid = true;
                    }
                    else
                    {
                        isValid = false; break;
                    }
                }
                if (isValid == false)
                {
                    return new FormatedResponse()
                    {
                        MessageCode = CommonMessageCode.EMPLOYEE_HAVE_EXIST_TIME_REGISTER,
                        InnerBody = model,
                        StatusCode = EnumStatusCode.StatusCode500
                    };
                }
                var checkValidate = await QueryData.ExecuteList("CHECK_VALIDATE_REGISTER_LEAVE",
                 new
                 {
                     P_EMPLOYEEID = data.EMPLOYEE_ID,
                     P_LEAVEFROM = data.DATE_START.Value.ToString("dd/MM/yyyy"),
                     P_LEAVETO = data.DATE_END.Value.ToString("dd/MM/yyyy"),
                     P_MANUALID = data.TYPE_ID,
                     P_LEAVEID = 0,
                     P_LEAVE_TYPE = data.TYPE_OFF,
                     P_DAYNUM = 0,
                 }, false);
                if (checkValidate != null)
                {
                    var ListValidates = checkValidate.Select(c => (string?)((dynamic)c).MESSAGECODE).FirstOrDefault();
                    if (ListValidates != null && ListValidates != "")
                    {
                        return new FormatedResponse()
                        {
                            MessageCode = ListValidates,
                            InnerBody = model,
                            StatusCode = EnumStatusCode.StatusCode500
                        };
                    }
                }
                await _dbContext.AtRegisterLeaves.AddAsync(data);
                await _dbContext.SaveChangesAsync();


                var obj = _dbContext.AtRegisterLeaves.Where(x => x.EMPLOYEE_ID == data.EMPLOYEE_ID && x.DATE_START == data.DATE_START && x.DATE_END == data.DATE_END && x.TYPE_OFF == data.TYPE_OFF && x.TYPE_ID == data.TYPE_ID && x.IS_EACH_DAY == data.IS_EACH_DAY).FirstOrDefault();

                if (obj != null)
                {

                    try
                    {
                        if (obj.IS_EACH_DAY ?? false)
                        {
                            for (int i = 0; i <= (obj.DATE_END - obj.DATE_START).Value.Days; i++)
                            {

                                if (model["dType" + (i + 1)] == null)
                                {
                                    _dbContext.AtRegisterLeaves.Remove(obj);
                                    await _dbContext.SaveChangesAsync();
                                    return new FormatedResponse() { MessageCode = CommonMessageCode.REGISTER_LEAVE_DETAIL_NULL, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                                }
                                else
                                {

                                    var manualId = _dbContext.AtTimeTypes.Where(x => x.CODE == model["shiftCode" + (i + 1)].ToString()).FirstOrDefault();
                                    if (manualId != null)
                                    {
                                        var dataDetail = new AT_REGISTER_LEAVE_DETAIL();
                                        dataDetail.REGISTER_ID = obj.ID;
                                        dataDetail.LEAVE_DATE = obj.DATE_START.Value.AddDays(i);
                                        dataDetail.MANUAL_ID = manualId.ID;
                                        dataDetail.NUMBER_DAY = decimal.Parse(model["day" + (i + 1)].ToString());
                                        dataDetail.TYPE_OFF = ((long?)model["dType" + (i + 1)]);
                                        await _dbContext.AtRegisterLeaveDetails.AddAsync(dataDetail);
                                    }
                                    else
                                    {
                                        _dbContext.AtRegisterLeaves.Remove(obj);
                                        await _dbContext.SaveChangesAsync();
                                        return new FormatedResponse() { MessageCode = CommonMessageCode.REGISTER_LEAVE_DETAIL_TIMETYPE_NULL, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                                    }

                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; i <= (obj.DATE_END - obj.DATE_START).Value.Days; i++)
                            {
                                var dataDetail = new AT_REGISTER_LEAVE_DETAIL();
                                dataDetail.REGISTER_ID = obj.ID;
                                dataDetail.LEAVE_DATE = obj.DATE_START.Value.AddDays(i);
                                dataDetail.NUMBER_DAY = 1;
                                dataDetail.MANUAL_ID = obj.TYPE_ID;
                                dataDetail.TYPE_OFF = obj.TYPE_OFF;

                                await _dbContext.AtRegisterLeaveDetails.AddAsync(dataDetail);
                            }
                        }

                        await _dbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _dbContext.AtRegisterLeaves.Remove(obj);
                        await _dbContext.SaveChangesAsync();
                        return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }


                }

                return new() { InnerBody = model, MessageCode = CommonMessageCode.CREATE_SUCCESS };

            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

            }

        }

        public async Task<FormatedResponse> UpdateVer2(GenericUnitOfWork _uow, DynamicDTO model, string sid, bool patchMode = true)
        {
            try
            {

                // check nhan vien thu viec

                var contractType = await (from c in _dbContext.HuContracts.AsNoTracking().Where(c => c.EMPLOYEE_ID == (long)model["employeeId"] && c.START_DATE <= DateTime.Parse(model["dateStart"].ToString()) && DateTime.Parse(model["dateEnd"].ToString()) <= c.EXPIRE_DATE).DefaultIfEmpty()
                                          from t in _dbContext.HuContractTypes.AsNoTracking().Where(t => t.ID == c.CONTRACT_TYPE_ID).DefaultIfEmpty()
                                          from s in _dbContext.SysContractTypes.AsNoTracking().Where(s => s.ID == t.TYPE_ID).DefaultIfEmpty()
                                          orderby c.ID descending
                                          select new
                                          {
                                              Code = s.CODE
                                          }).FirstOrDefaultAsync();
                if (contractType != null && contractType!.Code == "HDTV")
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMPLOYEE_HAVE_PROBATIONARY_CONTRACT, StatusCode = EnumStatusCode.StatusCode400 };
                }


                // get month + year datestart, dateend
                int monthStartDate = DateTime.Parse(model["dateStart"].ToString()).Month;
                int monthEndDate = DateTime.Parse(model["dateEnd"].ToString()).Month;
                int yearStartDate = DateTime.Parse(model["dateStart"].ToString()).Year;
                int yearEndDate = DateTime.Parse(model["dateEnd"].ToString()).Year;

                if (yearStartDate == yearEndDate)
                {
                    for (int i = monthStartDate; i <= monthEndDate; i++)
                    {
                        var salPeriod = await _dbContext.AtSalaryPeriods.Where(s => s.MONTH == i && s.YEAR == yearStartDate).FirstOrDefaultAsync();
                        var org = await _dbContext.HuEmployees.Where(e => e.ID == (long)model["employeeId"]).FirstOrDefaultAsync();
                        var checkLockOrgDateStart = (_dbContext.AtOrgPeriods.Where(p => p.ORG_ID == org!.ORG_ID && p.STATUSCOLEX == 1 && p.PERIOD_ID == salPeriod!.ID)).Count();
                        if (checkLockOrgDateStart != 0)
                        {
                            return new FormatedResponse() { MessageCode = CommonMessageCode.THE_WORK_PERIOD_HAS_CEASED_TO_APPLY, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                    }
                }


                var data = new AtRegisterLeaveDTO();
                data.TypeId = (long?)model["typeId"];
                data.Id = (long)model["id"];
                data.TypeOff = (long?)model["typeOff"];
                data.DateEnd = DateTime.Parse(model["dateEnd"].ToString());
                data.DateStart = DateTime.Parse(model["dateStart"].ToString());
                data.Note = (string?)model["note"];
                data.Reason = (string?)model["reason"];
                data.EmployeeId = (long)model["employeeId"];
                data.IsEachDay = (bool)model["isEachDay"];
                data.CreatedBy = sid;
                data.CreatedDate = DateTime.Now;
                data.UpdatedBy = sid;
                data.UpdatedDate = DateTime.Now;

                var listCheck = _dbContext.AtRegisterLeaves.Where(x => x.EMPLOYEE_ID == (long)model["employeeId"] && x.ID != (long)model["id"]).ToList();
                bool isValid = true;
                foreach (var itemCheck in listCheck)
                {
                    if (data.DateStart <= data.DateEnd && data.DateEnd < itemCheck.DATE_START)
                    {
                        isValid = true;
                    }
                    else if (data.DateStart <= data.DateEnd && data.DateStart > itemCheck.DATE_END)
                    {
                        isValid = true;
                    }
                    else
                    {
                        isValid = false; break;
                    }
                }
                if (isValid == false)
                {
                    return new FormatedResponse()
                    {
                        MessageCode = CommonMessageCode.EMPLOYEE_HAVE_EXIST_TIME_REGISTER,
                        InnerBody = model,
                        StatusCode = EnumStatusCode.StatusCode500
                    };
                }
                var checkValidate = await QueryData.ExecuteList("CHECK_VALIDATE_REGISTER_LEAVE",
                 new
                 {
                     P_EMPLOYEEID = data.EmployeeId,
                     P_LEAVEFROM = data.DateStart.Value.ToString("dd/MM/yyyy"),
                     P_LEAVETO = data.DateEnd.Value.ToString("dd/MM/yyyy"),
                     P_MANUALID = data.TypeId,
                     P_LEAVEID = 0,
                     P_LEAVE_TYPE = data.TypeOff,
                     P_DAYNUM = 0,
                 }, false);
                if (checkValidate != null)
                {
                    var ListValidates = checkValidate.Select(c => (string?)((dynamic)c).MESSAGECODE).FirstOrDefault();
                    if (ListValidates != null && ListValidates != "")
                    {
                        return new FormatedResponse()
                        {
                            MessageCode = ListValidates,
                            InnerBody = model,
                            StatusCode = EnumStatusCode.StatusCode500
                        };
                    }
                }
                var response = await _genericRepository.Update(_uow, data, sid, patchMode);
                var lstDetail = _dbContext.AtRegisterLeaveDetails.Where(x => x.REGISTER_ID == data.Id).ToList();

                _dbContext.RemoveRange(lstDetail);



                try
                {
                    if (data.IsEachDay ?? false)
                    {
                        for (int i = 0; i <= (data.DateEnd - data.DateStart).Value.Days; i++)
                        {

                            if (model["dType" + (i + 1)] == null)
                            {
                                return new FormatedResponse() { MessageCode = CommonMessageCode.REGISTER_LEAVE_DETAIL_NULL, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                            }
                            else
                            {

                                var manualId = _dbContext.AtTimeTypes.Where(x => x.CODE == model["shiftCode" + (i + 1)].ToString()).FirstOrDefault();
                                if (manualId != null)
                                {
                                    var dataDetail = new AT_REGISTER_LEAVE_DETAIL();
                                    dataDetail.REGISTER_ID = (long)data.Id;
                                    dataDetail.LEAVE_DATE = data.DateStart.Value.AddDays(i);
                                    dataDetail.MANUAL_ID = manualId.ID;
                                    dataDetail.NUMBER_DAY = decimal.Parse(model["day" + (i + 1)].ToString());
                                    dataDetail.TYPE_OFF = ((long?)model["dType" + (i + 1)]);
                                    await _dbContext.AtRegisterLeaveDetails.AddAsync(dataDetail);
                                }
                                else
                                {
                                    return new FormatedResponse() { MessageCode = CommonMessageCode.REGISTER_LEAVE_DETAIL_TIMETYPE_NULL, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                                }

                            }

                        }
                    }
                    else
                    {
                        for (int i = 0; i <= (data.DateEnd - data.DateStart).Value.Days; i++)
                        {
                            var dataDetail = new AT_REGISTER_LEAVE_DETAIL();
                            dataDetail.REGISTER_ID = (long)data.Id;
                            dataDetail.LEAVE_DATE = data.DateStart.Value.AddDays(i);
                            dataDetail.NUMBER_DAY = 1;
                            dataDetail.MANUAL_ID = data.TypeId;
                            dataDetail.TYPE_OFF = data.TypeOff;

                            await _dbContext.AtRegisterLeaveDetails.AddAsync(dataDetail);
                        }
                    }

                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }


                await _dbContext.SaveChangesAsync();
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

        }


        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
    }
}

