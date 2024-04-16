using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.AtSalaryPeriod
{
    [ApiExplorerSettings(GroupName = "015-ATTENDANCE-AT_SALARY_PERIOD")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class AtSalaryPeriodController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IAtSalaryPeriodRepository _AtSalaryPeriodRepository;
        private readonly AppSettings _appSettings;
        private IGenericRepository<AT_SALARY_PERIOD, AtSalaryPeriodDTO> _genericRepository;

        public AtSalaryPeriodController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _AtSalaryPeriodRepository = new AtSalaryPeriodRepository(dbContext, _uow);
            _appSettings = options.Value;
            _genericRepository = _uow.GenericRepository<AT_SALARY_PERIOD, AtSalaryPeriodDTO>();
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _AtSalaryPeriodRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _AtSalaryPeriodRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _AtSalaryPeriodRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<AtSalaryPeriodDTO> request)
        {
            var response = await _AtSalaryPeriodRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _AtSalaryPeriodRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _AtSalaryPeriodRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AtSalaryPeriodDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtSalaryPeriodRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<AtSalaryPeriodDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtSalaryPeriodRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AtSalaryPeriodDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtSalaryPeriodRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<AtSalaryPeriodDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtSalaryPeriodRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AtSalaryPeriodDTO model)
        {
            if (model.Id != null)
            {
                var response = await _AtSalaryPeriodRepository.Delete(_uow, (long)model.Id);
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
            var response = await _AtSalaryPeriodRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _AtSalaryPeriodRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewWithAtOrgPeriods(AtSalaryPeriodDTO reqquest)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtSalaryPeriodRepository.AddNewWithAtOrgPeriods(reqquest, sid);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateWithAtOrgPeriods(AtSalaryPeriodDTO reqquest)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtSalaryPeriodRepository.UpdateWithAtOrgPeriods(_uow, reqquest, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetListSalaryInYear(AtSalaryPeriodDTO param)
        {
            var response = await _AtSalaryPeriodRepository.GetListSalaryInYear(param);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetListSalaryPeriod(AtSalaryPeriodDTO param)
        {
            var response = await _AtSalaryPeriodRepository.GetListSalaryPeriod(param);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetYear()
        {
            try
            {
                //var response = await (from s in _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking()
                //                      from c in _uow.Context.Set<SYS_USER>().AsNoTracking().Where(x => x.ID == s.CREATED_BY)
                //                      from u in _uow.Context.Set<SYS_USER>().AsNoTracking().Where(x => x.ID == s.UPDATED_BY)
                //                      where s.IS_ACTIVE == true
                //                      select new
                //                      {
                //                          Year = s.YEAR
                //                      }
                //                      ).Select(x => x.Year).ToListAsync();
                

                var data = await _uow.Context.Set<AT_SALARY_PERIOD>().Where(x => x.IS_ACTIVE == true).ToListAsync();
                var result = data.Select(s => s.YEAR).Distinct().ToList();

                return Ok(new FormatedResponse() { InnerBody = result });
            }
            catch (Exception ex)
            {

                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMonthByYear(int year)
        {
            var response = await (from s in _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking().AsQueryable()
                                  where s.IS_ACTIVE == true
                                  where s.YEAR == year
                                  select new
                                  {
                                      Id = s.ID,
                                      Month = s.NAME,
                                  }).ToListAsync();
            return Ok(new FormatedResponse() { InnerBody = response });
        }
        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _genericRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(new FormatedResponse()
            {
                MessageCode = response.MessageCode,
                InnerBody = response.InnerBody,
                StatusCode = EnumStatusCode.StatusCode200
            });
        }

    }
}

