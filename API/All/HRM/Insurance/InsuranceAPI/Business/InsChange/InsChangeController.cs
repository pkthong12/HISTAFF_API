using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.InsChange
{
    [ApiExplorerSettings(GroupName = "104-INSURANCE-INS_CHANGE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class InsChangeController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IInsChangeRepository _InsChangeRepository;
        private readonly AppSettings _appSettings;

        public InsChangeController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _InsChangeRepository = new InsChangeRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _InsChangeRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _InsChangeRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _InsChangeRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<InsChangeDTO> request)
        {
            try
            {
                var response = await _InsChangeRepository.SinglePhaseQueryList(request);
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
                    /*return Ok(new FormatedResponse()
                    {
                        InnerBody = response
                    });*/

                    return Ok(
                        response
                    );
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
            var response = await _InsChangeRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _InsChangeRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InsChangeDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsChangeRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<InsChangeDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsChangeRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(InsChangeDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsChangeRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<InsChangeDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsChangeRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(InsChangeDTO model)
        {
            if (model.Id != null)
            {
                var response = await _InsChangeRepository.Delete(_uow, (long)model.Id);
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
            var response = await _InsChangeRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _InsChangeRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetOtherListInsType()
        {
            var response = await _InsChangeRepository.GetOtherListType();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetTypeInsChange()
        {
            var response = await _InsChangeRepository.GetInsTypeChange();
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetInforById(long id)
        {
            var response = await _InsChangeRepository.GetInforById(id);
            return Ok(response);

        }

        [HttpPost]
        public async Task<IActionResult> GetInschangeDashboard()
        {
            var response = await _InsChangeRepository.GetInschangeDashboard();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetLstInsCheck(long id)
        {
            var response = await _InsChangeRepository.GetLstInsCheck(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetUnit(long id)
        {
            var response = await _InsChangeRepository.GetUnit(id);
            return Ok(response);
        }
         [HttpPost]
        public async Task<IActionResult> SpsInsArisingManualLoad(InsChangeDTO dto)
        {
            var response = await _InsChangeRepository.SpsInsArisingManualLoad(dto);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> SpsInsArisingManualLoad2(InsChangeDTO dto)
        {
            var response = await _InsChangeRepository.SpsInsArisingManualLoad2(dto);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> SpsInsArisingManualGet(InsChangeDTO dto)
        {
            var response = await _InsChangeRepository.SpsInsArisingManualGet(dto);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> SpsInsArisingManualGet2(InsChangeDTO dto)
        {
            var response = await _InsChangeRepository.SpsInsArisingManualGet2(dto);
            return Ok(response);
        }
    }
}

