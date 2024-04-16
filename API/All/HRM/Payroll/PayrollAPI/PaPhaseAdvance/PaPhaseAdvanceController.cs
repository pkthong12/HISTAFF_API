using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.PaPhaseAdvance
{
    [ApiExplorerSettings(GroupName = "122-PAYROLL-PA_PHASE_ADVANCE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PaPhaseAdvanceController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPaPhaseAdvanceRepository _PaPhaseAdvanceRepository;
        private IGenericRepository<PA_PHASE_ADVANCE_SYMBOL, PaPhaseAdvanceSymbolDTO> _genericRepositoryChild;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _fullDbContext;

        public PaPhaseAdvanceController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PaPhaseAdvanceRepository = new PaPhaseAdvanceRepository(dbContext, _uow);
            _genericRepositoryChild = _uow.GenericRepository<PA_PHASE_ADVANCE_SYMBOL, PaPhaseAdvanceSymbolDTO>();
            _appSettings = options.Value;
            _fullDbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _PaPhaseAdvanceRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _PaPhaseAdvanceRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _PaPhaseAdvanceRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PaPhaseAdvanceDTO> request)
        {
            var response = await _PaPhaseAdvanceRepository.SinglePhaseQueryList(request);
            //return Ok(response);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _PaPhaseAdvanceRepository.GetById(id);
            return Ok( response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _PaPhaseAdvanceRepository.GetById(id);
            return Ok( response );
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaPhaseAdvanceDTO model)
        {
            _uow.CreateTransaction();
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            //var response = await _PaPhaseAdvanceRepository.Create(_uow, model, sid);
            //return Ok(response);
            try
            {
                var response = await _PaPhaseAdvanceRepository.Create(_uow, model, sid);
                if (response != null)
                {
                    var phaseAdvanceObj = response.InnerBody as PA_PHASE_ADVANCE;
                    var symbolObj = response.InnerBody as PA_PHASE_ADVANCE_SYMBOL;
                    if (phaseAdvanceObj != null)
                    {
                        if (model.ListSymbolId != null)
                        {
                            List<PaPhaseAdvanceSymbolDTO> list = new();
                            model.ListSymbolId.ForEach(item =>
                            {
                                list.Add(new()
                                {
                                    PhaseAdvanceId = phaseAdvanceObj.ID,
                                    SymbolId = item,
                                });
                            });
                            var addListPhaseSym = await _genericRepositoryChild.CreateRange(_uow, list, sid);
                        }
                        else
                        {
                            List<PaPhaseAdvanceSymbolDTO> list = new();
                            list.Add(new()
                            {
                                PhaseAdvanceId = phaseAdvanceObj.ID,
                                SymbolId = null,
                            });
                            var addListPhaseSym = await _genericRepositoryChild.CreateRange(_uow, list, sid);
                        }
                    }
                }
                await _fullDbContext.SaveChangesAsync();
                _uow.Commit();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PaPhaseAdvanceDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPhaseAdvanceRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PaPhaseAdvanceDTO model)
        {
            _uow.CreateTransaction();
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            try
            {
                var response = await _PaPhaseAdvanceRepository.Update(_uow, model, sid);
                if (model.ListSymbolId != null && model.ListSymbolId.Count != 0 && model.ListSymbolId[0] != null)
                {
                    _fullDbContext.PaPhaseAdvanceSymbols.RemoveRange(_fullDbContext.PaPhaseAdvanceSymbols.Where(x => x.PHASE_ADVANCE_ID == model.Id));
                    foreach (long item in model.ListSymbolId)
                    {
                        PA_PHASE_ADVANCE_SYMBOL objEmpData = new()
                        {
                            SYMBOL_ID = item,
                            PHASE_ADVANCE_ID = model.Id,
                        };
                        await _fullDbContext.PaPhaseAdvanceSymbols.AddAsync(objEmpData);
                    }
                }
                else
                {
                    _fullDbContext.PaPhaseAdvanceSymbols.RemoveRange(_fullDbContext.PaPhaseAdvanceSymbols.Where(x => x.PHASE_ADVANCE_ID == model.Id));
                }
                await _fullDbContext.SaveChangesAsync();
                _uow.Commit();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PaPhaseAdvanceDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPhaseAdvanceRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PaPhaseAdvanceDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PaPhaseAdvanceRepository.Delete(_uow, (long)model.Id);
                return Ok(new FormatedResponse() { InnerBody = response });
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            var response = await _PaPhaseAdvanceRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _PaPhaseAdvanceRepository.DeleteIds(_uow, model.Ids);
            return Ok(response );
        }

        [HttpGet]
        public async Task<IActionResult> GetYearPeriod()
        {
            var response = await _PaPhaseAdvanceRepository.GetYearPeriod();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrgId()
        {
            var response = await _PaPhaseAdvanceRepository.GetOrgId();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetMonthPeriodAt(int year)
        {
            var response = await _PaPhaseAdvanceRepository.GetMonthPeriodAt(year);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetOtherListSal()
        {
            var response = await _PaPhaseAdvanceRepository.GetOtherListSal();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAtSymbol()
        {
            var response = await _PaPhaseAdvanceRepository.GetAtSymbol();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetYearByPeriod(long id)
        {
            var response = await _PaPhaseAdvanceRepository.GetYearByPeriod(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAtSymbolById(long id)
        {
            try
            {
                var entity = _uow.Context.Set<AT_SYMBOL>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    where p.ID == id
                                    select new
                                    {
                                        Id = p.ID,
                                        Name = p.CODE + '-' + p.NAME,
                                    }).FirstOrDefaultAsync();
                return Ok(new FormatedResponse() { InnerBody = joined });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPhaseAdvanceRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }
    }
}

