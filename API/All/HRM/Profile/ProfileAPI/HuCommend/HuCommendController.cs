using API.All.DbContexts;
using API.DTO;
using API.Main;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuCommend
{
    [ApiExplorerSettings(GroupName = "048-PROFILE-HU_COMMEND")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuCommendController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuCommendRepository _HuCommendRepository;
        private IGenericRepository<HU_COMMEND_EMPLOYEE, HuCommendEmployeeDTO> _genericRepositoryChild;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;
        private readonly FullDbContext _fullDbContext;
        private readonly IWebHostEnvironment _env;

        public HuCommendController(
            FullDbContext fullDbContext,
            FullDbContext dbContext, IFileService fileService,
            IWebHostEnvironment env,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuCommendRepository = new HuCommendRepository(dbContext, _uow);
            _appSettings = options.Value;
            _fileService = fileService;
            _env = env;
            _genericRepositoryChild = _uow.GenericRepository<HU_COMMEND_EMPLOYEE, HuCommendEmployeeDTO>();
            _fullDbContext = fullDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuCommendRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuCommendRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuCommendRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuCommendDTO> request)
        {
            var response = await _HuCommendRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuCommendRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuCommendRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuCommendDTO model)
        {
            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();
            bool isEffectDateInvalid = false;
            try
            {
                var sid = Request.Sid(_appSettings);

                // chuyển List thành string
                if (model.CheckListRewardLevel != null)
                {
                    string list_reward_level_id_STR = string.Join(", ", model.CheckListRewardLevel);
                    model.ListRewardLevelId = list_reward_level_id_STR;
                }
                else
                {
                    model.ListRewardLevelId = "";
                }

                if (model.AttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = model.AttachmentBuffer.ClientFileName,
                        ClientFileType = model.AttachmentBuffer.ClientFileType,
                        ClientFileData = model.AttachmentBuffer.ClientFileData
                    }, location, sid);
                    var property = typeof(HuCommendDTO).GetProperty("Attachment");

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

                if (model.PaymentAttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = model.PaymentAttachmentBuffer.ClientFileName,
                        ClientFileType = model.PaymentAttachmentBuffer.ClientFileType,
                        ClientFileData = model.PaymentAttachmentBuffer.ClientFileData
                    }, location, sid);
                    var property = typeof(HuCommendDTO).GetProperty("PaymentAttachment");

                    if (property != null)
                    {
                        property?.SetValue(model, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": PaymentAttachment" });
                    }
                }

                //var id = _uow.Context.Set<HU_COMMEND>().AsNoTracking().AsQueryable().Max(p => p.ID);

                //if(model.EffectDate != null
                //    && model.No != null
                //    && model.SignDate != null
                //    && model.CommendObjId != null
                //    //&& model.Money != null
                //    && model.SignPaymentDate != null
                //    && model.PaymentNo != null
                //    && model.StatusPaymentId != null)
                //{

                if (model.EmployeeIds != null)
                {
                    foreach (var item in model.EmployeeIds)
                    {
                        var checkCon = await _fullDbContext.HuContracts.AsNoTracking().Where(x => x.EMPLOYEE_ID == item && x.STATUS_ID == OtherConfig.STATUS_APPROVE).AnyAsync();
                        if (!checkCon)
                        {
                            return Ok(new FormatedResponse() { 
                                MessageCode = "EMPLOYEE_DO_NOT_HAVE_A_CONTRACT", 
                                ErrorType = EnumErrorType.CATCHABLE, 
                                StatusCode = EnumStatusCode.StatusCode400 
                            });
                        }
                        var ckDate = await _fullDbContext.HuContracts.AsNoTracking().Where(x => x.EMPLOYEE_ID == item).OrderByDescending(c => c.START_DATE).FirstOrDefaultAsync();
                        if (ckDate != null)
                        {
                            if (model.SignDate!.Value.Date < ckDate.START_DATE!.Value.Date)
                            {
                                return Ok(new FormatedResponse()
                                {
                                    MessageCode = "EFFECTIVE_DATE_MUST_BE_GREATER_THAN_CONTRACT_EFFECTIVE_DATE",
                                    ErrorType = EnumErrorType.CATCHABLE,
                                    StatusCode = EnumStatusCode.StatusCode400
                                });
                            }
                        }
                    }
                }

                var newObjResponse = await _HuCommendRepository.Create(_uow, model, sid);
                if (newObjResponse != null)
                {
                    var newObj = newObjResponse.InnerBody as HU_COMMEND;
                    if (newObj != null)
                    {
                        if (model.EmployeeIds != null)
                        {
                            List<HuCommendEmployeeDTO> list = new();
                            model.EmployeeIds.ForEach(item =>
                            {
                                var checkEffectDate = from a in _fullDbContext.HuContracts.AsNoTracking().Where(x => x.EMPLOYEE_ID == item)
                                                      from o in _fullDbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == a.STATUS_ID)
                                                      where a.START_DATE > model.EffectDate || o.CODE != "DD"
                                                      select new
                                                      {
                                                          Id = a.ID,
                                                          StartDate = a.START_DATE

                                                      };
                                if (checkEffectDate.Any())
                                {
                                    isEffectDateInvalid = true;
                                    return;
                                }
                                list.Add(new()
                                {
                                    EmployeeId = item,
                                    CommendId = newObj.ID,
                                    StatusId = model.StatusPaymentId
                                });
                            });
                            if (isEffectDateInvalid == true)
                            {
                                return Ok(new FormatedResponse()
                                {
                                    ErrorType = EnumErrorType.CATCHABLE,
                                    StatusCode = EnumStatusCode.StatusCode400,
                                    MessageCode = CommonMessageCode.THE_EFFECT_DATE_MUST_BE_GREAT_THAN_THE_CONTRACT
                                });
                            }
                            var addChildren = await _genericRepositoryChild.CreateRange(_uow, list, sid);
                        }
                        else
                        {
                            return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": PaymentAttachment" });
                        }
                    }
                }

                await _fullDbContext.SaveChangesAsync();

                _uow.Commit();

                return Ok(new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.CREATE_SUCCESS,
                    InnerBody = newObjResponse.InnerBody,
                    StatusCode = EnumStatusCode.StatusCode200
                });
                //}

                //}
                //else {
                //    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DTO_MODEL_IS_INVALID});
                //}
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
        public async Task<IActionResult> CreateRange(List<HuCommendDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuCommendRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuCommendDTO request)
        {
            //if (request.StatusPaymentId != null && request.StatusPaymentId == OtherConfig.STATUS_APPROVE)
            //    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_CANNOT_EDIT_APPROVED_RECORD" });
            _uow.CreateTransaction();
            bool patchMode = true;
            List<UploadFileResponse> uploadFiles = new();
            bool isEffectDateInvalid = false;
            try
            {
                var sid = Request.Sid(_appSettings);
                if (sid == null) return Unauthorized();

                // chuyển List thành string
                if (request.CheckListRewardLevel != null)
                {
                    string list_reward_level_id_STR = string.Join(", ", request.CheckListRewardLevel);
                    request.ListRewardLevelId = list_reward_level_id_STR;
                }
                else
                {
                    request.ListRewardLevelId = "";
                }

                if (request.AttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = request.AttachmentBuffer.ClientFileName,
                        ClientFileType = request.AttachmentBuffer.ClientFileType,
                        ClientFileData = request.AttachmentBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(HuCommendDTO).GetProperty("Attachment");

                    if (property != null)
                    {
                        property?.SetValue(request, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": Attachment" });
                    }

                }
                if (request.PaymentAttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = request.PaymentAttachmentBuffer.ClientFileName,
                        ClientFileType = request.PaymentAttachmentBuffer.ClientFileType,
                        ClientFileData = request.PaymentAttachmentBuffer.ClientFileData
                    }, location, sid);
                    var property = typeof(HuCommendDTO).GetProperty("PaymentAttachment");

                    if (property != null)
                    {
                        property?.SetValue(request, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": PaymentAttachment" });
                    }
                }
                if (request.EmployeeIds != null)
                {
                    foreach (var item in request.EmployeeIds)
                    {
                        var checkCon = await _fullDbContext.HuContracts.AsNoTracking().Where(x => x.EMPLOYEE_ID == item && x.STATUS_ID != OtherConfig.STATUS_APPROVE).AnyAsync();
                        if (checkCon)
                        {
                            return Ok(new FormatedResponse()
                            {
                                MessageCode = "EMPLOYEE_DO_NOT_HAVE_A_CONTRACT",
                                ErrorType = EnumErrorType.CATCHABLE,
                                StatusCode = EnumStatusCode.StatusCode400
                            });
                        }
                        var ckDate = await _fullDbContext.HuContracts.AsNoTracking().Where(x => x.EMPLOYEE_ID == item).OrderByDescending(c => c.START_DATE).FirstOrDefaultAsync();
                        if (ckDate != null)
                        {
                            if (request.EffectDate!.Value.Date < ckDate.START_DATE!.Value.Date)
                            {
                                return Ok(new FormatedResponse()
                                {
                                    MessageCode = "EFFECTIVE_DATE_MUST_BE_GREATER_THAN_CONTRACT_EFFECTIVE_DATE",
                                    ErrorType = EnumErrorType.CATCHABLE,
                                    StatusCode = EnumStatusCode.StatusCode400
                                });
                            }
                        }
                    }
                }

                var updateResponse = await _HuCommendRepository.Update(_uow, request, sid, patchMode);
                if (updateResponse != null)
                {
                    var updateHuCommend = updateResponse.InnerBody as HU_COMMEND;
                    if (updateHuCommend != null)
                    {
                        if (request.EmployeeIds != null)
                        {
                            _fullDbContext.HuCommendEmployees.RemoveRange(_fullDbContext.HuCommendEmployees.Where(x => x.COMMEND_ID == request.Id));
                            foreach (long i in request.EmployeeIds)
                            {
                                HU_COMMEND_EMPLOYEE objEmpData = new()
                                {
                                    EMPLOYEE_ID = i,
                                    COMMEND_ID = request.Id,
                                    CREATED_BY = sid,
                                    CREATED_DATE = DateTime.Now,
                                    UPDATED_BY = sid,
                                    UPDATED_DATE = DateTime.Now,
                                    STATUS_ID = request.StatusPaymentId
                                };
                                await _fullDbContext.HuCommendEmployees.AddAsync(objEmpData);
                            }
                            //List<HuCommendEmployeeDTO> list = new();
                            //request.EmployeeIds.ForEach(item =>
                            //{
                            //    var checkEffectDate = from a in _fullDbContext.HuContracts.AsNoTracking().Where(x => x.EMPLOYEE_ID == item)
                            //                          from o in _fullDbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == a.STATUS_ID).DefaultIfEmpty()
                            //                          where a.START_DATE > request.EffectDate || o.CODE != "DD"
                            //                          select new
                            //                          {
                            //                              Id = a.ID,
                            //                              StartDate = a.START_DATE
                            //                          };
                            //    if (checkEffectDate.Any())
                            //    {
                            //        isEffectDateInvalid = true;
                            //        return;
                            //    }
                            //    list.Add(new()
                            //    {
                            //        EmployeeId = item,
                            //        CommendId = updateHuCommend.ID,
                            //        StatusId = request.StatusPaymentId
                            //    });
                            //});
                            //if (isEffectDateInvalid == true)
                            //{
                            //    return Ok(new FormatedResponse()
                            //    {
                            //        ErrorType = EnumErrorType.CATCHABLE,
                            //        StatusCode = EnumStatusCode.StatusCode400,
                            //        MessageCode = CommonMessageCode.THE_EFFECT_DATE_MUST_BE_GREAT_THAN_THE_CONTRACT
                            //    });
                            //}
                            //var updateChildren = await _genericRepositoryChild.UpdateRange(_uow, list, sid, patchMode);
                        }
                    }
                }
                await _fullDbContext.SaveChangesAsync();
                _uow.Commit();

                return Ok(updateResponse);
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
        public async Task<IActionResult> UpdateRange(List<HuCommendDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuCommendRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuCommendDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuCommendRepository.Delete(_uow, (long)model.Id);
                return Ok(response);
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            try
            {
                bool isCheckStatus = false;
                var otherList = _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "DD").FirstOrDefault();
                model.Ids.ForEach(item =>
                {
                    var getCommend = _uow.Context.Set<HU_COMMEND>().Where(x => x.ID == item).FirstOrDefault();
                    if (otherList!.ID == getCommend!.STATUS_PAYMENT_ID)
                    {
                        isCheckStatus = true;
                        return;
                    }
                });
                if (isCheckStatus == true)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.APPROVED_RECORDS_CAN_NOT_BE_DELETED });
                }
                else
                {
                    var response = await _HuCommendRepository.DeleteIds(_uow, model.Ids);
                    return Ok(response);

                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuCommendRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetListCommendByEmployee(long employeeId)
        {
            var entity = _uow.Context.Set<HU_COMMEND>().AsNoTracking();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
            var commendEmployee = _uow.Context.Set<HU_COMMEND_EMPLOYEE>().AsNoTracking().AsQueryable();
            var organization = _uow.Context.Set<HU_ORG_LEVEL>().AsNoTracking().AsQueryable();
            var otherList = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var orgLevel = _uow.Context.Set<HU_ORG_LEVEL>().AsNoTracking().AsQueryable();
            var list_title_commend = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();

            var result = await (from c in entity
                                from ce in commendEmployee.Where(x => x.COMMEND_ID == c.ID).DefaultIfEmpty()
                                from r in otherList.Where(x => x.ID == c.REWARD_ID).DefaultIfEmpty()
                                from f in otherList.Where(x => x.ID == c.STATUS_PAYMENT_ID).DefaultIfEmpty()
                                from e in employee.Where(x => x.ID == ce.EMPLOYEE_ID).DefaultIfEmpty()
                                from o in organization.Where(x => x.ID == c.ORG_LEVEL_ID).DefaultIfEmpty()
                                from rLevel in orgLevel.Where(x => x.ID == c.REWARD_LEVEL_ID).DefaultIfEmpty()
                                from reference_1 in list_title_commend.Where(x => x.ID == c.AWARD_TITLE_ID).DefaultIfEmpty()
                                where e.ID == employeeId && f.CODE == "DD"
                                select new HuCommendDTO()
                                {
                                    Id = c.ID,
                                    EmployeeId = employeeId,
                                    No = c.NO,
                                    SignDate = c.SIGN_DATE,
                                    CommendType = c.COMMEND_TYPE,
                                    Reason = c.REASON,
                                    Money = c.MONEY,
                                    EffectDate = c.EFFECT_DATE,
                                    RewardName = r.NAME,
                                    OrgLevelId = c.ORG_LEVEL_ID,
                                    OrgLevelName = o.NAME,
                                    Year = c.YEAR,
                                    PaymentNo = c.PAYMENT_NO,
                                    SignPaymentDate = c.SIGN_PAYMENT_DATE,
                                    RewardLevelName = rLevel.NAME,
                                    Note = c.NOTE,
                                    StatusId = c.STATUS_ID,
                                    StatusPaymentName = f.NAME,
                                    PaymentContent = c.PAYMENT_CONTENT,
                                    AwardTitleName = reference_1.NAME
                                }).ToListAsync();
            return Ok(new FormatedResponse() { InnerBody = result });
        }
        [HttpGet]
        public async Task<IActionResult> GetStatusList()
        {
            var result = await (from o in _fullDbContext.SysOtherLists.AsNoTracking().AsQueryable()
                                from t in _fullDbContext.SysOtherListTypes.AsNoTracking().Where(x => x.ID == o.TYPE_ID)
                                where t.CODE == "STATUS"
                                select new
                                {
                                    Id = o.ID,
                                    Name = o.NAME,
                                }).ToListAsync();
            return Ok(new FormatedResponse() { InnerBody = result });
        }

        [HttpPost]
        public async Task<IActionResult> ApproveCommend(GenericToggleIsActiveDTO model)
        {
            var response = await _HuCommendRepository.ApproveCommend(model.Ids);
            return Ok(response);
        }

    }
}

