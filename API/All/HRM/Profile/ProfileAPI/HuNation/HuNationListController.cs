using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.AutoMapper;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuNation
{
    [ApiExplorerSettings(GroupName = "079-PROFILE-HU_NATION")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuNationListController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuNationRepository _HuNationRepository;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _con;
        public HuNationListController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuNationRepository = new HuNationRepository(dbContext, _uow);
            _appSettings = options.Value;
            _con = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuNationDTO> request)
        {

            try
            {
                var response = await _HuNationRepository.SinglePhaseQueryList(request);

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
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuNationRepository.GetById(id);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HuNationDTO model)
        {
            try
            {
                var sid = Request.Sid(_appSettings);
                if (sid == null) return Unauthorized();
                model.IsActive = true;
                var response = await _HuNationRepository.Create(_uow, model, sid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuNationDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuNationRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            var check = await _HuNationRepository.CheckActive(model.Ids);
            if (check.StatusCode == EnumStatusCode.StatusCode400)
            {
                return Ok(check);
            }
            if (check.StatusCode == EnumStatusCode.StatusCode200)
            {
                var response = await _HuNationRepository.DeleteIds(_uow, model.Ids);
                return Ok(response);
            }
            return Ok(check);
        }

        [HttpGet]
        public async Task<IActionResult> CreateNewCode()
        {
            var response = await _HuNationRepository.CreateNewCode();
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuNationRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }
    }
}
