using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Common.Extensions;

namespace API.Controllers.AtSignDefault
{
    public class AtSignDefaultRepository : IAtSignDefaultRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_SIGN_DEFAULT, AtSignDefaultDTO> _genericRepository;
        private readonly GenericReducer<AT_SIGN_DEFAULT, AtSignDefaultDTO> _genericReducer;

        public AtSignDefaultRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_SIGN_DEFAULT, AtSignDefaultDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<AtSignDefaultDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtSignDefaultDTO> request)
        {
            var joined = from p in _dbContext.AtSignDefaults
                         from s in _dbContext.AtShifts.AsNoTracking().Where(sh => sh.ID == p.SIGN_DEFAULT).DefaultIfEmpty()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from pos in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.AsNoTracking().Where(j => j.ID == pos.JOB_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(org => org.ID == e.ORG_ID).DefaultIfEmpty()
                         orderby j.ORDERNUM
                         select new AtSignDefaultDTO
                         {
                             Id = p.ID,
                             EmployeeId = p.EMPLOYEE_ID,
                             OrgId = p.ORG_ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile!.FULL_NAME,
                             PositionName = pos.NAME,
                             OrgName = o.NAME,
                             SignDefaultName = "[" + s.CODE + "]",
                             EffectDateFrom = p.EFFECT_DATE_FROM,
                             EffectDateTo = p.EFFECT_DATE_TO,
                             Note = p.NOTE,
                             IsActive = p.IS_ACTIVE,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                             JobOrderNum = (int)(j.ORDERNUM ?? 999),
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

            var joined = (from l in _dbContext.AtSignDefaults.AsNoTracking().Where(l => l.ID == id)
                          from s in _dbContext.AtShifts.AsNoTracking().Where(sh => sh.ID == l.SIGN_DEFAULT).DefaultIfEmpty()
                          from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == l.EMPLOYEE_ID).DefaultIfEmpty()
                          from pos in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                          from o in _dbContext.HuOrganizations.AsNoTracking().Where(org => org.ID == e.ORG_ID).DefaultIfEmpty()
                          from c in _dbContext.SysUsers.AsNoTracking().Where(c => l.CREATED_BY == null ? false : c.ID == l.CREATED_BY).DefaultIfEmpty()
                          from u in _dbContext.SysUsers.AsNoTracking().Where(u => l.UPDATED_BY == null ? false : u.ID == l.UPDATED_BY).DefaultIfEmpty()
                          select new AtSignDefaultDTO
                          {
                              Id = l.ID,
                              EmployeeId = l.EMPLOYEE_ID,
                              EmployeeName = e.Profile!.FULL_NAME,
                              PositionName = pos.NAME,
                              OrgName = o.NAME,
                              OrgId = l.ORG_ID,
                              SignDefault = l.SIGN_DEFAULT,
                              EffectDateFrom = l.EFFECT_DATE_FROM,
                              EffectDateTo = l.EFFECT_DATE_TO,
                              Note = l.NOTE,
                              IsActive = l.IS_ACTIVE,
                              CreatedDate = l.CREATED_DATE,
                              UpdatedDate = l.UPDATED_DATE,
                              CreatedByUsername = c.USERNAME,
                              UpdatedByUsername = u.USERNAME
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtSignDefaultDTO dto, string sid)
        {
            // check effectFrom and effectTo
            if (dto.EffectDateFrom > dto.EffectDateTo)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.EXP_MUST_LESS_THAN_EFF, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

            // Check Employee
            var employee = await _dbContext.HuEmployees.Where(p => p.ID == dto.EmployeeId).FirstAsync();
            if (employee.WORK_STATUS_ID == OtherConfig.EMP_STATUS_TERMINATE  && (employee.TER_EFFECT_DATE <= dto.EffectDateFrom || dto.EffectDateTo <= employee.TER_EFFECT_DATE))
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.EMP_HAVE_NO_WORKING_STATUS, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

            // check wage
            var latestWage = await _dbContext.HuWorkings.Where(p => p.EMPLOYEE_ID == dto.EmployeeId && p.IS_WAGE != 0 && p.STATUS_ID == OtherConfig.STATUS_APPROVE).OrderBy(p => p.EFFECT_DATE).FirstOrDefaultAsync();
            if(latestWage == null)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.EMP_NOT_HAVE_SALARY_PROFILE, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            else
            {
                if ((latestWage.EFFECT_DATE!.Value.Date > dto.EffectDateFrom!.Value.Date))
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.SALARY_PROFILE_EXPIRED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }


            // check date -- todo 
            if(dto.EffectDateTo != null)
            {
                var checkExist = _dbContext.AtSignDefaults.Where(p => p.EMPLOYEE_ID == dto.EmployeeId
                        && (p.EFFECT_DATE_FROM!.Value.Date <= dto.EffectDateTo!.Value.Date && dto.EffectDateTo!.Value.Date <= p.EFFECT_DATE_TO!.Value.Date)
                        && p.IS_ACTIVE == true).Count();
                if (checkExist != 0)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.EMPLOYEE_HAVE_EXIST_SHIFT_DEFAULT, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            else
            {
                var checkExist = _dbContext.AtSignDefaults.Where(p => p.EMPLOYEE_ID == dto.EmployeeId 
                        && ((p.EFFECT_DATE_FROM!.Value.Date <= dto.EffectDateFrom!.Value.Date && dto.EffectDateFrom!.Value.Date <= p.EFFECT_DATE_TO!.Value.Date)) 
                        && p.IS_ACTIVE == true).Count();
                if(checkExist != 0)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.EMPLOYEE_HAVE_EXIST_SHIFT_DEFAULT, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                var checkSpecialCase = _dbContext.AtSignDefaults.Where(p => p.EMPLOYEE_ID == dto.EmployeeId && (p.EFFECT_DATE_FROM!.Value.Date <= dto.EffectDateFrom!.Value.Date && p.EFFECT_DATE_TO == null)).Count();
                if(checkSpecialCase != 0)
                {
                    var signDefault = await _dbContext.AtSignDefaults.Where(p => p.EMPLOYEE_ID == dto.EmployeeId && (p.EFFECT_DATE_FROM!.Value.Date <= dto.EffectDateFrom!.Value.Date && p.EFFECT_DATE_TO == null)).OrderByDescending(p => p.ID).FirstAsync();
                    signDefault.EFFECT_DATE_TO = dto.EffectDateFrom.Value.AddDays(-1);
                    _dbContext.Entry(signDefault).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }

            }


            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtSignDefaultDTO> dtos, string sid)
        {
            var add = new List<AtSignDefaultDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtSignDefaultDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtSignDefaultDTO> dtos, string sid, bool patchMode = true)
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
            foreach (var id in ids)
            {
                var item = await _dbContext.AtSignDefaults.Where(x => x.ID == id).AsNoTracking().SingleOrDefaultAsync();
                if (item != null && item.IS_ACTIVE == true)
                {
                    _uow.Rollback();
                    return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE };
                }
            }
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }

    }
}

