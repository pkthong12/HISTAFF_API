using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using API.Entities;
using AttendanceDAL.ViewModels;
using API.All.DbContexts;
using Common.Extensions;
using System;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using static System.Runtime.InteropServices.JavaScript.JSType;
using API.Main;
using API;
using Microsoft.Extensions.Options;
using API.DTO;
using CORE.Services.File;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using Azure;
using System.Runtime.Intrinsics.Arm;
using Org.BouncyCastle.Crypto;
using System.Linq.Dynamic.Core;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using CORE.AutoMapper;

namespace ProfileAPI.List
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "002-PROFILE-DISCIPLINE")]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuDisciplineController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly CoreDbContext _coreDbContext;
        private readonly GenericReducer<HU_DISCIPLINE, DisciplineDTO> genericReducer;
        private IGenericRepository<HU_DISCIPLINE, DisciplineDTO> _genericRepository;
        private IGenericRepository<HU_DISCIPLINE_EMP, HuDisciplineEmpDTO> _childGenericRepository;
        private AppSettings _appSettings;
        private readonly IWebHostEnvironment _env;
        private readonly IFileService _fileService;
        public HuDisciplineController(CoreDbContext coreDbContext, IOptions<AppSettings> options, IWebHostEnvironment env, IFileService fileService)
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            _coreDbContext = coreDbContext;
            genericReducer = new();
            _genericRepository = _uow.GenericRepository<HU_DISCIPLINE, DisciplineDTO>();
            _childGenericRepository = _uow.GenericRepository<HU_DISCIPLINE_EMP, HuDisciplineEmpDTO>();
            _appSettings = options.Value;
            _env = env;
            _fileService = fileService;
        }
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<DisciplineDTO> request)
        {

            try
            {
                var entity = _uow.Context.Set<HU_DISCIPLINE>().AsNoTracking().AsQueryable();
                var disciplineEmps = _uow.Context.Set<HU_DISCIPLINE_EMP>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var periods = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking().AsQueryable();
                var jobs = _uow.Context.Set<HU_JOB>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             from de in disciplineEmps.Where(x => x.DISCIPLINE_ID == p.ID)
                             from e in employees.Where(x => x.ID == de.EMPLOYEE_ID).DefaultIfEmpty()
                             from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                             from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                             from j in jobs.Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                             from f in otherLists.Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()
                             from c in otherLists.Where(x => x.ID == p.DISCIPLINE_OBJ).DefaultIfEmpty()
                             from d in otherLists.Where(x => x.ID == p.DISCIPLINE_TYPE).DefaultIfEmpty()
                             from s in employees.Where(x => x.ID == p.SIGN_ID).DefaultIfEmpty()
                             from st in positions.Where(x => x.ID == s.POSITION_ID).DefaultIfEmpty()
                             orderby j.ORDERNUM
                             select new DisciplineDTO
                             {
                                 Id = de.ID,
                                 EmployeeId = p.EMPLOYEE_ID,
                                 EmployeeCode = e.CODE,
                                 EmployeeName = e.Profile.FULL_NAME,
                                 OrgId = o.ID,
                                 OrgName = o.NAME,
                                 PositionName = t.NAME,
                                 DecisionType = p.DECISION_TYPE,
                                 DecisionTypeName = d.NAME,
                                 DecisionNo = p.DECISION_NO,
                                 EffectDate = p.EFFECT_DATE,
                                 ExpireDate = p.EXPIRE_DATE,
                                 StatusName = f.NAME,
                                 EmpStatusId = e.WORK_STATUS_ID,
                                 StatusId = p.STATUS_ID,
                                 IssuedDate = p.ISSUED_DATE,
                                 ViolatedDate = p.VIOLATED_DATE,
                                 BasedOn = p.BASED_ON,
                                 DocumentSignDate = p.DOCUMENT_SIGN_DATE,
                                 SignId = p.SIGN_ID,
                                 SignName = s.Profile.FULL_NAME,
                                 SignPosition = st.NAME,
                                 SignDate = p.SIGN_DATE,
                                 DisciplineObj = p.DISCIPLINE_OBJ,
                                 DisciplineObjName = c.NAME,
                                 DisciplineType = p.DISCIPLINE_TYPE,
                                 DisciplineTypeName = d.NAME,
                                 Reason = p.REASON,
                                 ExtendSalTime = p.EXTEND_SAL_TIME,
                                 Attachment = p.ATTACHMENT,
                                 Note = p.NOTE,
                                 CreatedBy = p.CREATED_BY,
                                 UpdatedBy = p.UPDATED_BY,
                                 CreatedDate = p.CREATED_DATE,
                                 UpdatedDate = p.UPDATED_DATE,
                                 JobOrderNum = (int)(j.ORDERNUM ?? 99)
                             };
                request.Sort = new List<SortItem>();
                request.Sort.Add(new SortItem() { Field = "CreatedDate", SortDirection = EnumSortDirection.DESC });
                var response = await genericReducer.SinglePhaseReduce(joined, request);
                if (response.ErrorType != EnumErrorType.NONE)
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = response.ErrorType,
                        MessageCode = response.MessageCode ?? CommonMessageCode.NOT_ALL_CASES_CATCHED,
                        StatusCode = EnumStatusCode.StatusCode400,
                    });
                }
                else
                {
                    return Ok(new FormatedResponse()
                    {
                        InnerBody = response
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            try
            {
                if (model.Ids != null)
                {
                    bool checkData = false;
                    var getOtherList = _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD").FirstOrDefault();
                    model.Ids.ForEach(item =>
                    {
                        var getDataBeforeDelete = _uow.Context.Set<HU_DISCIPLINE_EMP>().Where(x => x.ID == item).FirstOrDefault();
                        var getParentOfRecord = _uow.Context.Set<HU_DISCIPLINE>().Where(x => x.ID == getDataBeforeDelete!.DISCIPLINE_ID).FirstOrDefault();
                        if(getParentOfRecord!.STATUS_ID != getOtherList!.ID)
                        {
                            checkData = true;
                            return;
                        }
                    });
                    
                    if(checkData == true) 
                    {
                        return Ok(new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            StatusCode = EnumStatusCode.StatusCode400,
                            MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_IS_ACTIVE,
                        });
                    }
                    var response = await _childGenericRepository.DeleteIds(_uow,model.Ids);
                    return Ok(response);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DisciplineDTO model)
        {
            try
            {
                if (model.Id != null)
                {
                    var response = await _genericRepository.Delete(_uow, (long)model.Id);
                    return Ok(response);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetDisciplineByEmployee(long id)
        {
            try
            {
                var entity = _uow.Context.Set<HU_DISCIPLINE>().AsNoTracking().AsQueryable();
                var disciplineEmps = _uow.Context.Set<HU_DISCIPLINE_EMP>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var periods = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    from de in disciplineEmps.Where(x => x.DISCIPLINE_ID == p.ID)
                                    from e in employees.Where(x => x.ID == de.EMPLOYEE_ID).DefaultIfEmpty()
                                    from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                    from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                    from f in otherLists.Where(x => x.ID == p.STATUS_ID ).DefaultIfEmpty()
                                    from c in otherLists.Where(x => x.ID == p.DISCIPLINE_OBJ).DefaultIfEmpty()
                                    from d in otherLists.Where(x => x.ID == p.DISCIPLINE_TYPE).DefaultIfEmpty()
                                    from dt in otherLists.Where(x => x.ID == p.DECISION_TYPE).DefaultIfEmpty()
                                    from s in employees.Where(x => x.ID == p.SIGN_ID).DefaultIfEmpty()
                                    from st in positions.Where(x => x.ID == s.POSITION_ID).DefaultIfEmpty()
                                    where de.EMPLOYEE_ID == id && f.CODE == "DD"
                                    orderby p.VIOLATED_DATE descending
                                    select new 
                                    {
                                        Id = p.ID,
                                        EmployeeId = id,
                                        DecisionType = p.DECISION_TYPE,
                                        DecisionTypeName = dt.NAME,
                                        DecisionNo = p.DECISION_NO,
                                        EffectDate = p.EFFECT_DATE,
                                        ExpireDate = p.EXPIRE_DATE,
                                        StatusName = f.NAME,
                                        StatusId = p.STATUS_ID,
                                        EmpStatusId = e.WORK_STATUS_ID,
                                        IssuedDate = p.ISSUED_DATE,
                                        ViolatedDate = p.VIOLATED_DATE,
                                        BasedOn = p.BASED_ON,
                                        DocumentSignDate = p.DOCUMENT_SIGN_DATE,
                                        SignId = p.SIGN_ID,
                                        SignName = s.Profile.FULL_NAME,
                                        SignPosition = st.NAME,
                                        SignDate = p.SIGN_DATE,
                                        DisciplineObj = p.DISCIPLINE_OBJ,
                                        DisciplineObjName = c.NAME,
                                        DisciplineType = p.DISCIPLINE_TYPE,
                                        DisciplineTypeName = d.NAME,
                                        Reason = p.REASON,
                                        ExtendSalTime = p.EXTEND_SAL_TIME,
                                        Attachment = p.ATTACHMENT,
                                        Note = p.NOTE,
                                        CreatedBy = p.CREATED_BY,
                                        UpdatedBy = p.UPDATED_BY,
                                        CreatedDate = p.CREATED_DATE,
                                        UpdatedDate = p.UPDATED_DATE
                                    }).ToListAsync(); ;
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var entity = _uow.Context.Set<HU_DISCIPLINE>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var disciplineEmps = _uow.Context.Set<HU_DISCIPLINE_EMP>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var periods = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking().AsQueryable();
                var entityId = (from p in disciplineEmps where p.ID == id select p.DISCIPLINE_ID).FirstOrDefault();
                var joined = await (from p in entity
                                    from f in otherLists.Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()
                                    from c in otherLists.Where(x => x.ID == p.DISCIPLINE_OBJ).DefaultIfEmpty()
                                    from d in otherLists.Where(x => x.ID == p.DISCIPLINE_TYPE).DefaultIfEmpty()
                                    from s in employees.Where(x => x.ID == p.SIGN_ID).DefaultIfEmpty()
                                    from st in positions.Where(x => x.ID == s.POSITION_ID).DefaultIfEmpty()
                                    where p.ID == entityId
                                    select new
                                        {
                                            Id = p.ID,
                                            DecisionType = p.DECISION_TYPE,
                                            DecisionTypeName = d.NAME,
                                            DecisionNo = p.DECISION_NO,
                                            EffectDate = p.EFFECT_DATE,
                                            ExpireDate = p.EXPIRE_DATE,
                                            StatusName = f.NAME,
                                            StatusId = p.STATUS_ID,
                                            IssuedDate = p.ISSUED_DATE,
                                            ViolatedDate = p.VIOLATED_DATE,
                                            BasedOn = p.BASED_ON,
                                            DocumentSignDate = p.DOCUMENT_SIGN_DATE,
                                            SignId = p.SIGN_ID,
                                            SignDate = p.SIGN_DATE,
                                            SignerName = s.Profile.FULL_NAME,
                                            DisciplineObj = p.DISCIPLINE_OBJ,
                                            DisciplineObjName = c.NAME,
                                            DisciplineType = p.DISCIPLINE_TYPE,
                                            DisciplineTypeName = d.NAME,
                                            Reason = p.REASON,
                                            ExtendSalTime = p.EXTEND_SAL_TIME,
                                            Attachment = p.ATTACHMENT,
                                            Note = p.NOTE,
                                            CreatedBy = p.CREATED_BY,
                                            UpdatedBy = p.UPDATED_BY,
                                            CreatedDate = p.CREATED_DATE,
                                            UpdatedDate = p.UPDATED_DATE,
                                            EmployeeList = new List<EmployeeDTO>(),
                                        EmployeeIds = (from p in disciplineEmps.Where(x => x.DISCIPLINE_ID == entityId) select p.EMPLOYEE_ID).ToList()
                                    }).FirstOrDefaultAsync();

                var empList = (from e in entity
                               from d in disciplineEmps.Where(x => x.DISCIPLINE_ID == e.ID)
                               from p in employees.Where(x => x.ID == d.EMPLOYEE_ID)
                               from o in organizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                               from t in positions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                               from g in otherLists.Where(x => x.ID == p.Profile.GENDER_ID).DefaultIfEmpty()
                               from w in otherLists.Where(x => x.ID == p.WORK_STATUS_ID).DefaultIfEmpty()
                               where d.DISCIPLINE_ID == entityId
                               select new EmployeeDTO()
                                     {
                                         Id = p.ID,
                                         Fullname = p.Profile.FULL_NAME,
                                         Code = p.CODE,
                                         OrgName = o.NAME,
                                         PositionName = t.NAME,
                                         WorkStatusName = w.NAME,
                                         JoinDate = p.JOIN_DATE,
                                         BirthDate = p.Profile.BIRTH_DATE,
                                         BirthPlace = p.Profile.BIRTH_PLACE,
                                         GenderName = g.NAME,
                                         MobilePhone = p.Profile.MOBILE_PHONE,
                                         Email = p.Profile.EMAIL,
                                         WorkEmail = p.Profile.WORK_EMAIL,
                                         IdNo = p.Profile.ID_NO,
                                         IdDate = p.Profile.ID_DATE
                                     }).ToList();

                joined?.EmployeeList.AddRange(empList);

                var response = new FormatedResponse() { InnerBody = joined };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(DisciplineDTO model)
        {
            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();
            try
            {
                //if (model.EffectDate > model.ExpireDate)//check ngay hieu luc <= ngay het hieu luc
                //{
                //    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_FROM_DATE_MUST_LESS_THAN_EQUAL_TO_DATE" });
                //}else if(model.ViolatedDate > model.IssuedDate)//check ngay vi pham < ngay ban hanh
                //{
                //    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_VIOLATED_DATE_MUST_LESS_THAN_EQUAL_ISSUEDDATE_DATE" });
                //}else if(model.IssuedDate > model.SignDate)//check ngay ban hanh <= ngay ky
                //{
                //    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_ISSUEDDATE_DATE_MUST_LESS_THAN_EQUAL_SIGNDATE_DATE" });
                //}else if(model.EffectDate < model.SignDate)//check ngay ky <= ngay hieu luc
                //{
                //    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_SIGNDATE_DATE_MUST_LESS_THAN_EQUAL_EFFECTDATE_DATE" });
                //}
                var sid = Request.Sid(_appSettings);
                var check = true;
                var violatedDate = model.ViolatedDate;//ngay vi pham

                //check ngay ky hop dong <= Ngay vi pham
                var employeesCv = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
                var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                //if (model.EmployeeIds != null)
                //{
                //    foreach (var item in model.EmployeeIds)
                //    {
                //        //emp = item;
                //        var huEmployee = await _coreDbContext.Employees.AsNoTracking().Where(x => x.ID == item).ToListAsync();
                //        foreach(var item1 in huEmployee)
                //        {
                //            if(item1.JOIN_DATE_STATE > violatedDate)//check ngay vao cty <= ngay vi pham
                //            {
                //                check = false;
                //            }
                //        }
                //    }
                //    if (check == false)
                //    {
                //        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "ERROR_DATA_EMPLOYEE_JOIN_DATE_LES_THAN_VIOLATED_DATE" });
                //    }
                //}
                // First of all we need to upload all the attachments
                if (model.AttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = model.AttachmentBuffer.ClientFileName,
                        ClientFileType = model.AttachmentBuffer.ClientFileType,
                        ClientFileData = model.AttachmentBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(DisciplineDTO).GetProperty("Attachment");

                    if (property != null)
                    {
                        property?.SetValue(model, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": Attachment" });
                    }

                }
                var entity = _uow.Context.Set<HU_DISCIPLINE>().AsNoTracking().AsQueryable();
                long id = 1;
                if ((from p in entity select p).Any()) id = entity.Max(p => p.ID) + 1;

                HU_DISCIPLINE objData = new()
                {
                    DOCUMENT_SIGN_DATE = model.DocumentSignDate,
                    SIGN_ID = model.SignId,
                    SIGN_DATE = model.SignDate,
                    STATUS_ID = model.StatusId,
                    ISSUED_DATE = model.IssuedDate,
                    VIOLATED_DATE = model.ViolatedDate,
                    BASED_ON = model.BasedOn,
                    DECISION_TYPE = model.DecisionType,
                    DECISION_NO = model.DecisionNo,
                    EFFECT_DATE = model.EffectDate,
                    EXPIRE_DATE = model.ExpireDate,
                    DISCIPLINE_OBJ = model.DisciplineObj,
                    DISCIPLINE_TYPE = model.DisciplineType,
                    REASON = model.Reason,
                    EXTEND_SAL_TIME = model.ExtendSalTime,
                    NOTE = model.Note,
                    ATTACHMENT = model.Attachment,
                    CREATED_BY = sid,
                    CREATED_DATE = DateTime.Now,
                    UPDATED_BY = sid,
                    UPDATED_DATE = DateTime.Now,
                    ID = id
                };
                _coreDbContext.Disciplines.Add(objData);
                if (model.EmployeeIds != null)
                {
                    foreach (long i in model.EmployeeIds)
                    {

                        HU_DISCIPLINE_EMP objEmpData = new()
                        {
                            EMPLOYEE_ID = i,
                            DISCIPLINE_ID = objData.ID,
                            CREATED_BY = sid,
                            CREATED_DATE = DateTime.Now,
                            UPDATED_BY = sid,
                            UPDATED_DATE = DateTime.Now
                        };
                        _coreDbContext.DisciplineEmps.AddRange(objEmpData);
                    }
                }
                await _coreDbContext.SaveChangesAsync();
                _uow.Commit();
                return Ok(new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.CREATE_SUCCESS,
                    InnerBody = objData,
                    StatusCode = EnumStatusCode.StatusCode200
                });

            }
            catch (Exception ex)
            {
                // Try to delete uploaded files
                uploadFiles.ForEach(async f =>
                {
                    await _fileService.DeleteAttachment(f.SavedAs);
                });

                _uow.Rollback();
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(DisciplineDTO model)
        {
            //if (model.StatusId != null && model.StatusId == OtherConfig.STATUS_APPROVE)
            //    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_CANNOT_EDIT_APPROVED_RECORD" });
            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();
            try
            {
                if (model.EffectDate > model.ExpireDate)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_FROM_DATE_MUST_LESS_THAN_EQUAL_TO_DATE" });
                }
                //else if (model.ViolatedDate > model.IssuedDate)//check ngay vi pham < ngay ban hanh
                //{
                //    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_VIOLATED_DATE_MUST_LESS_THAN_EQUAL_ISSUEDDATE_DATE" });
                //}
                //else if (model.IssuedDate > model.SignDate)//check ngay ban hanh <= ngay ky
                //{
                //    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_ISSUEDDATE_DATE_MUST_LESS_THAN_EQUAL_SIGNDATE_DATE" });
                //}
                //else if (model.EffectDate < model.SignDate)//check ngay ky <= ngay hieu luc
                //{
                //    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_SIGNDATE_DATE_MUST_LESS_THAN_EQUAL_EFFECTDATE_DATE" });
                //}
                var sid = Request.Sid(_appSettings);
                var check = true;
                var violatedDate = model.ViolatedDate;//ngay vi pham

                //check ngay ky hop dong <= Ngay vi pham
                //var employeesCv = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
                //var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                //if (model.EmployeeIds != null)
                //{
                //    foreach (var item in model.EmployeeIds)
                //    {
                //        //emp = item;
                //        var huEmployee = await _coreDbContext.Employees.AsNoTracking().Where(x => x.ID == item).ToListAsync();
                //        foreach (var item1 in huEmployee)
                //        {
                //            if (item1.JOIN_DATE_STATE > violatedDate)//check ngay vao cty <= ngay vi pham
                //            {
                //                check = false;
                //            }
                //        }
                //    }
                //    if (check == false)
                //    {
                //        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "ERROR_DATA_EMPLOYEE_JOIN_DATE_LES_THAN_VIOLATED_DATE" });
                //    }
                //}
                // First of all we need to upload all the attachments
                if (model.AttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = model.AttachmentBuffer.ClientFileName,
                        ClientFileType = model.AttachmentBuffer.ClientFileType,
                        ClientFileData = model.AttachmentBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(DisciplineDTO).GetProperty("Attachment");

                    if (property != null)
                    {
                        property?.SetValue(model, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": Attachment" });
                    }

                }

                var response = await _genericRepository.Update(_uow, model, Request.Sid(_appSettings));

                if (model.EmployeeIds != null)
                {
                    _coreDbContext.DisciplineEmps.RemoveRange(_coreDbContext.DisciplineEmps.Where(x => x.DISCIPLINE_ID == model.Id));
                    foreach (long i in model.EmployeeIds)
                    {
                        HU_DISCIPLINE_EMP objEmpData = new()
                        {
                            EMPLOYEE_ID = i,
                            DISCIPLINE_ID = model.Id,
                            CREATED_BY = sid,
                            CREATED_DATE = DateTime.Now,
                            UPDATED_BY = sid,
                            UPDATED_DATE = DateTime.Now
                        };
                        _coreDbContext.DisciplineEmps.AddAsync(objEmpData);
                    }
                }
                await _coreDbContext.SaveChangesAsync();
                _uow.Commit();
                return Ok(response);

            }
            catch (Exception ex)
            {
                // Try to delete uploaded files
                uploadFiles.ForEach(async f =>
                {
                    await _fileService.DeleteAttachment(f.SavedAs);
                });

                _uow.Rollback();
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatusApprove(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var entityListDiscipEmp = _uow.Context.Set<HU_DISCIPLINE_EMP>();
            var entityListDiscip = _uow.Context.Set<HU_DISCIPLINE>();
            var searchEmp = await entityListDiscipEmp.Where(p => model.Ids.Contains(p.ID)).ToListAsync();
            var response = new List<HU_DISCIPLINE>();
            searchEmp.ForEach(x =>
            {
                var searchDiscip = entityListDiscip.Where(p => p.ID == x.DISCIPLINE_ID).First();
                searchDiscip.STATUS_ID = model.ValueToBind ? OtherConfig.STATUS_APPROVE : OtherConfig.STATUS_WAITING;
            });
            try
            {
                _uow.Context.SaveChanges();
                return Ok(new FormatedResponse()
                {
                    MessageCode = model.ValueToBind == true ? CommonMessageCode.APPROVE_SUCCESS : CommonMessageCode.PENDING_APPROVE_SUCCESS,
                    InnerBody = response,
                    StatusCode = EnumStatusCode.StatusCode200
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse()
                {
                    MessageCode = "FAIL",
                    InnerBody = response,
                    StatusCode = EnumStatusCode.StatusCode500
                });
            }

        }
    }
}
