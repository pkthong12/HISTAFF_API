using API;
using API.All.DbContexts;
using API.DTO;
using API.Entities;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-WELFARE")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuWelfareController : BaseController1
    {
        private IHttpContextAccessor _accessor;
        private readonly GenericUnitOfWork _uow;
        private readonly ProfileDbContext _profileDbContext;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_WELFARE, WelfareInputDTO> _genericRepository;
        private AppSettings _appSettings;
        public HuWelfareController(IProfileUnitOfWork unitOfWork, IHttpContextAccessor accessor, FullDbContext context, ProfileDbContext coreDbContext, IOptions<AppSettings> options) : base(unitOfWork)
        {
            _accessor = accessor;
            _uow = new GenericUnitOfWork(coreDbContext);
            _dbContext = context;
            _genericRepository = _uow.GenericRepository<HU_WELFARE, WelfareInputDTO>();
            _appSettings = options.Value;
            _profileDbContext = coreDbContext;
        }
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<WelfareDTO> request)
        {
            try
            {
                /* HERE WE CAN EITHER CHOOSE ONE OF THE 2 APPROACHS: TwoPhaseQueryList or SinglePhaseQueryList */
                /* WE WILL KNOW WHAT APPROACH IS FASTER IN THE FUTURE */

                var response = await _unitOfWork.BenerfitRepository.TwoPhaseQueryList(request);

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
        public async Task<ActionResult> GetAll(WelfareDTO param)
        {
            var r = await _unitOfWork.BenerfitRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetById(long id)
        {
            var r = await _unitOfWork.BenerfitRepository.GetById(id);
            return Ok(new FormatedResponse()
            {
                InnerBody = r.Data
            });
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody]WelfareInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }

            if (string.IsNullOrWhiteSpace(param.Code))
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                return ResponseResult("NAME_NOT_BLANK");
            }

            var r = await _unitOfWork.BenerfitRepository.CreateAsync(param);
            return Ok(new FormatedResponse()
            {
                InnerBody = r.Data,
                MessageCode = CommonMessageCode.CREATE_SUCCESS,
            });
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody]WelfareInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            if (param.Id == null)
            {
                return ResponseResult("ID_NOT_BLANK");
            }
            if (string.IsNullOrWhiteSpace(param.Code))
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                return ResponseResult("NAME_NOT_BLANK");
            }

            var r = await _unitOfWork.BenerfitRepository.UpdateAsync(param);
            return Ok(new FormatedResponse()
            {
                InnerBody = r.Data,
                MessageCode = CommonMessageCode.UPDATE_SUCCESS
            });
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatus(WelfareInputDTO request)
        {
            var r = await _unitOfWork.BenerfitRepository.ChangeStatusAsync(request);
            return Ok(new FormatedResponse() { MessageCode = request.ValueToBind == true ? CommonMessageCode.TOGGLE_IS_ACTIVE_SUCCESS : (request.ValueToBind == false ? CommonMessageCode.TOGGLE_IS_INACTIVE_SUCCESS : ""), StatusCode = EnumStatusCode.StatusCode200 });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(WelfareInputDTO model)
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

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            try
            {

                //check ban ghi phat sinh tai cac man #
                foreach (var item in model.Ids)
                {
                    var check1 = _dbContext.HuWelfareAutos.Where(x => x.WELFARE_ID == item).AsNoTracking().Count();
                    var check2 = _dbContext.HuWelfareMngs.Where(x => x.WELFARE_ID == item).AsNoTracking().Count();
                    if (check1 > 0)
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "DO_NOT_DELETE_DATA_RECORD_WELFARE_AUTO", StatusCode = EnumStatusCode.StatusCode400 });
                    }
                    else if (check2 > 0)
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "DO_NOT_DELETE_DATA_RECORD_WELFARE_MNG", StatusCode = EnumStatusCode.StatusCode400 });
                    }
                }
                if (model.Ids != null)
                {
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
        public async Task<ActionResult> GetList()
        {
            try
            {
                var r = await _unitOfWork.BenerfitRepository.GetList();
                return Ok(new FormatedResponse()
                {
                    InnerBody = r.Data
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetListAuto()
        {
            try
            {
                var r = await _unitOfWork.BenerfitRepository.GetListAuto();
                return Ok(new FormatedResponse()
                {
                    InnerBody = r.Data
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCode()
        {
            decimal num;
            var queryCode = (from x in _profileDbContext.Welfares where x.CODE.Length == 5 select x.CODE).ToList();
            var existingCode = (from p in queryCode where Decimal.TryParse(p.Substring(2), out num) orderby p descending select p).ToList();
            string newcode = StringCodeGenerator.CreateNewCode("PL", 3, existingCode);
            return Ok(new FormatedResponse() { InnerBody = newcode});
        }

        [HttpPost]

        public async Task<IActionResult> GetListInPeriod(HuWelfareDTO param)
        {
            var r = await _unitOfWork.BenerfitRepository.GetListInPeriod(param);
            return Ok(r);
        }
    }
}
