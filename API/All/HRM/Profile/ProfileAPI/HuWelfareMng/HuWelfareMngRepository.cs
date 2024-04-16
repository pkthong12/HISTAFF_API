using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Common.Extensions;

namespace API.Controllers.HuWelfareMng
{
    public class HuWelfareMngRepository : IHuWelfareMngRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_WELFARE_MNG, HuWelfareMngDTO> _genericRepository;
        private readonly GenericReducer<HU_WELFARE_MNG, HuWelfareMngDTO> _genericReducer;

        public HuWelfareMngRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_WELFARE_MNG, HuWelfareMngDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuWelfareMngDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuWelfareMngDTO> request)
        {

            var joined = from l in _dbContext.HuWelfareMngs
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == l.EMPLOYEE_ID).DefaultIfEmpty()
                         from s in _dbContext.AtSalaryPeriods.AsNoTracking().Where(sp => sp.ID == l.PERIOD_ID).DefaultIfEmpty()
                         from p in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.Where(x => x.ID == p.JOB_ID).DefaultIfEmpty()
                         from d in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                         from w in _dbContext.HuWelfares.AsNoTracking().Where(we => we.ID == l.WELFARE_ID).DefaultIfEmpty()
                         from c in _dbContext.SysUsers.AsNoTracking().Where(c => l.CREATED_BY == null ? false : c.ID == l.CREATED_BY).DefaultIfEmpty()
                         from u in _dbContext.SysUsers.AsNoTracking().Where(u => l.UPDATED_BY == null ? false : u.ID == l.UPDATED_BY).DefaultIfEmpty()
                         select new HuWelfareMngDTO
                         {
                             Id = l.ID,
                             EmployeeId = l.EMPLOYEE_ID,
                             OrgId = e.ORG_ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile!.FULL_NAME,
                             WorkStatusId = e.WORK_STATUS_ID,
                             PositionName = p.NAME,
                             DepartmentName = d.NAME,
                             WelfareName = w.NAME,
                             DecisionCode = l.DECISION_CODE,
                             EffectDate = l.EFFECT_DATE,
                             Money = l.MONEY,
                             CreatedDate = l.CREATED_DATE,
                             UpdatedDate = l.UPDATED_DATE,
                             CreatedByUsername = c.USERNAME,
                             UpdatedByUsername = u.USERNAME,
                             JobOrderNum = (int)(j.ORDERNUM ?? 99)
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            var response = await _genericRepository.ReadAll();
            return response;
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

            var joined = (from l in _dbContext.HuWelfareMngs.AsNoTracking().Where(l => l.ID == id)
                          from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == l.EMPLOYEE_ID).DefaultIfEmpty()
                          from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                          from s in _dbContext.AtSalaryPeriods.AsNoTracking().Where(sp => sp.ID == l.PERIOD_ID).DefaultIfEmpty()
                          from p in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                          from d in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                          from w in _dbContext.HuWelfares.AsNoTracking().Where(we => we.ID == l.WELFARE_ID).DefaultIfEmpty()
                          from c in _dbContext.SysUsers.AsNoTracking().Where(c => l.CREATED_BY == null ? false : c.ID == l.CREATED_BY).DefaultIfEmpty()
                          from u in _dbContext.SysUsers.AsNoTracking().Where(u => l.UPDATED_BY == null ? false : u.ID == l.UPDATED_BY).DefaultIfEmpty()
                          from t in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                          from j in _dbContext.HuJobs.Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                          orderby j.ORDERNUM
                          select new HuWelfareMngDTO
                          {
                              Id = l.ID,
                              WelfareId = l.WELFARE_ID,
                              EmployeeId = l.EMPLOYEE_ID,
                              OrgId = e.ORG_ID,
                              EmployeeCode = e.CODE,
                              EmployeeName = cv.FULL_NAME,
                              PositionName = p.NAME,
                              DepartmentName = d.NAME,
                              PeriodId = l.PERIOD_ID,
                              WelfareName = w.NAME,
                              EffectDate = l.EFFECT_DATE,
                              ExpireDate = l.EXPIRE_DATE,
                              DecisionCode = l.DECISION_CODE,
                              Money = l.MONEY,
                              Note = l.NOTE,
                              CreatedDate = l.CREATED_DATE,
                              UpdatedDate = l.UPDATED_DATE,
                              CreatedByUsername = c.USERNAME,
                              UpdatedByUsername = u.USERNAME,
                              JobOrderNum = (int)(j.ORDERNUM ?? 99)
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuWelfareMngDTO dto, string sid)
        {
            var decision = await _dbContext.HuWorkings.AsNoTracking().Where(w => w.EMPLOYEE_ID == dto.EmployeeId).FirstOrDefaultAsync();
            if(decision != null && decision.TYPE_ID == OtherConfig.POSTPONE_CONTRACT && decision.STATUS_ID == OtherConfig.STATUS_APPROVE && (decision.EFFECT_DATE!.Value.Date <= dto.EffectDate!.Value.Date && dto.EffectDate!.Value.Date <= decision.EXPIRE_DATE!.Value.Date))
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.EMP_POSTPONE_CONTRACT, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            if (dto.EffectDate > dto.ExpireDate)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.EXP_MUST_LESS_THAN_EFF, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuWelfareMngDTO> dtos, string sid)
        {
            var add = new List<HuWelfareMngDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuWelfareMngDTO dto, string sid, bool patchMode = true)
        {
            if (dto.EffectDate > dto.ExpireDate)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.EXP_MUST_LESS_THAN_EFF , ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuWelfareMngDTO> dtos, string sid, bool patchMode = true)
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

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

    }
}

