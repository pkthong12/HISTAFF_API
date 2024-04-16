﻿
using API;
using API.All.DbContexts;
using API.All.SYSTEM.Common;
using API.All.SYSTEM.CoreAPI.Word;
using API.Controllers.HuConcurrently;
using API.DTO;
using API.Entities;
using API.Main;
using Common.DataAccess;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.VariantTypes;
using InsuranceDAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;
using System;
using System.Data;

namespace ProfileAPI.List
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "002-PROFILE-WORKING")]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuWorkingController : BaseController1
    {
        private readonly GenericUnitOfWork _uow;
        private IGenericRepository<HU_WORKING, WorkingInputDTO> _genericRepository;
        private IInsArisingRepository _insArisingRepository;
        private IHuConcurrentlyRepository _positonConcurrently;
        private readonly GenericReducer<HU_WORKING, WorkingInputDTO> genericReducer;
        private readonly IFileService _fileService;
        private AppSettings _appSettings;
        private readonly IWebHostEnvironment _env;
        private CoreDbContext _coreDbContext;
        private readonly IWordRespsitory _wordRespsitory;
        private FullDbContext _fullDbContext;

        public HuWorkingController(IProfileUnitOfWork unitOfWork, IOptions<AppSettings> options, IWebHostEnvironment env, IFileService fileService, CoreDbContext coreDbContext, FullDbContext dbContext) : base(unitOfWork)
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            _genericRepository = _uow.GenericRepository<HU_WORKING, WorkingInputDTO>();
            _insArisingRepository = new InsArisingRepository(coreDbContext, _uow);
            _positonConcurrently = new HuConcurrentlyRepository(dbContext, _uow);
            genericReducer = new();
            _appSettings = options.Value;
            _env = env;
            _fileService = fileService;
            _coreDbContext = coreDbContext;
            _wordRespsitory = new WordRepository();
            _fullDbContext = dbContext;
        }
        //DECISION
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuWorkingDTO> request)
        {
            try
            {
                /* HERE WE CAN EITHER CHOOSE ONE OF THE 2 APPROACHS: TwoPhaseQueryList or SinglePhaseQueryList */
                /* WE WILL KNOW WHAT APPROACH IS FASTER IN THE FUTURE */

                var response = await _unitOfWork.WorkingRepository.TwoPhaseQueryList(request);

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
        [HttpGet]
        public async Task<ActionResult> GetAll(WorkingDTO param)
        {
            var r = await _unitOfWork.WorkingRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetWorking(WorkingDTO param)
        {
            var r = await _unitOfWork.WorkingRepository.GetWorking(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetById(long Id)
        {
            try
            {
                var r = await _unitOfWork.WorkingRepository.GetById(Id);
                return Ok(new FormatedResponse() { InnerBody = r.Data });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetLastWorking(long? empId, DateTime? date)
        {
            var r = await _unitOfWork.WorkingRepository.GetLastWorking(empId, date);
            return Ok(new FormatedResponse() { InnerBody = r.Data });
        }
        [HttpGet]
        public async Task<ActionResult> GetWorkingOld(long? empId, long? id)
        {
            var r = await _unitOfWork.WorkingRepository.GetWorkingOld(empId, id);
            return Ok(new FormatedResponse() { InnerBody = r.Data });
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] WorkingInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var sid = Request.Sid(_appSettings);
            if (param.AttachmentBuffer != null)
            {
                List<UploadFileResponse> uploadFiles = new();
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                {
                    ClientFileName = param.AttachmentBuffer.ClientFileName,
                    ClientFileType = param.AttachmentBuffer.ClientFileType,
                    ClientFileData = param.AttachmentBuffer.ClientFileData
                }, location, sid);

                // Assign saved paths
                var property = typeof(WorkingInputDTO).GetProperty("Attachment");

                if (property != null)
                {
                    property?.SetValue(param, uploadFileResponse.SavedAs);
                    uploadFiles.Add(uploadFileResponse);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": Attachment" });
                }

            }


            var approved = await _fullDbContext.SysOtherLists.Where(x => x.CODE == "DD").FirstOrDefaultAsync();
            var r = await _unitOfWork.WorkingRepository.CreateAsync(param);
            if (r.StatusCode == "200" && r.InnerBody != null)
            {
                var rsObjData = (WorkingInputDTO)r.InnerBody;
                if (rsObjData.IsNotResign.HasValue && rsObjData.IsNotResign.Value == true)
                {
                    if (param.IsResponsible.HasValue && param.IsResponsible == true)
                    {
                        var arisingObj = new InsArisingDTO()
                        {
                            PkeyRef = rsObjData.Id,
                            EmployeeId = param.EmployeeId,
                            EffectDate = param.EffectDate,
                            TableRef = "WORKING"
                        };
                        var s = await _insArisingRepository.InsertArising(_uow, arisingObj, sid);
                    }
                    if ((!param.IsResponsible.HasValue || param.IsResponsible == false))
                    {
                        var sys = _coreDbContext.SysOtherLists.Where(x => x.CODE == "DD").FirstOrDefault();

                        var waitApprove = _coreDbContext.SysOtherLists.Where(x => x.CODE == "CD").FirstOrDefault();
                        var rejectApprove = _coreDbContext.SysOtherLists.Where(x => x.CODE == "TCPD").FirstOrDefault();

                        if (param.StatusId.HasValue && param.StatusId == sys!.ID)
                        {
                            var join = (from e2 in _fullDbContext.HuEmployees.Where(x => x.ID == param.SignId)
                                        from q in _fullDbContext.HuEmployeeCvs.Where(x => x.ID == e2.PROFILE_ID).DefaultIfEmpty()
                                        from p in _fullDbContext.HuPositions.Where(x => x.ID == e2.POSITION_ID).DefaultIfEmpty()
                                            // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                        select new
                                        {
                                            SigningPositionName = p.NAME,
                                        }).FirstOrDefault();
                            var positionConcurrently = new HU_CONCURRENTLY()
                            {
                                EFFECTIVE_DATE = param.EffectDate,
                                EXPIRATION_DATE = param.ExpireDate,
                                EMPLOYEE_ID = param.EmployeeId,
                                DECISION_NUMBER = param.DecisionNo,
                                ORG_ID = param.OrgId,
                                POSITION_ID = param.PositionId,
                                STATUS_ID = param.StatusId,
                                SIGNING_DATE = param.SignDate,
                                SIGNING_EMPLOYEE_ID = param.SignId,
                                NOTE = param.Note,
                                IS_FROM_WORKING = true,
                                SIGNING_POSITION_NAME = (join != null) ? join.SigningPositionName : "",
                                WORKING_ID = param.Id
                            };
                            var s = await _fullDbContext.HuConcurrentlys.AddAsync(positionConcurrently);
                            await _fullDbContext.SaveChangesAsync();
                            return Ok(new FormatedResponse() { InnerBody = positionConcurrently, MessageCode = CommonMessageCode.CREATE_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 });
                        }
                        else if (param.StatusId.HasValue && (param.StatusId == waitApprove!.ID || param.StatusId == rejectApprove!.ID))
                        {
                            return Ok(new FormatedResponse()
                            {
                                InnerBody = true,
                                MessageCode = CommonMessageCode.CREATE_SUCCESS,
                                StatusCode = EnumStatusCode.StatusCode200
                            });
                        }
                        else
                        {
                            return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 });
                        }


                    }
                }

                return Ok(new FormatedResponse() { InnerBody = r.Data, MessageCode = CommonMessageCode.CREATE_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 });
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
            }

        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] WorkingInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var sid = Request.Sid(_appSettings);
            if (param.AttachmentBuffer != null)
            {
                List<UploadFileResponse> uploadFiles = new();
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                {
                    ClientFileName = param.AttachmentBuffer.ClientFileName,
                    ClientFileType = param.AttachmentBuffer.ClientFileType,
                    ClientFileData = param.AttachmentBuffer.ClientFileData
                }, location, sid);

                // Assign saved paths
                var property = typeof(WorkingInputDTO).GetProperty("Attachment");

                if (property != null)
                {
                    property?.SetValue(param, uploadFileResponse.SavedAs);
                    uploadFiles.Add(uploadFileResponse);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": Attachment" });
                }

            }
            if (param.AttachmentBuffer == null)
            {
                var e = _coreDbContext.Workings.AsNoTracking().Where(p => p.ID == param.Id).First();
                param.Attachment = e.ATTACHMENT;
            }

            var r = await _unitOfWork.WorkingRepository.UpdateAsync(param);

            if (r.StatusCode == "200" && r.InnerBody != null)
            {
                var rsObjData = (WorkingInputDTO)r.InnerBody;
                if (rsObjData.IsNotResign.HasValue && rsObjData.IsNotResign.Value == true)
                {
                    if (param.IsResponsible.HasValue && param.IsResponsible == true)
                    {
                        var arisingObj = new InsArisingDTO()
                        {
                            PkeyRef = rsObjData.Id,
                            EmployeeId = param.EmployeeId,
                            EffectDate = param.EffectDate,
                            TableRef = "WORKING"
                        };
                        //dk pó cu khac pos
                        var s = await _insArisingRepository.InsertArising(_uow, arisingObj, sid);
                    }
                    if ((!param.IsResponsible.HasValue || param.IsResponsible == false))
                    {
                        await _unitOfWork.WorkingRepository.UpdateAsync(param);
                        var sys = _coreDbContext.SysOtherLists.Where(x => x.CODE == "DD").FirstOrDefault();

                        var waitApprove = _coreDbContext.SysOtherLists.Where(x => x.CODE == "CD").FirstOrDefault();
                        var rejectApprove = _coreDbContext.SysOtherLists.Where(x => x.CODE == "TCPD").FirstOrDefault();

                        if (param.StatusId.HasValue && param.StatusId == sys!.ID)
                        {
                            var concurentlyExist = _fullDbContext.HuConcurrentlys.Where(x => x.EMPLOYEE_ID == param.EmployeeId && x.IS_FROM_WORKING == true && x.DECISION_NUMBER == param.DecisionNo && x.EFFECTIVE_DATE == param.EffectDate && x.WORKING_ID == param.Id).FirstOrDefault();
                            if (concurentlyExist == null)
                            {
                                var join = (from e2 in _fullDbContext.HuEmployees.Where(x => x.ID == param.SignId)
                                            from q in _fullDbContext.HuEmployeeCvs.Where(x => x.ID == e2.PROFILE_ID).DefaultIfEmpty()
                                            from p in _fullDbContext.HuPositions.Where(x => x.ID == e2.POSITION_ID).DefaultIfEmpty()
                                                // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                            select new
                                            {
                                                SigningPositionName = p.NAME,
                                            }).FirstOrDefault();
                                var positionConcurrently = new HU_CONCURRENTLY()
                                {
                                    EFFECTIVE_DATE = param.EffectDate,
                                    EXPIRATION_DATE = param.ExpireDate,
                                    EMPLOYEE_ID = param.EmployeeId,
                                    DECISION_NUMBER = param.DecisionNo,
                                    ORG_ID = param.OrgId,
                                    POSITION_ID = param.PositionId,
                                    STATUS_ID = param.StatusId,
                                    SIGNING_DATE = param.SignDate,
                                    SIGNING_EMPLOYEE_ID = param.SignId,
                                    NOTE = param.Note,
                                    IS_FROM_WORKING = true,
                                    SIGNING_POSITION_NAME = (join != null) ? join.SigningPositionName : "",
                                    WORKING_ID = param.Id
                                };
                                var s = await _fullDbContext.HuConcurrentlys.AddAsync(positionConcurrently);
                                await _fullDbContext.SaveChangesAsync();
                                return Ok(new FormatedResponse() { InnerBody = positionConcurrently, MessageCode = CommonMessageCode.UPDATE_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 });
                            }
                            else
                            {
                                return Ok(new FormatedResponse()
                                {
                                    InnerBody = true,
                                    MessageCode = CommonMessageCode.UPDATE_SUCCESS,
                                    StatusCode = EnumStatusCode.StatusCode200
                                });
                            }

                        }
                        else if (param.StatusId.HasValue && (param.StatusId == waitApprove!.ID || param.StatusId == rejectApprove!.ID))
                        {
                            return Ok(new FormatedResponse()
                            {
                                InnerBody = true,
                                MessageCode = CommonMessageCode.UPDATE_SUCCESS,
                                StatusCode = EnumStatusCode.StatusCode200
                            });
                        }
                        else
                        {
                            return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 });
                        }
                    }
                }
                return Ok(new FormatedResponse() { InnerBody = r.Data, MessageCode = CommonMessageCode.UPDATE_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 });
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
            }
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatusApprove(WorkingInputDTO request)
        {
            var sid = Request.Sid(_appSettings);
            foreach (var item in request.ids)
            {
                var r = _coreDbContext.Workings.Where(x => x.ID == item).FirstOrDefault();
                var statusIdCur = r.STATUS_ID;
                r.UPDATED_DATE = DateTime.UtcNow;
                r.UPDATED_BY = sid;
                r.STATUS_ID = request.ValueToBind == true ? OtherConfig.STATUS_APPROVE : (request.ValueToBind == false ? OtherConfig.STATUS_WAITING : null);
                var isResign = await _unitOfWork.WorkingRepository.CheckIsResign(r);
                
                if (statusIdCur != OtherConfig.STATUS_APPROVE && request.ValueToBind == true)
                {
                    if (isResign)
                    {
                        if (r.IS_RESPONSIBLE.HasValue && r.IS_RESPONSIBLE == true)
                        {
                            var arisingObj = new InsArisingDTO()
                            {
                                PkeyRef = r.ID,
                                EmployeeId = r.EMPLOYEE_ID,
                                EffectDate = r.EFFECT_DATE,
                                TableRef = "WORKING"
                            };
                            var s = await _insArisingRepository.InsertArising(_uow, arisingObj, sid);
                        }
                        else
                        {
                            var sys = _coreDbContext.SysOtherLists.Where(x => x.CODE == "DD").FirstOrDefault();
                            if (r.STATUS_ID.HasValue && r.STATUS_ID == sys!.ID)
                            {
                                var concurentlyExist = _fullDbContext.HuConcurrentlys.Where(x => x.EMPLOYEE_ID == r.EMPLOYEE_ID && x.IS_FROM_WORKING == true && x.DECISION_NUMBER == r.DECISION_NO && x.EFFECTIVE_DATE == r.EFFECT_DATE && x.WORKING_ID == r.ID).FirstOrDefault();
                                if(concurentlyExist == null)
                                {
                                    var join = await (from e2 in _fullDbContext.HuEmployees.Where(x => x.ID == request.SignId)
                                                      from q in _fullDbContext.HuEmployeeCvs.Where(x => x.ID == e2.PROFILE_ID).DefaultIfEmpty()
                                                      from p in _fullDbContext.HuPositions.Where(x => x.ID == e2.POSITION_ID).DefaultIfEmpty()
                                                          // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                                      select new
                                                      {
                                                          SigningPositionName = p.NAME,
                                                      }).FirstOrDefaultAsync();
                                    var positionConcurrently = new HU_CONCURRENTLY()
                                    {
                                        EFFECTIVE_DATE = r.EFFECT_DATE,
                                        EXPIRATION_DATE = r.EXPIRE_DATE,
                                        EMPLOYEE_ID = r.EMPLOYEE_ID,
                                        DECISION_NUMBER = r.DECISION_NO,
                                        ORG_ID = r.ORG_ID,
                                        POSITION_ID = r.POSITION_ID,
                                        STATUS_ID = r.STATUS_ID,
                                        SIGNING_DATE = r.SIGN_DATE,
                                        SIGNING_EMPLOYEE_ID = r.SIGN_ID,
                                        NOTE = r.NOTE,
                                        IS_FROM_WORKING = true,
                                        SIGNING_POSITION_NAME = join != null ? join.SigningPositionName : null,
                                        WORKING_ID = r.ID
                                    };
                                    var s = await _fullDbContext.HuConcurrentlys.AddAsync(positionConcurrently);
                                    await _fullDbContext.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 });
                            }
                        }
                    }
                    else
                    {
                        if (request.IsResponsible == false)
                        {
                            var entity = _fullDbContext.HuConcurrentlys.Where(x => x.WORKING_ID == request.Id).FirstOrDefault();
                            if (entity != null)
                            {
                                _fullDbContext.HuConcurrentlys.Remove(entity);
                                _fullDbContext.SaveChanges();
                            }
                        }
                    }
                }
                var result = _coreDbContext.Workings.Update(r);
            }
            await _coreDbContext.SaveChangesAsync();
            return Ok(new FormatedResponse() { MessageCode = request.ValueToBind == true ? CommonMessageCode.APPROVE_SUCCESS : (request.ValueToBind == false ? CommonMessageCode.PENDING_APPROVE_SUCCESS : ""), StatusCode = EnumStatusCode.StatusCode200 });

        }
        [HttpPost]
        public async Task<ActionResult> Remove([FromBody] List<long> id)
        {
            var r = await _unitOfWork.WorkingRepository.RemoveAsync(id);
            return Ok(new FormatedResponse() { InnerBody = r.Data });
        }
        [HttpPost]
        public async Task<ActionResult> OpenStatus([FromBody] long id)
        {
            var r = await _unitOfWork.WorkingRepository.OpenStatus(id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> TemplateImport([FromBody] int orgId)
        {
            try
            {
                var stream = await _unitOfWork.WorkingRepository.TemplateImport(orgId);
                var fileName = "tempWorking.xlsx";
                if (stream.StatusCode == "200")
                {
                    return new FileStreamResult(stream.memoryStream, "application/octet-stream") { FileDownloadName = fileName };
                }
                return ResponseResult(stream);
            }
            catch (Exception ex)
            {
                return ResponseResult(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> ImportTemplate([FromBody] ImportParam param)
        {
            try
            {
                var r = await _unitOfWork.WorkingRepository.ImportTemplate(param);
                if (r.memoryStream != null && r.StatusCode == "200")
                {
                    var fileName = "tempWorkingError.xlsx";
                    return new FileStreamResult(r.memoryStream, "application/octet-stream") { FileDownloadName = fileName };
                }
                return ImportResult(r);
            }
            catch (Exception ex)
            {
                return ResponseResult(ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult> PortalGetAll()
        {
            var r = await _unitOfWork.WorkingRepository.PortalGetAll();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> PortalGetBy(long id)
        {
            var r = await _unitOfWork.WorkingRepository.PortalGetBy(id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            try
            {
                if (model.Ids != null)
                {
                    var checkStatus = (from p in _coreDbContext.Workings where model.Ids.Contains(p.ID) && p.STATUS_ID == OtherConfig.STATUS_APPROVE select p).ToList().Count();
                    if (checkStatus > 0)
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE, StatusCode = EnumStatusCode.StatusCode400 });
                    }
                    var response = await _genericRepository.DeleteIds(_uow, model.Ids);
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
        public async Task<IActionResult> GetDecisionNoWorking()
        {
            decimal num;
            string formatCode = "/QĐ-VNS";
            string formatDecisionNo = "xxxx" + formatCode;
            int numOfDecisionNo = 4;
            var queryCode = (from x in _coreDbContext.Workings where x.DECISION_NO.Length == 11 && x.IS_WAGE != -1 select x.DECISION_NO).ToList();
            var existingCode = (from p in queryCode where Decimal.TryParse(p.Substring(0, numOfDecisionNo), out num) orderby p descending select p).FirstOrDefault();

            string newcode = "";
            //string newcode = StringCodeGenerator.CreateNewCode(formatCode, numOfDecisionNo, existingCode, "RIGHT");
            newcode = (int.Parse(existingCode.Substring(0, 4)) + 1).ToString("D4") + formatCode;
            return Ok(new FormatedResponse() { InnerBody = new { Code = newcode } });
        }
        //WAGE
        [HttpPost]
        public async Task<IActionResult> QueryListWage(GenericQueryListDTO<HuWorkingDTO> request)
        {
            try
            {
                /* HERE WE CAN EITHER CHOOSE ONE OF THE 2 APPROACHS: TwoPhaseQueryList or SinglePhaseQueryList */
                /* WE WILL KNOW WHAT APPROACH IS FASTER IN THE FUTURE */

                var response = await _unitOfWork.WorkingRepository.TwoPhaseQueryListWage(request);

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

        [HttpGet]
        public async Task<IActionResult> GetInsideCompanyByEmployee(long id)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                var entity = _uow.Context.Set<HU_WORKING>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var companies = _uow.Context.Set<HU_COMPANY>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();

                var joined = await (from p in entity
                                    from e in employees.Where(c => c.ID == p.EMPLOYEE_ID)
                                    from t in positions.Where(c => c.ID == p.POSITION_ID).DefaultIfEmpty()
                                    from lmp in positions.Where(c => c.ID == t.LM).DefaultIfEmpty()
                                    from lm in employees.Where(c => c.ID == lmp.MASTER).DefaultIfEmpty()
                                    from o in organizations.Where(c => c.ID == p.ORG_ID).DefaultIfEmpty()
                                    from f in otherLists.Where(c => c.ID == p.STATUS_ID).DefaultIfEmpty()
                                    from l in otherLists.Where(c => c.ID == p.TYPE_ID).DefaultIfEmpty()
                                    from s in employees.Where(c => c.ID == p.SIGN_ID).DefaultIfEmpty()
                                    from c in companies.Where(c => c.ID == o.COMPANY_ID).DefaultIfEmpty()
                                    from eo in otherLists.Where(c => c.ID == p.EMPLOYEE_OBJ_ID).DefaultIfEmpty()
                                    where p.IS_WAGE == null && p.EMPLOYEE_ID == id && f.CODE == "DD"
                                    orderby p.EFFECT_DATE
                                    select new
                                    {
                                        Id = p.ID,
                                        OrgId = p.ORG_ID,
                                        EmployeeId = e.ID,
                                        EmployeeCode = e.CODE,
                                        EmployeeName = e.Profile.FULL_NAME,
                                        PositionName = t.NAME,
                                        SignDate = p.SIGN_DATE,
                                        SignerName = s.Profile.FULL_NAME,
                                        SignerCode = s.CODE,
                                        SignerPosition = p.SIGNER_POSITION,
                                        OrgName = o.NAME,
                                        EffectDate = p.EFFECT_DATE,
                                        ExpireDate = p.EXPIRE_DATE,
                                        DecisionNo = p.DECISION_NO,
                                        StatusName = f.NAME,
                                        TypeName = l.NAME,
                                        LmName = lm.Profile.FULL_NAME,
                                        LmPosition = lmp.NAME,
                                        WorkAddress = c.WORK_ADDRESS,
                                        EmployeeOject = eo.NAME,
                                        Note = p.NOTE,
                                        WorkTime = (p.EXPIRE_DATE == null || p.EXPIRE_DATE < currentDate) ? (Math.Round((currentDate - p.EFFECT_DATE!.Value).TotalDays / 30, 2).ToString() + " tháng") : (Math.Round((p.EXPIRE_DATE!.Value - p.EFFECT_DATE!.Value).TotalDays / 30, 2).ToString() + " tháng"),
                                        //WorkTime = (p.EXPIRE_DATE == null || p.EXPIRE_DATE < currentDate) ? (Math.Round((currentDate - p.EFFECT_DATE!.Value).TotalDays / 365.25, 2).ToString() + " năm") : (Math.Round((p.EXPIRE_DATE!.Value - p.EFFECT_DATE!.Value).TotalDays / 365.25, 2).ToString() + " năm"),
                                    }).ToListAsync();

                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetWorkingByEmployee(long id)
        {
            try
            {
                var entity = _uow.Context.Set<HU_WORKING>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var salaryTypes = _uow.Context.Set<HU_SALARY_TYPE>().AsNoTracking().AsQueryable();
                var salaryRanks = _uow.Context.Set<HU_SALARY_RANK>().AsNoTracking().AsQueryable();
                var salaryScales = _uow.Context.Set<HU_SALARY_SCALE>().AsNoTracking().AsQueryable();
                var salaryLevels = _uow.Context.Set<HU_SALARY_LEVEL>().AsNoTracking().AsQueryable();
                var companyInfos = _uow.Context.Set<HU_COMPANY>().AsNoTracking().AsQueryable();

                var joined = await (from p in entity
                                    from e in employees.Where(c => c.ID == p.EMPLOYEE_ID)
                                    from t in positions.Where(c => c.ID == p.POSITION_ID)
                                    from o in organizations.Where(c => c.ID == p.ORG_ID)
                                    from f in otherLists.Where(c => c.ID == p.STATUS_ID).DefaultIfEmpty()
                                    from l in otherLists.Where(c => c.ID == p.TYPE_ID)
                                    from s in employees.Where(c => c.ID == p.SIGN_ID).DefaultIfEmpty()
                                    from tl in salaryTypes.Where(c => c.ID == p.SALARY_TYPE_ID).DefaultIfEmpty()
                                    from sc in salaryScales.Where(c => c.ID == p.SALARY_SCALE_ID).DefaultIfEmpty()
                                    from sdcv in salaryScales.Where(c => c.ID == p.SALARY_SCALE_DCV_ID).DefaultIfEmpty()
                                    from ra in salaryRanks.Where(c => c.ID == p.SALARY_RANK_ID).DefaultIfEmpty()
                                    from sl in salaryLevels.Where(c => c.ID == p.SALARY_LEVEL_ID).DefaultIfEmpty()
                                    from sldcv in salaryLevels.Where(c => c.ID == p.SALARY_LEVEL_DCV_ID).DefaultIfEmpty()
                                    from tax in otherLists.Where(c => c.ID == p.TAXTABLE_ID).DefaultIfEmpty()
                                    from rdcv in salaryRanks.Where(c => c.ID == p.SALARY_RANK_DCV_ID).DefaultIfEmpty()
                                    from ci in companyInfos.Where(c => c.ID == o.COMPANY_ID).DefaultIfEmpty()
                                    from r in otherLists.Where(x => x.ID == ci.REGION_ID).DefaultIfEmpty()
                                    where p.IS_WAGE == -1 && p.EMPLOYEE_ID == id && f.CODE == "DD"
                                    orderby p.EFFECT_DATE
                                    select new
                                    {
                                        Id = p.ID,
                                        OrgId = p.ORG_ID,
                                        EmployeeCode = e.CODE,
                                        EmployeeName = e.Profile.FULL_NAME,
                                        PositionName = t.NAME,
                                        EmployeeObjId = p.EMPLOYEE_OBJ_ID,
                                        SignDate = p.SIGN_DATE,
                                        SignerName = p.SIGNER_NAME,
                                        SignerCode = s.CODE,
                                        SignerPosition = p.SIGNER_POSITION,
                                        OrgName = o.NAME,
                                        EffectDate = p.EFFECT_DATE,
                                        ExpireDate = p.EXPIRE_DATE,
                                        DecisionNo = p.DECISION_NO,
                                        StatusName = f.NAME,
                                        TypeName = l.NAME,
                                        SalBasic = p.SAL_BASIC,
                                        ShortTempSalary = p.SHORT_TEMP_SALARY,
                                        RegionName = r.NAME,
                                        TaxTableName = tax.NAME,
                                        Coefficient = p.COEFFICIENT,
                                        CoefficientDcv = p.COEFFICIENT_DCV,
                                        SalTotal = p.SAL_TOTAL,
                                        SalPercent = p.SAL_PERCENT,
                                        SalaryType = tl.NAME,
                                        salaryRankDcvName = rdcv.NAME,
                                        SalaryScaleDcvName = sdcv.NAME,
                                        SalaryScaleName = sc.NAME,
                                        SalaryRankName = ra.NAME,
                                        SalaryLevelName = sl.NAME,
                                        salaryLevelDcvName = sldcv.NAME,
                                        Note = p.NOTE
                                    }).ToListAsync();
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetAllWage(WorkingDTO param)
        {
            var r = await _unitOfWork.WorkingRepository.GetAllWage(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetWage(WorkingDTO param)
        {
            var r = await _unitOfWork.WorkingRepository.GetWorking(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetWageById(long Id)
        {
            try
            {
                var r = await _unitOfWork.WorkingRepository.GetWageById(Id);
                return Ok(new FormatedResponse() { InnerBody = r.Data });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        //[HttpGet]
        //public async Task<ActionResult> GetLastWage(long Id)
        //{
        //    var r = await _unitOfWork.WorkingRepository.GetLastWage(Id);
        //    return ResponseResult(r);
        //}
        [HttpPost]
        public async Task<ActionResult> CreateWage([FromBody] WorkingInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var sid = Request.Sid(_appSettings);
            if (param.AttachmentBuffer != null)
            {
                List<UploadFileResponse> uploadFiles = new();

                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                {
                    ClientFileName = param.AttachmentBuffer.ClientFileName,
                    ClientFileType = param.AttachmentBuffer.ClientFileType,
                    ClientFileData = param.AttachmentBuffer.ClientFileData
                }, location, sid);

                // Assign saved paths
                var property = typeof(WorkingInputDTO).GetProperty("Attachment");

                if (property != null)
                {
                    property?.SetValue(param, uploadFileResponse.SavedAs);
                    uploadFiles.Add(uploadFileResponse);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": Attachment" });
                }

            }
            var r = await _unitOfWork.WorkingRepository.CreateWageAsync(param, sid);
            if (r.StatusCode == "200" && r.InnerBody != null)
            {
                var rsObjData = (WorkingInputDTO)r.InnerBody;
                var arisingObj = new API.DTO.InsArisingDTO()
                {
                    PkeyRef = rsObjData.Id,
                    EmployeeId = param.EmployeeId,
                    EffectDate = param.EffectDate,
                    TableRef = "WAGE"
                };
                var s = await _insArisingRepository.InsertArising(_uow, arisingObj, sid);
                return Ok(new FormatedResponse() { InnerBody = r.Data, MessageCode = CommonMessageCode.CREATE_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 });
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
            }
        }
        [HttpPost]
        public async Task<ActionResult> UpdateWage(WorkingInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var sid = Request.Sid(_appSettings);

            var checkContract = _coreDbContext.Contracts.Where(p => p.WORKING_ID == param.Id && p.STATUS_ID == OtherConfig.STATUS_APPROVE).Any();
            if (checkContract)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_UPDATE_BECAUSE_CONTRACT_IS_APPROVE, StatusCode = EnumStatusCode.StatusCode400 });
            }

            if (param.AttachmentBuffer != null)
            {
                List<UploadFileResponse> uploadFiles = new();

                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                {
                    ClientFileName = param.AttachmentBuffer.ClientFileName,
                    ClientFileType = param.AttachmentBuffer.ClientFileType,
                    ClientFileData = param.AttachmentBuffer.ClientFileData
                }, location, sid);

                // Assign saved paths
                var property = typeof(WorkingInputDTO).GetProperty("Attachment");

                if (property != null)
                {
                    property?.SetValue(param, uploadFileResponse.SavedAs);
                    uploadFiles.Add(uploadFileResponse);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": Attachment" });
                }

            }
            else
            {
                var item = _uow.Context.Set<HU_WORKING>().Where(x => x.ID == param.Id).FirstOrDefault();
                param.Attachment = item?.ATTACHMENT;
            }
            var r = await _unitOfWork.WorkingRepository.UpdateWageAsync(param, sid);
            if (r.StatusCode == "200")
            {
                var rsObjData = (WorkingInputDTO)r.InnerBody;
                var arisingObj = new API.DTO.InsArisingDTO()
                {
                    PkeyRef = rsObjData.Id,
                    EmployeeId = param.EmployeeId,
                    EffectDate = param.EffectDate,
                    TableRef = "WAGE"
                };
                var s = await _insArisingRepository.InsertArising(_uow, arisingObj, sid);
                return Ok(new FormatedResponse() { InnerBody = r.Data, MessageCode = CommonMessageCode.UPDATE_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 });
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
            }
        }
        [HttpPost]
        public async Task<ActionResult> RemoveWage([FromBody] List<long> id)
        {
            var r = await _unitOfWork.WorkingRepository.RemoveWageAsync(id);
            return Ok(new FormatedResponse() { InnerBody = r.Data });
        }
        [HttpPost]
        public async Task<ActionResult> OpenStatusWage([FromBody] long id)
        {
            var r = await _unitOfWork.WorkingRepository.OpenStatusWage(id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> TemplateImportWage([FromBody] int orgId)
        {
            try
            {
                var stream = await _unitOfWork.WorkingRepository.TemplateImport(orgId);
                var fileName = "tempWage.xlsx";
                if (stream.StatusCode == "200")
                {
                    return new FileStreamResult(stream.memoryStream, "application/octet-stream") { FileDownloadName = fileName };
                }
                return ResponseResult(stream);
            }
            catch (Exception ex)
            {
                return ResponseResult(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> ImportTemplateWage([FromBody] ImportParam param)
        {
            try
            {
                var r = await _unitOfWork.WorkingRepository.ImportTemplate(param);
                if (r.memoryStream != null && r.StatusCode == "200")
                {
                    var fileName = "tempWageError.xlsx";
                    return new FileStreamResult(r.memoryStream, "application/octet-stream") { FileDownloadName = fileName };
                }
                return ImportResult(r);
            }
            catch (Exception ex)
            {
                return ResponseResult(ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult> PortalGetAllWage()
        {
            var r = await _unitOfWork.WorkingRepository.PortalWageGetAll();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> PortalGetByWage(long id)
        {
            var r = await _unitOfWork.WorkingRepository.PortalWageGetBy(id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteIdsWage(IdsRequest model)
        {
            try
            {
                if (model.Ids != null)
                {
                    var checkStatus = (from p in _coreDbContext.Workings where model.Ids.Contains(p.ID) && (p.STATUS_ID == OtherConfig.STATUS_APPROVE || p.STATUS_ID == OtherConfig.STATUS_DECLINE) select p).ToList().Count();
                    if (checkStatus > 0)
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE_DECLINE, StatusCode = EnumStatusCode.StatusCode400 });
                    }
                    var checkUsedContract = (from p in _coreDbContext.Contracts where model.Ids.Contains((long)p.WORKING_ID) select p).ToList().Count();
                    var checkUsedWage = (from p in _coreDbContext.Workings where model.Ids.Contains((long)p.WAGE_ID) select p).ToList().Count();
                    var checkUsedArisings = (from p in _coreDbContext.Arisings where model.Ids.Contains((long)p.PKEY_REF) && p.TABLE_REF == "WAGE" select p).ToList().Count();
                    var checkUsedSheetSum = (from p in _coreDbContext.PaPayrollsheetSums where model.Ids.Contains((long)p.WORKING_ID) select p).ToList().Count();
                    var checkUsedSheetBackdate = (from p in _coreDbContext.PaPayrollsheetSumBackdates where model.Ids.Contains((long)p.WORKING_ID) select p).ToList().Count();
                    if (checkUsedContract > 0 || checkUsedArisings > 0 || checkUsedSheetSum > 0 || checkUsedSheetBackdate > 0 || checkUsedWage > 0)
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DATA_HAS_USED, StatusCode = EnumStatusCode.StatusCode400 });
                    }
                    var response = await _genericRepository.DeleteIds(_uow, model.Ids);
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
        public async Task<IActionResult> CalculateExpireShortTemp(long? empId, string? date, long? levelId)
        {
            try
            {
                var r = await _unitOfWork.WorkingRepository.CalculateExpireShortTemp(empId, date.Replace("/", "_"), levelId);
                return Ok(new FormatedResponse() { InnerBody = r.Data });

            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUpsal(WorkingInputDTO request)
        {
            try
            {
                var r = await _unitOfWork.WorkingRepository.UpdateUpsal(request);
                if (r.StatusCode == "200")
                {
                    return Ok(new FormatedResponse() { InnerBody = r.Data, MessageCode = CommonMessageCode.UPDATE_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 });
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
                }

            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> checkDecisionMaster(WorkingInputDTO request)
        {
            try
            {
                var r = await _unitOfWork.WorkingRepository.checkDecisionMaster(request);
                if (r.StatusCode == "200" || r.StatusCode == "204")
                {
                    return Ok(new FormatedResponse() { InnerBody = r.Data, MessageCode = "", StatusCode = r.StatusCode == "204" ? EnumStatusCode.StatusCode204 : EnumStatusCode.StatusCode200 });
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
                }

            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatusApproveWage(WorkingInputDTO request)
        {
            var sid = Request.Sid(_appSettings);

            foreach (var item in request.ids)
            {
                var r = _coreDbContext.Workings.Where(x => x.ID == item).FirstOrDefault();
                var statusIdCur = r.STATUS_ID;
                r.UPDATED_DATE = DateTime.UtcNow;
                r.UPDATED_BY = sid;
                r.STATUS_ID = request.ValueToBind == true ? OtherConfig.STATUS_APPROVE : (request.ValueToBind == false ? OtherConfig.STATUS_WAITING : null);
                if (statusIdCur != OtherConfig.STATUS_APPROVE && request.ValueToBind == true)
                {
                    var arisingObj = new InsArisingDTO()
                    {
                        PkeyRef = r.ID,
                        EmployeeId = r.EMPLOYEE_ID,
                        EffectDate = r.EFFECT_DATE,
                        TableRef = "WAGE"
                    };
                    var s = await _insArisingRepository.InsertArising(_uow, arisingObj, sid);
                }
                var result = _coreDbContext.Workings.Update(r);
            }
            await _coreDbContext.SaveChangesAsync();
            return Ok(new FormatedResponse() { MessageCode = request.ValueToBind == true ? CommonMessageCode.APPROVE_SUCCESS : (request.ValueToBind == false ? CommonMessageCode.PENDING_APPROVE_SUCCESS : ""), StatusCode = EnumStatusCode.StatusCode200 });
        }
        

        [HttpGet]
        public async Task<IActionResult> PrintHuWorking(long id)
        {
            try
            {
                var QueryData = new SqlQueryDataTemplate(_fullDbContext);

                var fileType = (from w in _fullDbContext.HuWorkings
                                join o in _fullDbContext.SysOtherLists on w.TYPE_ID equals o.ID into oGroup
                                from o in oGroup.DefaultIfEmpty()
                                where w.ID == id
                                select new
                                {
                                    CODE = o.CODE,
                                }).FirstOrDefault();
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.WordTemplates);
                string relativePath = "";
                int type = 0;
                if (fileType != null)
                {
                    if (fileType.CODE == "BP")
                    {
                        relativePath = "QĐ biệt phái cán bộ.docx";
                    }
                    else if (fileType.CODE == "BN")
                    {
                        relativePath = "QĐ bổ nhiệm.docx";
                    }
                    else if (fileType.CODE == "CDHD")
                    {
                        relativePath = "QD chấm dứt HĐLĐ.doc";
                    }
                    else if (fileType.CODE == "CT")
                    {
                        relativePath = "QD tiếp nhận cán bộ.docx";
                    }
                    else if (fileType.CODE == "TDNS")
                    {
                        relativePath = "QD tuyển dụng nhân sự.docx";
                    }
                    else if (fileType.CODE == "DDCV")
                    {
                        relativePath = "QĐ về điều động chuyên viên.doc";
                    }
                    else if (fileType.CODE == "DDBN")
                    {
                        relativePath = "QĐ về điều động, bổ nhiệm cán bộ.doc";
                    }
                    else if (fileType.CODE == "TNVKN")
                    {
                        relativePath = "QĐ về thôi nhiệm vụ kiêm nhiệm.doc";
                    }
                    else if (fileType.CODE == "MN")
                    {
                        relativePath = "QĐ miễn nhiệm.doc";
                    }
                    else if (fileType.CODE == "BLN")
                    {
                        relativePath = "QĐ bổ nhiệm lại cán bộ.docx";
                    }
                    else if (fileType.CODE == "00079")
                    {
                        relativePath = "QDNL_NLD_1.docx";
                        type = 1;
                    }
                    else if (fileType.CODE == "QDTL")
                    {
                        relativePath = "QDNL_NLD_NQL.docx";
                        type = 1;
                    }
                    else if (fileType.CODE == "00091")
                    {
                        relativePath = "QDNL_NLD_2.docx";
                        type = 1;
                    }
                    else if (fileType.CODE == "00078")
                    {
                        relativePath = "QDNL_NLD_3.docx";
                        type = 1;
                    }
                    else if (fileType.CODE == "00073")
                    {
                        relativePath = "6. VEAM_Quyet dinh nang luong.docx";
                        type = 2;
                    }
                    else if (fileType.CODE == "00067")
                    {
                        relativePath = "5. VEAM_Quyet dinh xep luong.doc";
                        type = 3;
                    }
                    else
                    {
                        return Ok(new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.UNCATCHABLE,
                            MessageCode = "Loại quyết định không tồn tại mẫu in !",
                            StatusCode = EnumStatusCode.StatusCode400,
                        });
                    }
                    var absolutePath = Path.Combine(location, relativePath);
                    DataSet dataSet = new("MyDataSet");
                    dataSet = QueryData.ExecuteStoreToTable(type == 0 ? "PKG_PRINT_DECISION" : (type == 1 ? "PKG_PRINT_DECISION_WAGE"
                                                            : (type == 2 ? "PKG_PRINT_DECISION_TO_RAISE_SALARY" : "PKG_QUAN_LY_XEP_LUONG")), new
                                                            {
                                                                P_ID = id
                                                            }, false);
                    var file = await _wordRespsitory.ExportWordNoImage(dataSet, absolutePath);
                    return File(file, "application/octet-stream", relativePath);
                }

                return Ok(null);

            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message,
                    StatusCode = EnumStatusCode.StatusCode400,
                });
            }

        }



        [HttpGet]
        public async Task<IActionResult> GetFileName(long id)
        {
            try
            {
                var QueryData = new SqlQueryDataTemplate(_fullDbContext);

                var fileType = await (from w in _fullDbContext.HuWorkings
                                      join o in _fullDbContext.SysOtherLists on w.TYPE_ID equals o.ID
                                      join e in _fullDbContext.HuEmployees on w.EMPLOYEE_ID equals e.ID
                                      join cv in _fullDbContext.HuEmployeeCvs on e.PROFILE_ID equals cv.ID
                                      where w.ID == id
                                      select new
                                      {
                                          CODE = o.CODE,
                                          EMPLOYEE_NAME = cv.FULL_NAME,
                                          EMPLOYEE_CODE = e.CODE
                                      }).FirstOrDefaultAsync();
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.WordTemplates);
                string relativePath = "";
                if (fileType != null)
                {
                    if (fileType.CODE == "BP")
                    {
                        relativePath = "QĐ biệt phái cán bộ " + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    else if (fileType.CODE == "BN")
                    {
                        relativePath = "QĐ bổ nhiệm.docx" + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    else if (fileType.CODE == "CDHD")
                    {
                        relativePath = "QD chấm dứt HĐLĐ " + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    else if (fileType.CODE == "CT")
                    {
                        relativePath = "QD tiếp nhận cán bộ" + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    else if (fileType.CODE == "TDNS")
                    {
                        relativePath = "QD tuyển dụng nhân sự" + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    else if (fileType.CODE == "DDCV")
                    {
                        relativePath = "QĐ về điều động chuyên viên" + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    else if (fileType.CODE == "DDBN")
                    {
                        relativePath = "QĐ về điều động, bổ nhiệm cán bộ" + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    else if (fileType.CODE == "TNVKN")
                    {
                        relativePath = "QĐ về thôi nhiệm vụ kiêm nhiệm" + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    else if (fileType.CODE == "MN")
                    {
                        relativePath = "QĐ miễn nhiệm" + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    else if (fileType.CODE == "BLN")
                    {
                        relativePath = "QĐ bổ nhiệm lại cán bộ" + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    else if (fileType.CODE == "00079")
                    {
                        relativePath = "QDNL_NLD_1" + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    else if (fileType.CODE == "QDTL")
                    {
                        relativePath = "QDNL_NLD_NQL" + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    else if (fileType.CODE == "00091")
                    {
                        relativePath = "QDNL_NLD_2" + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    else if (fileType.CODE == "00078")
                    {
                        relativePath = "QDNL_NLD_3" + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    else if (fileType.CODE == "00073")
                    {
                        relativePath = "6. VEAM_Quyet dinh nang luong" + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    else if (fileType.CODE == "00067")
                    {
                        relativePath = "5. VEAM_Quyet dinh xep luong" + "_" + fileType.EMPLOYEE_NAME + "_" + fileType.EMPLOYEE_CODE;
                    }
                    return Ok(new FormatedResponse() { InnerBody = relativePath });
                }
            }
            catch (Exception ex)
            {
            }
            return null;

        }

        [HttpGet]
        public async Task<ActionResult> ScanApproveWorkings()
        {
            _unitOfWork.WorkingRepository.ScanApproveWorkings();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GetSalaryMinimumOfRegion(ContextModel request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            var response = await _unitOfWork.WorkingRepository.GetSalaryMinimumOfRegion(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetFileNameWorkingProcess(long id)
        {
            try
            {
                // lấy id của "Quyết định thôi giữ chức vụ"
                var id1 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "TGCV")?.ID;

                // lấy id của "Quyết định thôi miễn nhiệm chức vụ"
                var id2 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "MN")?.ID;

                // lấy id của "Quyết định thôi bãi nhiệm chức vụ"
                var id3 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "BM")?.ID;

                // lấy id của "Quyết định tiếp nhận cán bộ"
                var id4 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "CT")?.ID;

                // lấy id của "Quyết định tạm hoãn HĐLĐ"
                var id5 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "QDTH")?.ID;

                // lấy id của "Thỏa thuận tạm hoãn HĐLĐ"
                var id6 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "TTTH")?.ID;

                // lấy id của "Quyết định bổ nhiệm"
                var id7 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "BN")?.ID;

                // lấy id của "Quyết định về điều động"
                var id8 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "DDCV")?.ID;

                // lấy id của "Quyết định biệt phái cán bộ"
                var id9 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "BP")?.ID;

                // lấy id của "Quyết định biệt phái cán bộ (Gia hạn)"
                var id10 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "BPGH")?.ID;

                // lấy id của "Thông báo nghỉ hưu"
                var id11 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "00357")?.ID;

                // lấy id của "Thoả thuận chấm dứt hợp đồng lao động"
                var id12 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "00356")?.ID;


                // lấy bản ghi quá trình công tác
                var record = await _fullDbContext.HuWorkings.FirstOrDefaultAsync(x => x.ID == id);

                string fileName = "";

                if (record != null)
                {
                    if (record.TYPE_ID == id1)
                    {
                        fileName = "Quyet_dinh_thoi_giu_chuc_vu";
                    }
                    else if (record.TYPE_ID == id2)
                    {
                        fileName = "Quyet_dinh_thoi_mien_nhiem_chuc_vu";
                    }
                    else if (record.TYPE_ID == id3)
                    {
                        fileName = "Quyet_dinh_thoi_bai_nhiem_chuc_vu";
                    }
                    else if (record.TYPE_ID == id4)
                    {
                        fileName = "Quyet_dinh_tiep_nhan";
                    }
                    else if (record.TYPE_ID == id5)
                    {
                        fileName = "Quyet_dinh_tam_hoan_HDLD";
                    }
                    else if (record.TYPE_ID == id6)
                    {
                        fileName = "Thoa_thuan_tam_hoan_HDLD";
                    }
                    else if (record.TYPE_ID == id7)
                    {
                        fileName = "Quyet_dinh_bo_nhiem";
                    }
                    else if (record.TYPE_ID == id8)
                    {
                        fileName = "Quyet_dinh_ve_dieu_dong";
                    }
                    else if (record.TYPE_ID == id9)
                    {
                        fileName = "Quyet_dinh_biet_phai_can_bo";
                    }
                    else if (record.TYPE_ID == id10)
                    {
                        fileName = "Quyet_dinh_biet_phai_can_bo_gia_han";
                    }
                    else if (record.TYPE_ID == id11)
                    {
                        fileName = "Thong_bao_nghi_huu";
                    }
                    else if (record.TYPE_ID == id12)
                    {
                        fileName = "Thoa_thuan_cham_dut_HDLD";
                    }
                    else
                    {
                        return Ok(new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = CommonMessageCodes.ID_OF_DECISION_DIFFERENT_ID_OF_SYS_OTHER_LIST,
                            StatusCode = EnumStatusCode.StatusCode400
                        });
                    }
                }
                else
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCodes.NOT_FOUND_RECORD_OF_WORKING_PROCESS,
                        StatusCode = EnumStatusCode.StatusCode400
                    });
                }

                return Ok(new FormatedResponse()
                {
                    InnerBody = fileName
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message,
                    StatusCode = EnumStatusCode.StatusCode400
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> PrintWorkingProcess(long id)
        {
            try
            {
                // get file path
                string relativePath = "";
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.WordTemplates);


                // execute stored procedure SQL Server
                var QueryData = new SqlQueryDataTemplate(_fullDbContext);

                DataSet dataSet = new DataSet("MyDataSet");


                // lấy id của "Quyết định thôi giữ chức vụ"
                var id1 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "TGCV")?.ID;

                // lấy id của "Quyết định thôi miễn nhiệm chức vụ"
                var id2 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "MN")?.ID;

                // lấy id của "Quyết định thôi bãi nhiệm chức vụ"
                var id3 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "BM")?.ID;

                // lấy id của "Quyết định tiếp nhận cán bộ"
                var id4 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "CT")?.ID;

                // lấy id của "Quyết định tạm hoãn HĐLĐ"
                var id5 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "QDTH")?.ID;

                // lấy id của "Thỏa thuận tạm hoãn HĐLĐ"
                var id6 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "TTTH")?.ID;

                // lấy id của "Quyết định bổ nhiệm"
                var id7 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "BN")?.ID;

                // lấy id của "Quyết định về điều động"
                var id8 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "DDCV")?.ID;

                // lấy id của "Quyết định biệt phái cán bộ"
                var id9 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "BP")?.ID;

                // lấy id của "Quyết định biệt phái cán bộ (Gia hạn)"
                var id10 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "BPGH")?.ID;

                // lấy id của "Thông báo nghỉ hưu"
                var id11 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "00357")?.ID;

                // lấy id của "Thoả thuận chấm dứt hợp đồng lao động"
                var id12 = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "00356")?.ID;


                // lấy bản ghi quá trình công tác
                var record = await _fullDbContext.HuWorkings.FirstOrDefaultAsync(x => x.ID == id);

                if (record != null)
                {
                    if (record.TYPE_ID == id1)
                    {
                        relativePath = "DECISION_REJECT.doc";

                        dataSet = QueryData.ExecuteStoreToTable(
                            "PKG_PRINT_DECISION_REJECT",
                            new { P_ID = id },
                            false);
                    }
                    else if (record.TYPE_ID == id2)
                    {
                        relativePath = "DECISION_REJECT2.doc";

                        dataSet = QueryData.ExecuteStoreToTable(
                            "PKG_PRINT_DECISION_REJECT",
                            new { P_ID = id },
                            false);
                    }
                    else if (record.TYPE_ID == id3)
                    {
                        relativePath = "DECISION_REJECT3.doc";

                        dataSet = QueryData.ExecuteStoreToTable(
                            "PKG_PRINT_DECISION_REJECT",
                            new { P_ID = id },
                            false);
                    }
                    else if (record.TYPE_ID == id4)
                    {
                        relativePath = "DECISION_RECEIVE.doc";

                        dataSet = QueryData.ExecuteStoreToTable(
                            "PKG_PRINT_DECISION_REJECT",
                            new { P_ID = id },
                            false);
                    }
                    else if (record.TYPE_ID == id5)
                    {
                        relativePath = "DECISON_PAUSE.docx";

                        dataSet = QueryData.ExecuteStoreToTable(
                            "PKG_PRINT_DECISION_PAUSE",
                            new { P_ID = id },
                            false);
                    }
                    else if (record.TYPE_ID == id6)
                    {
                        relativePath = "DEAL_PAUSE.doc";

                        dataSet = QueryData.ExecuteStoreToTable(
                            "PKG_PRINT_DEAL_PAUSE",
                            new { P_ID = id },
                            false);
                    }
                    else if (record.TYPE_ID == id7)
                    {
                        relativePath = "DECISION_APPOINT.docx";

                        dataSet = QueryData.ExecuteStoreToTable(
                            "PKG_PRINT_DECISION_APPOINT",
                            new { P_ID = id },
                            false);
                    }
                    else if (record.TYPE_ID == id8)
                    {
                        relativePath = "DECISION_ON_MOBILIZATION.docx";

                        dataSet = QueryData.ExecuteStoreToTable(
                            "PKG_PRINT_DECISION_ON_MOBILIZATION",
                            new { P_ID = id },
                            false);
                    }
                    else if (record.TYPE_ID == id9)
                    {
                        relativePath = "DECISION_CHANGE_WORK_ADDRESS.doc";

                        dataSet = QueryData.ExecuteStoreToTable(
                            "PKG_PRINT_DECISION_CHANGE_WORK_ADDRESS",
                            new { P_ID = id },
                            false);
                    }
                    else if (record.TYPE_ID == id10)
                    {
                        relativePath = "DECISION_CHANGE_WORK_ADDRESS_PROLONG.docx";

                        dataSet = QueryData.ExecuteStoreToTable(
                            "PKG_PRINT_DECISION_CHANGE_WORK_ADDRESS_PROLONG",
                            new { P_ID = id },
                            false);
                    }
                    else if (record.TYPE_ID == id11)
                    {
                        relativePath = "VEAM_Thong bao nghi huu.doc";

                        dataSet = QueryData.ExecuteStoreToTable(
                            "PKG_PRINT_NOTI_RETIREMENT_2",
                            new { P_ID = id },
                            false);
                    }
                    else if (record.TYPE_ID == id12)
                    {
                        relativePath = "VEAM_Thoa thuan cham dut HDLD.docx";

                        dataSet = QueryData.ExecuteStoreToTable(
                            "PKG_PRINT_AGREEMENT_TERMINATE_CONTRACT_2",
                            new { P_ID = id },
                            false);
                    }
                    else
                    {
                        return Ok(new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = CommonMessageCodes.ID_OF_DECISION_DIFFERENT_ID_OF_SYS_OTHER_LIST,
                            StatusCode = EnumStatusCode.StatusCode400
                        });
                    }
                }
                else
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCodes.NOT_FOUND_RECORD_OF_WORKING_PROCESS,
                        StatusCode = EnumStatusCode.StatusCode400
                    });
                }

                var absolutePath = Path.Combine(location, relativePath);

                // create file
                var file = await _wordRespsitory.ExportWordNoImage(dataSet, absolutePath);


                // return result
                return File(file, "application/octet-stream", relativePath);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message,
                    StatusCode = EnumStatusCode.StatusCode400,
                });
            }
        }
    }
}
