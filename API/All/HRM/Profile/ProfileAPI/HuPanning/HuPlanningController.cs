using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.Extension;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuPlanning
{
    [ApiExplorerSettings(GroupName = "149-PROFILE-HU_PLANNING")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuPlanningController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuPlanningRepository _HuPlanningRepository;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;
        private readonly FullDbContext _fullDbContext;
        private readonly IGenericRepository<HU_PLANNING_EMP, HuPlanningEmpDTO> _genericRepositoryChild;

        public HuPlanningController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
            IFileService fileService,
            IWebHostEnvironment env,
            FullDbContext fullDbContext)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuPlanningRepository = new HuPlanningRepository(dbContext, _uow);
            _appSettings = options.Value;
            _fileService = fileService;
            _env = env;
            _fullDbContext = fullDbContext;
            _genericRepositoryChild = _uow.GenericRepository<HU_PLANNING_EMP, HuPlanningEmpDTO>();
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuPlanningRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuPlanningRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuPlanningRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuPlanningDTO> request)
        {
            var response = await _HuPlanningRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuPlanningRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuPlanningRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuPlanningDTO model)
        {
            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();
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
                    var property = typeof(HuPlanningDTO).GetProperty("Attachment");

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


                var newObjResponse = await _HuPlanningRepository.Create(_uow, model, sid);
                if (newObjResponse != null)
                {
                    var newObj = newObjResponse.InnerBody as HU_PLANNING;
                    if (newObj != null)
                    {
                        if (model.EmployeeList != null && model.EmployeeList.Count != 0)
                        {
                            if (model.EmployeeIds.Count != model.EmployeeList!.Count)
                            {
                                return Ok(new FormatedResponse()
                                {
                                    ErrorType = EnumErrorType.CATCHABLE,
                                    StatusCode = EnumStatusCode.StatusCode400,
                                    MessageCode = "LACK_OF_PLANNING_TITLES_AND_PLANNING_TYPES"
                                });
                            }
                            List<HuPlanningEmpDTO> list = new();
                            model.EmployeeList.ForEach(item =>
                            {
                                list.Add(new()
                                {
                                    PlanningId = newObj.ID,
                                    EmployeeId = (long)item["id"],
                                    PlanningTitleId = (long)item["planningTitleId"],
                                    PlanningTypeId = (long)item["planningTypeId"],
                                    EvaluateId = item.HasProperty("evaluateId") == false ? null : (long)item["evaluateId"],
                                });
                            });
                            var addChildren = await _genericRepositoryChild.CreateRange(_uow, list, sid);
                        }
                        else
                        {
                            if (model.EmployeeIds != null && model.EmployeeIds.Count != 0)
                            {
                                List<HuPlanningEmpDTO> list = new();
                                model.EmployeeIds.ForEach(item =>
                                {
                                    list.Add(new()
                                    {
                                        PlanningId = newObj.ID,
                                        EmployeeId = item,
                                        PlanningTitleId = null,
                                        PlanningTypeId = null,
                                        EvaluateId = null
                                    });
                                });
                                var addChildren = await _genericRepositoryChild.CreateRange(_uow, list, sid);
                            }
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
            //var sid = Request.Sid(_appSettings);
            //if (sid == null) return Unauthorized();
            //var response = await _HuPlanningRepository.Create(_uow, model, sid);
            //return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuPlanningDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuPlanningRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuPlanningDTO model)
        {
            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();
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
                    var property = typeof(HuPlanningDTO).GetProperty("Attachment");

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

                var updateObjResponse = await _HuPlanningRepository.Update(_uow, model, sid);
                if (updateObjResponse != null)
                {
                    var newObj = updateObjResponse.InnerBody as HU_PLANNING;
                    if (newObj != null)
                    {
                        if (model.EmployeeIds != null)
                        {
                            if (model.EmployeeIds.Count != model.EmployeeList!.Count)
                            {
                                return Ok(new FormatedResponse()
                                {
                                    ErrorType = EnumErrorType.CATCHABLE,
                                    StatusCode = EnumStatusCode.StatusCode400,
                                    MessageCode = "LACK_OF_PLANNING_TITLES_AND_PLANNING_TYPES"
                                });
                            }
                            _fullDbContext.HuPlanningEmps.RemoveRange(_fullDbContext.HuPlanningEmps.Where(x => x.PLANNING_ID == model.Id));

                            if (model.EmployeeList != null && model.EmployeeList.Count != 0)
                            {
                                List<HuPlanningEmpDTO> list = new();
                                model.EmployeeList.ForEach(item =>
                                {
                                    list.Add(new()
                                    {
                                        PlanningId = newObj.ID,
                                        EmployeeId = (long)item["id"],
                                        PlanningTitleId = (long)item["planningTitleId"],
                                        PlanningTypeId = (long)item["planningTypeId"],
                                        EvaluateId = item.HasProperty("evaluateId") == false ? null : (long)item["evaluateId"],
                                        CreatedBy = sid,
                                        CreatedDate = DateTime.Now,
                                        UpdatedBy = sid,
                                        UpdatedDate = DateTime.Now,
                                    });
                                });
                                var addChildren = await _genericRepositoryChild.CreateRange(_uow, list, sid);
                            }
                            else
                            {
                                if (model.EmployeeIds != null && model.EmployeeIds.Count != 0)
                                {
                                    List<HuPlanningEmpDTO> list = new();
                                    model.EmployeeIds.ForEach(item =>
                                    {
                                        list.Add(new()
                                        {
                                            PlanningId = newObj.ID,
                                            EmployeeId = item,
                                            PlanningTitleId = null,
                                            PlanningTypeId = null,
                                            EvaluateId = null,
                                            CreatedBy = sid,
                                            CreatedDate = DateTime.Now,
                                            UpdatedBy = sid,
                                            UpdatedDate = DateTime.Now,
                                        });
                                    });
                                    var addChildren = await _genericRepositoryChild.CreateRange(_uow, list, sid);
                                }
                            }
                        }
                    }
                }
                await _fullDbContext.SaveChangesAsync();
                _uow.Commit();
                return Ok(updateObjResponse);
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

            //var sid = Request.Sid(_appSettings);
            //if (sid == null) return Unauthorized();
            //var response = await _HuPlanningRepository.Update(_uow, model, sid);
            //return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuPlanningDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuPlanningRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuPlanningDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuPlanningRepository.Delete(_uow, (long)model.Id);
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
            var response = await _HuPlanningRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuPlanningRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAppLevel()
        {
            try
            {
                var response = await (from s in _uow.Context.Set<HU_JOB>().AsNoTracking()
                                      where s.ACTFLG == "A"
                                      select new
                                      {
                                          Id = s.ID,
                                          Name = s.NAME_VN
                                      }
                                      ).ToListAsync();
                return Ok(new FormatedResponse() { InnerBody = response });
            }
            catch (Exception ex)
            {

                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCertificateByEmp(long id)
        {
            try
            {
                var response = await _HuPlanningRepository.GetCertificateByEmp(id);
                return Ok(response);
            }catch (Exception ex) 
            { 
                return Ok(new FormatedResponse() {  ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

    }
}

