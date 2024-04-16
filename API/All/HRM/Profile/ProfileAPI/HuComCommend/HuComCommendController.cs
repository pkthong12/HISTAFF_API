using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuCommend
{
    [ApiExplorerSettings(GroupName = "048-PROFILE-HU_COM_COMMEND")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuComCommendController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuComCommendRepository _HuComCommendRepository;
        private IGenericRepository<HU_COMMEND_EMPLOYEE, HuCommendEmployeeDTO> _genericRepositoryChild;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;
        private readonly FullDbContext _fullDbContext;
        private readonly IWebHostEnvironment _env;

        public HuComCommendController(
            FullDbContext fullDbContext,
            FullDbContext dbContext, IFileService fileService,
            IWebHostEnvironment env,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuComCommendRepository = new HuComCommendRepository(dbContext, _uow);
            _appSettings = options.Value;
            _fileService = fileService;
            _env = env;
            _genericRepositoryChild = _uow.GenericRepository<HU_COMMEND_EMPLOYEE,HuCommendEmployeeDTO>();
            _fullDbContext = fullDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuComCommendRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuComCommendRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuComCommendRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuComCommendDTO> request)
        {
            var response = await _HuComCommendRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response});
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuComCommendRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuComCommendRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuComCommendDTO model)
        {
            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();
            bool isEffectDateInvalid = false;
            try
            {
                var sid = Request.Sid(_appSettings);
                if (model.AttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = model.AttachmentBuffer.ClientFileName,
                        ClientFileType = model.AttachmentBuffer.ClientFileType,
                        ClientFileData = model.AttachmentBuffer.ClientFileData
                    }, location, sid);
                    var property = typeof(HuComCommendDTO).GetProperty("Attachment");

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

               
                    var newObjResponse = await _HuComCommendRepository.Create(_uow, model, sid);
                        if(newObjResponse.InnerBody != null)
                        {
                            var newObj = newObjResponse.InnerBody as HU_COM_COMMEND;
                            //if(newObj != null)
                            //{
                            //    if(model.EmployeeIds != null)
                            //    {
                            //        List<HuCommendEmployeeDTO> list = new();
                            //        model.EmployeeIds.ForEach(item =>
                            //        {
                            //                list.Add(new()
                            //                {
                            //                    EmployeeId = item,
                            //                    CommendId = newObj.ID,
                            //                });
                            //            });
                            //        if(isEffectDateInvalid == true)
                            //        {
                            //            return Ok(new FormatedResponse()
                            //            {
                            //                ErrorType = EnumErrorType.CATCHABLE,
                            //                StatusCode = EnumStatusCode.StatusCode400,
                            //                MessageCode = CommonMessageCode.THE_EFFECT_DATE_MUST_BE_GREAT_THAN_THE_CONTRACT
                            //            });
                            //        }
                            //         var addChildren = await _genericRepositoryChild.CreateRange(_uow, list, sid);
                            //    }
                            //    else
                            //    {
                            //        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": PaymentAttachment" });
                            //    }
                            //}
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
        public async Task<IActionResult> CreateRange(List<HuComCommendDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuComCommendRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuComCommendDTO request)
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
                var property = typeof(HuComCommendDTO).GetProperty("Attachment");

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
                var updateResponse = await _HuComCommendRepository.Update(_uow, request, sid, patchMode);
                if (updateResponse.InnerBody != null)
                {
                    var updateHuCommend = updateResponse.InnerBody as HU_COMMEND;
                    if (updateHuCommend != null)
                    {
                        if(request.EmployeeIds != null)
                        {
                            List<HuCommendEmployeeDTO> list = new();
                            request.EmployeeIds.ForEach(item =>
                            {
                                list.Add(new()
                                {
                                    EmployeeId = item,
                                    CommendId = updateHuCommend.ID,
                                   
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
                            var updateChildren = await _genericRepositoryChild.UpdateRange(_uow, list, sid, patchMode);
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
        public async Task<IActionResult> UpdateRange(List<HuComCommendDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuComCommendRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuComCommendDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuComCommendRepository.Delete(_uow, (long)model.Id);
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
                    if(otherList.ID == getCommend.STATUS_PAYMENT_ID)
                    {
                        isCheckStatus = true;
                        return;
                    }
                });
                if(isCheckStatus == true)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.APPROVED_RECORDS_CAN_NOT_BE_DELETED });
                }
                else
                {
                    var response = await _HuComCommendRepository.DeleteIds(_uow, model.Ids);
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
            var response = await _HuComCommendRepository.DeleteIds(_uow, model.Ids);
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

            var result = await(from c in entity
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
            var response = await _HuComCommendRepository.ApproveCommend(model.Ids);
            return Ok(response);
        }

    }
}

