using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.AtTimeExplanation
{
    public class AtTimeExplanationRepository : IAtTimeExplanationRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_TIME_EXPLANATION, AtTimeExplanationDTO> _genericRepository;
        private readonly GenericReducer<AT_TIME_EXPLANATION, AtTimeExplanationDTO> _genericReducer;

        public AtTimeExplanationRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_TIME_EXPLANATION, AtTimeExplanationDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<AtTimeExplanationDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtTimeExplanationDTO> request)
        {
            var joined = from p in _dbContext.AtTimeExplanations.AsNoTracking()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from s in _dbContext.AtShifts.AsNoTracking().Where(s => s.ID == p.SHIFT_ID).DefaultIfEmpty()
                         from at in _dbContext.AtSalaryPeriods.AsNoTracking().Where(a => a.START_DATE <= p.EXPLANATION_DAY && p.EXPLANATION_DAY<=a.END_DATE).Take(1).DefaultIfEmpty()
                         from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from t in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.AsNoTracking().Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                         from c in _dbContext.SysUsers.AsNoTracking().Where(c => p.CREATED_BY == null ? false : c.ID == p.CREATED_BY).DefaultIfEmpty()
                         from u in _dbContext.SysUsers.AsNoTracking().Where(u => p.UPDATED_BY == null ? false : u.ID == p.UPDATED_BY).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new AtTimeExplanationDTO
                         {
                             Id = p.ID,
                             EmployeeCode = e.CODE,
                             EmployeeId = p.EMPLOYEE_ID,
                             EmployeeName = cv.FULL_NAME,
                             PositionName = t.NAME,
                             JobOrderNum = (int)(j.ORDERNUM ?? 999),
                             OrgName = o.NAME,
                             OrgId = o.ID,
                             ExplanationDay = p.EXPLANATION_DAY,
                             ShiftId = p.SHIFT_ID,
                             ShiftName = s.CODE,
                             ShiftYear = s.HOURS_START.ToString("yyyy"),
                             PeriodId = at.ID,
                             Year = p.EXPLANATION_DAY!.Value.Year,
                             SwipeIn = p.SWIPE_IN,
                             SwipeOut = p.SWIPE_OUT,
                             ActualWorkingHours = p.ACTUAL_WORKING_HOURS,
                             ExplanationCode = p.EXPLANATION_CODE,
                             Reason = p.REASON,
                             CreatedBy = p.CREATED_BY,
                             CreatedByUsername = c.USERNAME,
                             CreatedDate = p.CREATED_DATE,
                             UpdatedBy = p.UPDATED_BY,
                             UpdatedByUsername = u.USERNAME,
                             UpdatedDate = p.UPDATED_DATE,
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
            var joined = (from p in _dbContext.AtTimeExplanations.AsNoTracking().Where(p => p.ID == id)
                          from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                          from s in _dbContext.AtShifts.AsNoTracking().Where(s => s.ID == p.SHIFT_ID).DefaultIfEmpty()
                          from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                          from t in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                          from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                          from c in _dbContext.SysUsers.AsNoTracking().Where(c => p.CREATED_BY == null ? false : c.ID == p.CREATED_BY).DefaultIfEmpty()
                          from u in _dbContext.SysUsers.AsNoTracking().Where(u => p.UPDATED_BY == null ? false : u.ID == p.UPDATED_BY).DefaultIfEmpty()
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                          select new AtTimeExplanationDTO
                          {
                              Id = p.ID,
                              EmployeeCode = e.CODE,
                              EmployeeId = p.EMPLOYEE_ID,
                              EmployeeName = cv.FULL_NAME,
                              PositionName = t.NAME,
                              OrgName = o.NAME,
                              ExplanationDay = p.EXPLANATION_DAY,
                              ShiftId = p.SHIFT_ID,
                              ShiftName = s.NAME,
                              SwipeIn = p.SWIPE_IN,
                              SwipeOut = p.SWIPE_OUT,
                              ActualWorkingHours = p.ACTUAL_WORKING_HOURS,
                              ExplanationCode = p.EXPLANATION_CODE,
                              Reason = p.REASON,
                              CreatedBy = p.CREATED_BY,
                              CreatedByUsername = c.USERNAME,
                              CreatedDate = p.CREATED_DATE,
                              UpdatedBy = p.UPDATED_BY,
                              UpdatedByUsername = u.USERNAME,
                              UpdatedDate = p.UPDATED_DATE,
                          }).FirstOrDefault();
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtTimeExplanationDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtTimeExplanationDTO> dtos, string sid)
        {
            var add = new List<AtTimeExplanationDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtTimeExplanationDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtTimeExplanationDTO> dtos, string sid, bool patchMode = true)
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
        public async Task<FormatedResponse> GetListSalaryPeriod()
        {
            var queryable = (from p in _dbContext.AtSalaryPeriods.AsNoTracking()
                             select new
                             {
                                 Id = p.ID,
                                 Name = p.NAME,
                                 Code = p.YEAR,
                             });
            return new FormatedResponse() { InnerBody = queryable };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
    }
}

