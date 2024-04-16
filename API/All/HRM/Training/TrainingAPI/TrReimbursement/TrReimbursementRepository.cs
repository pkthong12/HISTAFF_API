using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Common.Extensions;

namespace API.Controllers.TrReimbursement
{
    public class TrReimbursementRepository : ITrReimbursementRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_REIMBURSEMENT, TrReimbursementDTO> _genericRepository;
        private readonly GenericReducer<TR_REIMBURSEMENT, TrReimbursementDTO> _genericReducer;

        public TrReimbursementRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_REIMBURSEMENT, TrReimbursementDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<TrReimbursementDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrReimbursementDTO> request)
        {
            var _dayNow = DateTime.Now;
            var joined = from p in _dbContext.TrReimbursements.AsNoTracking()
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from e in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                         from t in _dbContext.TrPrograms.AsNoTracking().Where(x => x.ID == p.TR_PROGRAM_ID).DefaultIfEmpty()
                         from lj in _dbContext.HuTerminates.AsNoTracking().Where(x => x.EMPLOYEE_ID == e.ID && x.STATUS_ID == OtherConfig.STATUS_APPROVE).OrderByDescending(x => x.CREATED_BY).DefaultIfEmpty()
                         select new TrReimbursementDTO
                         {
                             Id = p.ID,
                             OrgId = e.ORG_ID,
                             IsReserves = p.IS_RESERVES,
                             IsLaveDate = p.IS_LAVE_DATE,
                             Year = p.YEAR,
                             DateFromCommitment = p.DATE_FROM_COMMITMENT,
                             DateToCommitment = p.DATE_TO_COMMITMENT,
                             EndDateFromCommitment = p.END_DATE_FROM_COMMITMENT,
                             EndDateToCommitment = p.END_DATE_TO_COMMITMENT,
                             MonthReimbursement = p.MONTH_REIMBURSEMENT,
                             LeaveJobDate = lj.EFFECT_DATE,
                             TrProgramName = t.NAME,
                             TrStartDate = t.START_DATE,
                             TrEndDate = t.END_DATE,
                             CostReimburse = t.COST_STUDENT,
                             NumCommitment = (p.DATE_TO_COMMITMENT!.Value).DayOfYear - (p.DATE_FROM_COMMITMENT!.Value).DayOfYear,
                             FinalDate = p.FINAL_DATE,
                             NumDayRemainingCommit = (p.DATE_TO_COMMITMENT.Value < _dayNow) ? 0 : (decimal)(_dayNow - p.DATE_FROM_COMMITMENT.Value).TotalDays,
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, long value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return new() { InnerBody = resposne };
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, string value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return new() { InnerBody = resposne };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var joined = await (from l in _dbContext.TrReimbursements.Where(x => x.ID == id)
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                          select new TrReimbursementDTO
                          {
                              Id = l.ID
                          }).FirstOrDefaultAsync(); 
            if (joined != null)
            {
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrReimbursementDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrReimbursementDTO> dtos, string sid)
        {
            var add = new List<TrReimbursementDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrReimbursementDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrReimbursementDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(_uow, dtos, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, long id)
        {
            var response = await _genericRepository.Delete(_uow, id);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<long> ids)
        {
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetListProgram(TrReimbursementDTO param)
        {
            var query = await (from p in _dbContext.TrPrograms.AsNoTracking()
                               where p.YEAR == param.Year
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }
    }
}

