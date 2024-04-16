using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;
using Microsoft.IdentityModel.JsonWebTokens;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.GenericUOW;
using API.Main;
using API;
using Microsoft.Extensions.Options;
using API.DTO;
using System.Linq.Dynamic.Core;

namespace ProfileAPI.List
{
    [ApiExplorerSettings(GroupName = "002-PROFILE-JOB")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuJobController : BaseController1
    {

        private readonly ProfileDbContext _profileDbContext;
        private IHttpContextAccessor _accessor;
        private readonly GenericUnitOfWork _uow;
        private IGenericRepository<HU_JOB, HUJobEditDTO> _genericRepository;
        private readonly GenericReducer<HU_JOB, HuJobDTO> _genericReducer;
        private AppSettings _appSettings;
        public HuJobController(IProfileUnitOfWork unitOfWork, IHttpContextAccessor accessor, ProfileDbContext coreDbContext, IOptions<AppSettings> options) : base(unitOfWork)
        {
            _profileDbContext = coreDbContext;
            _accessor = accessor;
            _uow = new GenericUnitOfWork(coreDbContext);
            _genericRepository = _uow.GenericRepository<HU_JOB, HUJobEditDTO>();
            _appSettings = options.Value;
            _genericReducer = new();
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HUJobInputDTO> request)
        {
            try
            {
                var response = await _unitOfWork.HuJobRepository.SinglePhaseQueryList(request);

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
        public async Task<IActionResult> QueryListForOrgOverview(GenericQueryListDTO<HuJobDTO> request)
        {
            try
            {
                var orgIds = request.InOperators?.Single(x => x.Field == "orgId").Values;

                if (orgIds == null) return BadRequest();

                var groups = from p in _profileDbContext.Positions
                             where orgIds.Contains(p.ORG_ID ?? -1)

                             join j in _profileDbContext.HUJobs on p.JOB_ID equals j.ID into pj
                             from pjResult in pj.DefaultIfEmpty()

                             join e in _profileDbContext.Employees on p.ID equals e.POSITION_ID into pe
                             from peResult in pe.DefaultIfEmpty()

                             where pjResult != null && peResult != null

                             group peResult by new { Id = p.JOB_ID, NameVn = pjResult.NAME_VN, NameEn = pjResult.NAME_EN } into g

                             select new HuJobDTO()
                             {
                                 Id = g.Key.Id,
                                 NameVn = g.Key.NameVn,
                                 NameEn = g.Key.NameEn,
                                 EmployeeCount = g.Count(),
                             };
                request.InOperators = null;
                var list = groups.ToList();
                for (var i = 0; i < list.Count(); i++)
                {
                    list[i].Orders = i + 1;
                }
                groups = list.AsQueryable();

                var response = await _genericReducer.SinglePhaseReduce(groups, request);
                
                return Ok(new FormatedResponse() { InnerBody = response });
            }catch(Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            //try
            //{
            //    var response = await _genericRepository.GetById(id);
            //    return Ok(response);
            //}
            //catch (Exception ex)
            //{
            //    return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            //}
            var r = await _unitOfWork.HuJobRepository.GetJobById(id);
            return Ok(new FormatedResponse()
            {
                InnerBody = r.Data
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(HUJobEditDTO model)
        {
            try
            {
                var sid = Request.Sid(_appSettings);

                var validate = _profileDbContext.HUJobs.Where(x => x.CODE.ToLower().Equals(model.Code.ToLower()) && x.ID != model.Id && x.ACTFLG == "A").Count();
                if (validate > 0)
                {
                    return Ok(new FormatedResponse() { MessageCode = "UI_FORM_CONTROL_ERROR_JOB_CODE_DUPLICATED", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode519 });
                }
                var validateName = _profileDbContext.HUJobs.Where(x => x.NAME_VN.ToLower().Equals(model.NameVnNoCode.ToLower()) && x.ID != model.Id && x.ACTFLG == "A").Count();
                if (validateName > 0)
                {
                    return Ok(new FormatedResponse() { MessageCode = "UI_FORM_CONTROL_ERROR_JOB_NAME_DUPLICATED", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode519 });
                }
                model.Actflg = "A";
                model.NameVn = model.NameVnNoCode;
                model.CreatedDate = DateTime.UtcNow;
                model.CreatedBy = sid;
                var response = await _genericRepository.Create(_uow, model, sid ?? "");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> Update(HUJobEditDTO model)
        //{
        //    try
        //    {
        //        //HU_JOB entity = new();
        //        var response = await _genericRepository.Update(_uow, model /*, entity */);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
        //    }
        //}
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] HUJobEditDTO model)
        {
            if (model == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            var itemCheck = _profileDbContext.HUJobs.Where(x => x.ID == model.Id).FirstOrDefault();

            if (itemCheck.ACTFLG == "A")
            {
                return Ok(new FormatedResponse() { MessageCode = "NOT_UPDATE_BECAUSE_ROW_ACTIVE", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode519 });
            }
            //if (string.IsNullOrWhiteSpace(model.NameVn))
            //{
            //    return ResponseResult("NAME_VN_NOT_BLANK");
            //}
            //if (string.IsNullOrWhiteSpace(model.NameEn))
            //{
            //    return ResponseResult("NAME_EN_NOT_BLANK");
            //}
            //if (string.IsNullOrWhiteSpace(model.Code))
            //{
            //    return ResponseResult("CODE_NOT_BLANK");
            //}
            var validatePositionUse = (from p in _profileDbContext.Positions
                                       from j in _profileDbContext.HUJobs.Where(x => x.ID == p.JOB_ID)
                                       where j.ID == model.Id && p.IS_ACTIVE == true
                                       select p).Count();
            if (validatePositionUse > 0)
            {
                return Ok(new FormatedResponse() { MessageCode = "UI_FORM_CONTROL_ERROR_JOB_POSITION_ORG_USE", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode519 });
            }
            var validateUse = (from p in _profileDbContext.Positions
                               from e in _profileDbContext.Employees.Where(x => x.POSITION_ID == p.ID)
                               from j in _profileDbContext.HUJobs.Where(x => x.ID == p.JOB_ID)
                               where j.ID == model.Id && p.IS_ACTIVE == true
                               select e).Count();
            if (validateUse > 0)
            {
                return Ok(new FormatedResponse() { MessageCode = "UI_FORM_CONTROL_ERROR_JOB_EMPLOYEE_USE", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode519 });
            }
            var validate = _profileDbContext.HUJobs.Where(x => x.CODE.ToLower().Equals(model.Code.ToLower()) && x.ID != model.Id && x.ACTFLG == "A").Count();
            if (validate > 0)
            {
                return Ok(new FormatedResponse() { MessageCode = "UI_FORM_CONTROL_ERROR_JOB_CODE_DUPLICATED", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode519 });
            }
            var validateName = _profileDbContext.HUJobs.Where(x => x.NAME_VN.ToLower().Equals(model.NameVnNoCode.ToLower()) && x.ID != model.Id && x.ACTFLG == "A").Count();
            if (validateName > 0)
            {
                return Ok(new FormatedResponse() { MessageCode = "UI_FORM_CONTROL_ERROR_JOB_NAME_DUPLICATED", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode519 });
            }
            var userName = _accessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Typ)?.Value?.Trim();
            model.NameVn = model.NameVnNoCode;
            var r = await _genericRepository.Update(_uow, model, "");
            return Ok(r);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(HUJobEditDTO model)
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
        //----------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            try
            {
                if (model.Ids != null)
                {
                    var checkStatus = (from p in _profileDbContext.HUJobs where model.Ids.Contains(p.ID) && p.ACTFLG == "A" select p).ToList().Count();
                    if (checkStatus > 0)
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE, StatusCode = EnumStatusCode.StatusCode400 });
                    }
                    var validatePositionUse = (from p in _profileDbContext.Positions
                                               from j in _profileDbContext.HUJobs.Where(x => x.ID == p.JOB_ID)
                                               where model.Ids.Contains(j.ID) && p.IS_ACTIVE == true
                                               select p).Count();
                    if (validatePositionUse > 0)
                    {
                        return Ok(new FormatedResponse() { MessageCode = "UI_FORM_CONTROL_ERROR_JOB_POSITION_USE_DELETE", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode519 });
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
        //[HttpPost]
        //public async Task<ActionResult> GetJobs(HUJobInputDTO param)
        //{
        //    var r = await _unitOfWork.HuJobRepository.GetJobs(param);
        //    return Ok(r);
        //}
        //[HttpGet]
        //[Route("{id:int}")]
        //public async Task<ActionResult> GetJob(int Id)
        //{
        //    var r = await _unitOfWork.HuJobRepository.GetJob(Id);
        //    return ResponseResult(r);
        //}



        [HttpPost]
        public async Task<ActionResult> ChangeStatus(HUJobDTO request)
        {
            var r = await _unitOfWork.HuJobRepository.ChangeStatusAsync(request);
            return Ok(new FormatedResponse() { MessageCode = request.ValueToBind == true ? CommonMessageCode.TOGGLE_IS_ACTIVE_SUCCESS : (request.ValueToBind == false ? CommonMessageCode.TOGGLE_IS_INACTIVE_SUCCESS : ""), StatusCode = EnumStatusCode.StatusCode200 });
        }

        //[HttpPost]
        //[Route("delete")]
        //public async Task<ActionResult> Delete([FromBody] List<long> ids)
        //{
        //    var r = await _unitOfWork.HuJobRepository.DeleteAsync(ids);
        //    return ResponseResult(r);
        //}

        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            try
            {
                var r = await _unitOfWork.HuJobRepository.GetList();
                return Ok(new FormatedResponse() { InnerBody = r.Data });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCodeByJobFamily(long id)
        {
            try
            {
                var response = await _unitOfWork.HuJobRepository.GetCodeByJobFamily(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
    }
}
