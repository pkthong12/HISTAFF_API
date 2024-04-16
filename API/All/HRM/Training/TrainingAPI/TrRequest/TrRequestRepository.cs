using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using CORE.Services.File;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using ProfileDAL.ViewModels;
using PayrollDAL.Models;
using DocumentFormat.OpenXml.Spreadsheet;

namespace API.Controllers.TrRequest
{
    public class TrRequestRepository : ITrRequestRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_REQUEST, TrRequestDTO> _genericRepository;
        private readonly GenericReducer<TR_REQUEST, TrRequestDTO> _genericReducer;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;

        public TrRequestRepository(
            FullDbContext context, 
            GenericUnitOfWork uow,
            IWebHostEnvironment env,
            IOptions<AppSettings> options,
            IFileService fileService)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_REQUEST, TrRequestDTO>();
            _genericReducer = new();
            _env = env;
            _appSettings = options.Value;
            _fileService = fileService;
        }

        public async Task<GenericPhaseTwoListResponse<TrRequestDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrRequestDTO> request)
        {
            var joined = from p in _dbContext.TrRequests.AsNoTracking()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.STATUS_ID).DefaultIfEmpty() 
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == p.REQUEST_SENDER_ID).DefaultIfEmpty()
                         from pos in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                         from c in _dbContext.TrCourses.AsNoTracking().Where(c => c.ID == p.TR_COURSE_ID).DefaultIfEmpty()
                         from tf in _dbContext.SysOtherLists.AsNoTracking().Where(tf => tf.ID == p.TRAIN_FORM_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == p.ORG_ID).DefaultIfEmpty()
                         select new TrRequestDTO
                         {
                             Id = p.ID,
                             StatusId = p.STATUS_ID,
                             Status = s.NAME,
                             RequestCode = p.REQUEST_CODE,
                             Year = p.YEAR,
                             RequestSenderId = p.REQUEST_SENDER_ID,
                             SenderName = e.Profile!.FULL_NAME,
                             SenderPosition = pos.NAME,
                             SenderEmail = e.Profile!.WORK_EMAIL,
                             SenderPhoneNumber = e.Profile!.MOBILE_PHONE,
                             RequestDate = p.REQUEST_DATE,
                             TrCourseId = p.TR_COURSE_ID,
                             TrCourseName = c.COURSE_NAME,
                             OtherCourse = p.OTHER_COURSE,
                             TrainFormId = p.TRAIN_FORM_ID,
                             TrainFormName = tf.NAME,
                             TargetTrain = p.TARGET_TRAIN,
                             Content = p.CONTENT,
                             OrgId = p.ORG_ID,
                             OrgName = o.NAME,
                             PropertiesNeedId = p.PROPERTIES_NEED_ID,
                             ExpectedDate = p.EXPECTED_DATE,
                             ExpectDateTo = p.EXPECT_DATE_TO,
                             ExpectedCost = p.EXPECTED_COST,
                             TrainerNumber = p.TRAINER_NUMBER,
                             TrCommit = p.TR_COMMIT,
                             Certificate = p.CERTIFICATE,
                             TrPlace = p.TR_PLACE,  
                             Remark = p.REMARK,
                             RejectReason = p.REJECT_REASON,
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
            List<long> lstcheckCenterItems = new List<long>();
            List<long> lstcheckTeacherItems = new List<long>();
            var joined = await (from p in _dbContext.TrRequests.AsNoTracking().Where(p => p.ID == id)
                                from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.STATUS_ID).DefaultIfEmpty()
                                from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == p.REQUEST_SENDER_ID).DefaultIfEmpty()
                                from pos in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                                from c in _dbContext.TrCourses.AsNoTracking().Where(c => c.ID == p.TR_COURSE_ID).DefaultIfEmpty()
                                from tf in _dbContext.SysOtherLists.AsNoTracking().Where(tf => tf.ID == p.TRAIN_FORM_ID).DefaultIfEmpty()
                                from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == p.ORG_ID).DefaultIfEmpty()
                                from f in _dbContext.SysOtherLists.AsNoTracking().Where(f => f.ID == c.TR_TRAIN_FIELD).DefaultIfEmpty()
                                select new
                                {
                                    Id = p.ID,
                                    StatusId = p.STATUS_ID,
                                    Status = s.NAME,
                                    RequestCode = p.REQUEST_CODE,
                                    Year = p.YEAR,
                                    RequestSenderId = p.REQUEST_SENDER_ID,
                                    SenderName = e.Profile!.FULL_NAME,
                                    SenderPosition = pos.NAME,
                                    SenderEmail = e.Profile!.WORK_EMAIL,
                                    SenderPhoneNumber = e.Profile!.MOBILE_PHONE,
                                    RequestDate = p.REQUEST_DATE,
                                    TrCourseId = p.TR_COURSE_ID,
                                    TrCourseName = c.COURSE_NAME,
                                    TrTrainFeild = f.NAME,
                                    OtherCourse = p.OTHER_COURSE,
                                    TrainFormId = p.TRAIN_FORM_ID,
                                    TrainFormName = tf.NAME,
                                    TargetTrain = p.TARGET_TRAIN,
                                    Content = p.CONTENT,
                                    OrgId = p.ORG_ID,
                                    OrgName = o.NAME,
                                    PropertiesNeedId = p.PROPERTIES_NEED_ID,
                                    ExpectedDate = p.EXPECTED_DATE,
                                    ExpectDateTo = p.EXPECT_DATE_TO,
                                    ExpectedCost = p.EXPECTED_COST,
                                    TrainerNumber = p.TRAINER_NUMBER,
                                    TrCommit = p.TR_COMMIT,
                                    Certificate = p.CERTIFICATE,
                                    TrCurrencyId = p.TR_CURRENCY_ID,
                                    TrPlace = p.TR_PLACE,
                                    Remark = p.REMARK,
                                    RejectReason = p.REJECT_REASON,
                                    AttachFile = p.ATTACH_FILE,
                                    EmployeeList = new List<EmployeeDTO>(),
                                    EmployeeIds = (from te in _dbContext.TrRequestEmployees.Where(x => x.TR_REQUEST_ID == id) select te.EMPLOYEE_ID).ToList(),
                                    CentersId = p.CENTERS_ID,
                                    TeachersIs = p.TEACHERS_ID,
                                    ListCentersId = new List<long>(),
                                    ListTeachersId = new List<long>(),
                                }).FirstOrDefaultAsync();
            if (joined != null)
            {
                var employeeList = (from c in _dbContext.TrRequests.AsNoTracking()
                                    from ce in _dbContext.TrRequestEmployees.Where(x => x.TR_REQUEST_ID == c.ID)
                                    from e in _dbContext.HuEmployees.AsNoTracking().Where(x => x.ID == ce.EMPLOYEE_ID)
                                    from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                    from po in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                    from g in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == e.Profile!.GENDER_ID).DefaultIfEmpty()
                                    where ce.TR_REQUEST_ID == id
                                    select new EmployeeDTO()
                                    {
                                        Id = e.ID,
                                        Fullname = e.Profile!.FULL_NAME,
                                        Avatar = e.Profile!.AVATAR,
                                        Code = e.CODE,
                                        PositionName = po.NAME,
                                        OrgName = o.NAME,
                                        JoinDate = e.JOIN_DATE,
                                        BirthDate = e.Profile!.BIRTH_DATE,
                                        BirthPlace = e.Profile!.BIRTH_PLACE,
                                        GenderName = g.NAME,
                                        MobilePhone = e.Profile!.MOBILE_PHONE,
                                        Email = e.Profile!.EMAIL,
                                        IdNo = e.Profile!.ID_NO,
                                        IdDate = e.Profile!.ID_DATE
                                    }).ToList();
                joined?.EmployeeList.AddRange(employeeList);

                if(joined?.CentersId != "")
                {
                    string[] centerList = joined?.CentersId.Split(",")!;
                    if(centerList.Count() != 0)
                    {
                        foreach (var item in centerList)
                        {
                            lstcheckCenterItems.Add(Convert.ToInt64(item));
                        }
                    }
                    joined?.ListCentersId.AddRange(lstcheckCenterItems);
                }

                if (joined?.TeachersIs != "")
                {
                    string[] teacherList = joined?.TeachersIs.Split(",")!;
                    if (teacherList.Count() != 0)
                    {
                        foreach (var item in teacherList)
                        {
                            lstcheckTeacherItems.Add(Convert.ToInt64(item));
                        }
                    }
                    joined?.ListTeachersId.AddRange(lstcheckTeacherItems);
                }

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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrRequestDTO dto, string sid)
        {
            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();
            try
            {
                dto.CentersId = (dto.ListCentersId != null ? string.Join(",", dto.ListCentersId.ToArray()) : "");
                dto.TeachersId = (dto.ListTeachersId != null ? string.Join(",", dto.ListTeachersId.ToArray()) : "");
                if (dto.AttachedFileBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = dto.AttachedFileBuffer.ClientFileName,
                        ClientFileType = dto.AttachedFileBuffer.ClientFileType,
                        ClientFileData = dto.AttachedFileBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(TrRequestDTO).GetProperty("AttachFile");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": AttachFile" };
                    }
                }

                var response = await _genericRepository.Create(_uow, dto, sid);

                if (dto.EmployeeIds != null && dto.EmployeeIds.Count > 0)
                {
                    var d = _dbContext.TrRequestEmployees.Where(x => x.TR_REQUEST_ID == dto.Id).ToList();
                    if (d != null)
                    {
                        _dbContext.TrRequestEmployees.RemoveRange(d);
                    }
                    foreach (var item in dto.EmployeeIds)
                    {

                        var trRequestEmp = new TR_REQUEST_EMPLOYEE();
                        trRequestEmp.EMPLOYEE_ID = item;
                        var requestId = response.InnerBody!.GetType().GetProperty("ID")!.GetValue(response.InnerBody, null);
                        trRequestEmp.TR_REQUEST_ID = (long)requestId!;

                        await _dbContext.TrRequestEmployees.AddAsync(trRequestEmp);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                _uow.Commit();
                return response;
            }
            catch (Exception ex)
            {
                uploadFiles.ForEach(async f =>
                {
                    await _fileService.DeleteAttachment(f.SavedAs);
                });

                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
            /*var response = await _genericRepository.Create(_uow, dto, sid);
            return response;*/
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrRequestDTO> dtos, string sid)
        {
            var add = new List<TrRequestDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrRequestDTO dto, string sid, bool patchMode = true)
        {
            List<UploadFileResponse> uploadFiles = new();
            try
            {
                if (dto.AttachedFileBuffer != null)
                {
                    dto.CentersId = (dto.ListCentersId != null ? string.Join(",", dto.ListCentersId.ToArray()) : "");
                    dto.TeachersId = (dto.ListTeachersId != null ? string.Join(",", dto.ListTeachersId.ToArray()) : "");
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = dto.AttachedFileBuffer.ClientFileName,
                        ClientFileType = dto.AttachedFileBuffer.ClientFileType,
                        ClientFileData = dto.AttachedFileBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(TrRequestDTO).GetProperty("AttachFile");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": AttachFile" };
                    }
                }
                var response = await _genericRepository.Update(_uow, dto, sid, patchMode);

                if (dto.EmployeeIds != null && dto.EmployeeIds.Count > 0)
                {
                    var d = _dbContext.TrRequestEmployees.Where(x => x.TR_REQUEST_ID == dto.Id).ToList();
                    if (d != null)
                    {
                        _dbContext.TrRequestEmployees.RemoveRange(d);
                    }
                    foreach (var item in dto.EmployeeIds)
                    {

                        var trRequestEmp = new TR_REQUEST_EMPLOYEE();
                        trRequestEmp.EMPLOYEE_ID = item;
                        var requestId = response.InnerBody!.GetType().GetProperty("ID")!.GetValue(response.InnerBody, null);
                        trRequestEmp.TR_REQUEST_ID = (long)requestId!;

                        await _dbContext.TrRequestEmployees.AddAsync(trRequestEmp);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                _uow.Commit();
                return response;
            }
            catch (Exception ex)
            {
                uploadFiles.ForEach(async f =>
                {
                    await _fileService.DeleteAttachment(f.SavedAs);
                });

                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
            /*var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;*/
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrRequestDTO> dtos, string sid, bool patchMode = true)
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

        public Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }
    }
}

