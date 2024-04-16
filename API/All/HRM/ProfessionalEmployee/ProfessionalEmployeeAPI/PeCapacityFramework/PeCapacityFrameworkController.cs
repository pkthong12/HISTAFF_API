using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System.Linq.Dynamic.Core;

namespace API.All.HRM.ProfessionalEmployee.ProfessionalEmployeeAPI.PeCapacityFramework
{
    [ApiExplorerSettings(GroupName = "567-PROFESSIONAL_EMPLOYEE-PE_CAPACITY_FRAMEWORK")]

    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]

    public class PeCapacityFrameworkController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly PeCapacityFrameworkRepository _PeCapacityFrameworkRepository;
        private readonly AppSettings _appSettings;

        public PeCapacityFrameworkController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PeCapacityFrameworkRepository = new PeCapacityFrameworkRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _PeCapacityFrameworkRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _PeCapacityFrameworkRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _PeCapacityFrameworkRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PeCapacityFrameworkDTO> request)
        {
            try
            {
                var response = await _PeCapacityFrameworkRepository.SinglePhaseQueryList(request);

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
            var response = await _PeCapacityFrameworkRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _PeCapacityFrameworkRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PeCapacityFrameworkDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            var response = await _PeCapacityFrameworkRepository.Create(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PeCapacityFrameworkDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PeCapacityFrameworkRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PeCapacityFrameworkDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            var response = await _PeCapacityFrameworkRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PeCapacityFrameworkDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PeCapacityFrameworkRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PeCapacityFrameworkDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PeCapacityFrameworkRepository.Delete(_uow, (long)model.Id);
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
            bool isCheckDataActive = false;

            foreach (var item in model.Ids)
            {
                var getDataActive = _uow.Context.Set<PE_CAPACITY_FRAMEWORK>().Where(x => x.ID == item && x.IS_ACTIVE == true).FirstOrDefault();

                if (getDataActive != null)
                {
                    isCheckDataActive = true;
                    break;
                }
            }

            if (isCheckDataActive == true)
            {
                return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_IS_ACTIVE });
            }
            else
            {
                var response = await _PeCapacityFrameworkRepository.DeleteIds(_uow, model.Ids);
                return Ok(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _PeCapacityFrameworkRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            var response = await _PeCapacityFrameworkRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }
    }
}