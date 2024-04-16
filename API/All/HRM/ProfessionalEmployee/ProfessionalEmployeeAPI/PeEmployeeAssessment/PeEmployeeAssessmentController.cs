using API.All.DbContexts;
using API.All.SYSTEM.Common;
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

namespace API.All.HRM.ProfessionalEmployee.ProfessionalEmployeeAPI.PeEmployeeAssessment
{
    [ApiExplorerSettings(GroupName = "567-PROFESSIONAL_EMPLOYEE-PE_EMPLOYEE_ASSESSMENT")]

    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]

    public class PeEmployeeAssessmentController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly PeEmployeeAssessmentRepository _PeEmployeeAssessmentRepository;
        private readonly AppSettings _appSettings;

        public PeEmployeeAssessmentController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PeEmployeeAssessmentRepository = new PeEmployeeAssessmentRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _PeEmployeeAssessmentRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _PeEmployeeAssessmentRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _PeEmployeeAssessmentRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PeEmployeeAssessmentDTO> request)
        {
            try
            {
                var response = await _PeEmployeeAssessmentRepository.SinglePhaseQueryList(request);

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
            var response = await _PeEmployeeAssessmentRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _PeEmployeeAssessmentRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PeEmployeeAssessmentDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            var response = await _PeEmployeeAssessmentRepository.Create(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PeEmployeeAssessmentDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PeEmployeeAssessmentRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PeEmployeeAssessmentDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            var response = await _PeEmployeeAssessmentRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PeEmployeeAssessmentDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PeEmployeeAssessmentRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PeEmployeeAssessmentDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PeEmployeeAssessmentRepository.Delete(_uow, (long)model.Id);
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
            var response = await _PeEmployeeAssessmentRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _PeEmployeeAssessmentRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(RequestModel requestModel)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();


            // check "huCompetencyPeriodId"
            if (requestModel.huCompetencyPeriodId == null || requestModel.huCompetencyPeriodId == -1)
            {
                return Ok(new FormatedResponse()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCodes.NO_SELECTED_HU_COMPETENCY_PERIOD_ID_TO_CREATE,
                    StatusCode = EnumStatusCode.StatusCode400
                });
            }


            // check "ids"
            if (requestModel.ids == null || requestModel.ids.Count() == 0)
            {
                return Ok(new FormatedResponse()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCodes.NO_SELECTED_ID_TO_CREATE,
                    StatusCode = EnumStatusCode.StatusCode400
                });
            }
            else
            {
                var response = await _PeEmployeeAssessmentRepository.AddEmployee(requestModel, _uow, sid);
                return Ok(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(List<long> ids)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            if (ids.Count() == 0)
            {
                return Ok(new FormatedResponse()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCodes.NO_SELECTED_ID_TO_DELETE,
                    StatusCode = EnumStatusCode.StatusCode400
                });
            }

            var response = await _PeEmployeeAssessmentRepository.DeleteEmployee(ids, _uow);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetDropDownListEvaluationPeriod()
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            var response = await _PeEmployeeAssessmentRepository.GetDropDownListEvaluationPeriod();
            return Ok(response);
        }
    }
}