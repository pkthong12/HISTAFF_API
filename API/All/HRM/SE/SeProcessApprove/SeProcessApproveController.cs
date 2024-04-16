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
using ProfileDAL.ViewModels;

namespace API.Controllers.SeProcessApprove
{
    [ApiExplorerSettings(GroupName = "158-SYSTEM-SE_PROCESS_APPROVE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class SeProcessApproveController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ISeProcessApproveRepository _SeProcessApproveRepository;
        private IGenericRepository<SE_PROCESS_APPROVE_POS, SeProcessApprovePosDTO> _genericRepositoryChild;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _fullDbContext;

        public SeProcessApproveController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _genericRepositoryChild = _uow.GenericRepository<SE_PROCESS_APPROVE_POS, SeProcessApprovePosDTO>();
            _SeProcessApproveRepository = new SeProcessApproveRepository(dbContext, _uow);
            _appSettings = options.Value;
            _fullDbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _SeProcessApproveRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _SeProcessApproveRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _SeProcessApproveRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<SeProcessApproveDTO> request)
        {
            var response = await _SeProcessApproveRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _SeProcessApproveRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _SeProcessApproveRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SeProcessApproveDTO model)
        {
            _uow.CreateTransaction();
            try
            {
                var query = _fullDbContext.SeProcessApproves.AsNoTracking().Where(x => x.PROCESS_ID == model.ProcessId && x.LEVEL_ORDER_ID == model.LevelOrderId).Any();
                if (query)
                {
                    return Ok(new FormatedResponse()
                    {
                        MessageCode = CommonMessageCode.THE_PROCESS_EXISTS,
                        StatusCode = EnumStatusCode.StatusCode500
                    });
                }
                if (model.ProcessId == null)
                {
                    return Ok(new FormatedResponse()
                    {
                        MessageCode = CommonMessageCode.THE_PROCESS_NOT_NULL,
                        StatusCode = EnumStatusCode.StatusCode500
                    });
                }
                var sid = Request.Sid(_appSettings);
                if (model.CheckList != null)
                {
                    model.ApprovalPosition = model.CheckList.Contains(1);
                    model.SameApprover = model.CheckList.Contains(2);
                }
                else
                {
                    model.ApprovalPosition = false;
                    model.SameApprover = false;
                }
                if (model.ListCheck != null)
                {
                    switch (model.ListCheck)
                    {
                        case 1:
                            model.DirectManager = true;
                            model.ManagerAffiliatedDepartments = false;
                            model.ManagerSuperiorDepartments = false;
                            model.IsDirectMngOfDirectMng = false;
                            break;
                        case 2:
                            model.DirectManager = false;
                            model.ManagerAffiliatedDepartments = true;
                            model.ManagerSuperiorDepartments = false;
                            model.IsDirectMngOfDirectMng = false;
                            break;
                        case 3:
                            model.DirectManager = false;
                            model.ManagerAffiliatedDepartments = false;
                            model.ManagerSuperiorDepartments = true; 
                            model.IsDirectMngOfDirectMng = false;
                            break;
                        case 4:
                            model.DirectManager = false;
                            model.ManagerAffiliatedDepartments = false;
                            model.ManagerSuperiorDepartments = false;
                            model.IsDirectMngOfDirectMng = true;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    model.DirectManager = false;
                    model.ManagerAffiliatedDepartments = false;
                    model.ManagerSuperiorDepartments = false;
                    model.IsDirectMngOfDirectMng = false;
                }
                var newObjResponse = await _SeProcessApproveRepository.Create(_uow, model, sid);
                if (newObjResponse != null)
                {
                    var newObj = newObjResponse.InnerBody as SE_PROCESS_APPROVE;
                    var pos = newObjResponse.InnerBody as SE_PROCESS_APPROVE_POS;
                    if (newObj != null)
                    {
                        if (model.PosIds != null && model.PosIds.Count != 0)
                        {
                            List<SeProcessApprovePosDTO> list = new();
                            model.PosIds.ForEach(item =>
                            {
                                list.Add(new()
                                {
                                    PosId = item,
                                    ProcessApproveId = newObj.ID,
                                    IsMngAffiliatedDepartments = newObj.MANAGER_AFFILIATED_DEPARTMENTS,
                                    IsMngSuperiorDepartments = newObj.MANAGER_SUPERIOR_DEPARTMENTS,
                                    IsDirectManager = newObj.DIRECT_MANAGER,
                                    IsDirectMngOfDirectMng = newObj.IS_DIRECT_MNG_OF_DIRECT_MNG,
                                });
                            });
                            var addListPos = await _genericRepositoryChild.CreateRange(_uow, list, sid);
                        }
                        else
                        {
                            List<SeProcessApprovePosDTO> list = new();
                            list.Add(new()
                            {
                                PosId = null,
                                ProcessApproveId = newObj.ID,
                                IsMngAffiliatedDepartments = newObj.MANAGER_AFFILIATED_DEPARTMENTS,
                                IsMngSuperiorDepartments = newObj.MANAGER_SUPERIOR_DEPARTMENTS,
                                IsDirectManager = newObj.DIRECT_MANAGER,
                                IsDirectMngOfDirectMng = newObj.IS_DIRECT_MNG_OF_DIRECT_MNG,
                            });
                            var addListPos = await _genericRepositoryChild.CreateRange(_uow, list, sid);
                        }
                    }
                }
                await _fullDbContext.SaveChangesAsync();
                _uow.Commit();
                return Ok(newObjResponse);
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<SeProcessApproveDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SeProcessApproveRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SeProcessApproveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            bool patchMode = true;
            try
            {
                if (model.CheckList != null)
                {
                    model.ApprovalPosition = model.CheckList.Contains(1);
                    model.SameApprover = model.CheckList.Contains(2);
                }
                else
                {
                    model.ApprovalPosition = false;
                    model.SameApprover = false;
                }
                if (model.ListCheck != null)
                {
                    switch (model.ListCheck)
                    {
                        case 1:
                            model.DirectManager = true;
                            model.ManagerAffiliatedDepartments = false;
                            model.ManagerSuperiorDepartments = false;
                            model.IsDirectMngOfDirectMng = false;
                            break;
                        case 2:
                            model.DirectManager = false;
                            model.ManagerAffiliatedDepartments = true;
                            model.ManagerSuperiorDepartments = false;
                            model.IsDirectMngOfDirectMng = false;
                            break;
                        case 3:
                            model.DirectManager = false;
                            model.ManagerAffiliatedDepartments = false;
                            model.ManagerSuperiorDepartments = true;
                            model.IsDirectMngOfDirectMng = false;
                            break;
                        case 4:
                            model.DirectManager = false;
                            model.ManagerAffiliatedDepartments = false;
                            model.ManagerSuperiorDepartments = false;
                            model.IsDirectMngOfDirectMng = true;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    model.DirectManager = false;
                    model.ManagerAffiliatedDepartments = false;
                    model.ManagerSuperiorDepartments = false;
                    model.IsDirectMngOfDirectMng = false;
                }
                var updateObjResponse = await _SeProcessApproveRepository.Update(_uow, model, sid, patchMode);
                if (model.PosIds != null && model.PosIds.Count != 0)
                {
                    _fullDbContext.SeProcessApprovePos.RemoveRange(_fullDbContext.SeProcessApprovePos.Where(x => x.PROCESS_APPROVE_ID == model.Id));
                    foreach (long item in model.PosIds)
                    {
                        SE_PROCESS_APPROVE_POS objEmpData = new()
                        {
                            POS_ID = item,
                            PROCESS_APPROVE_ID = model.Id,
                            IS_DIRECT_MANAGER = model.DirectManager,
                            IS_MNG_AFFILIATED_DEPARTMENTS = model.ManagerAffiliatedDepartments,
                            IS_DIRECT_MNG_OF_DIRECT_MNG = model.IsDirectMngOfDirectMng,
                            IS_MNG_SUPERIOR_DEPARTMENTS = model.ManagerSuperiorDepartments,
                        };
                        _fullDbContext.SeProcessApprovePos.AddAsync(objEmpData);
                    }
                }
                else
                {
                    _fullDbContext.SeProcessApprovePos.RemoveRange(_fullDbContext.SeProcessApprovePos.Where(x=>x.PROCESS_APPROVE_ID == model.Id));
                }
                await _fullDbContext.SaveChangesAsync();
                _uow.Commit();

                return Ok(updateObjResponse);
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<SeProcessApproveDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SeProcessApproveRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SeProcessApproveDTO model)
        {
            if (model.Id != null)
            {
                var response = await _SeProcessApproveRepository.Delete(_uow, (long)model.Id);
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
            var response = await _SeProcessApproveRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _SeProcessApproveRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetLevelOrder()
        {
            var response = await _SeProcessApproveRepository.GetLevelOrder();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetListProcess()
        {
            var response = await _SeProcessApproveRepository.GetListProcess();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteByIds(IdsRequest model)
        {
            try
            {
                if (model.Ids != null)
                {
                    foreach (var item in model.Ids)
                    {
                        var x = _fullDbContext.SeProcessApprovePos.AsNoTracking().Where(x => x.PROCESS_APPROVE_ID == item).ToList();
                        var p = _fullDbContext.SeProcessApproves.AsNoTracking().Where(p => p.ID == item).ToList();
                        _fullDbContext.SeProcessApproves.RemoveRange(p);
                        _fullDbContext.SeProcessApprovePos.RemoveRange(x);
                    }
                    _fullDbContext.SaveChanges();
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.DELETE_SUCCESS,
                        StatusCode = EnumStatusCode.StatusCode200,
                    });
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
        public async Task<IActionResult> GetListByProcess(long processId)
        {
            var response = await _SeProcessApproveRepository.GetListByProcess(processId);
            return Ok(response);
        }
    }
}

