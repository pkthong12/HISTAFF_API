using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using Common.Repositories;
using Common.Extensions;
using Common.Interfaces;
using Microsoft.Extensions.Options;
using Azure;

namespace API.Controllers.HuWelfareAuto
{
    public class HuWelfareAutoRepository : RepositoryBase<HU_WELFARE_AUTO>, IHuWelfareAutoRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_WELFARE_AUTO, HuWelfareAutoDTO> _genericRepository;
        private readonly GenericReducer<HU_WELFARE_AUTO, HuWelfareAutoDTO> _genericReducer;

        public HuWelfareAutoRepository(FullDbContext context, GenericUnitOfWork uow) : base(context)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_WELFARE_AUTO, HuWelfareAutoDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuWelfareAutoDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuWelfareAutoDTO> request)
        {            
            var joined = from p in _dbContext.HuWelfareAutos.AsNoTracking()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(x=>x.ID == p.ORG_ID).DefaultIfEmpty()
                         from po in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.Where(x => x.ID == po.JOB_ID).DefaultIfEmpty()
                         from sp in _dbContext.AtSalaryPeriods.AsNoTracking().Where(x => x.ID == p.SALARY_PERIOD_ID).DefaultIfEmpty()
                         from ot in _dbContext.HuWelfares.AsNoTracking().Where(x => x.ID == p.WELFARE_ID).DefaultIfEmpty()
                         from g in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.GENDER_ID).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuWelfareAutoDTO
                         {
                             Id = p.ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile.FULL_NAME,
                             OrgId = p.ORG_ID,
                             OrgName = o.NAME,
                             PositionName = po.NAME,
                             SalaryPeriodId = p.SALARY_PERIOD_ID,
                             SalaryPeriodName = sp.NAME,
                             BenefitName = ot.NAME,
                             BenefitId = p.WELFARE_ID,
                             BirthDate = p.BIRTH_DATE,
                             CountChild = p.COUNT_CHILD,
                             GenderName = g.NAME,
                             Seniority = p.SENIORITY,
                             ContactTypeName = p.CONTRACT_TYPE_NAME,
                             Money = p.MONEY,
                             EffectiveDate = p.EFFECTIVE_DATE,
                             CreatedDate = p.CREATED_DATE,
                             UpdatedDate = p.UPDATED_DATE,
                             JobOrderNum = (int)(j.ORDERNUM ?? 99)
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
            try
            {
                var r = await (from l in _dbContext.HuWelfareAutos
                                    from e in _dbContext.HuEmployees.Where(x => x.ID == l.EMPLOYEE_ID).DefaultIfEmpty()
                                    from p in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                    from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                    where l.ID == id
                                    select new HuWelfareAutoDTO
                                    {
                                        Id = l.ID,
                                        EmployeeID = l.EMPLOYEE_ID,
                                        EmployeeCode = e.CODE,
                                        EmployeeName = e.Profile.FULL_NAME,
                                        PositionName = p.NAME,
                                        OrgName = o.NAME,
                                        ContactTypeName = l.CONTRACT_TYPE_NAME,
                                        Year = l.YEAR,
                                        SalaryPeriodId = l.SALARY_PERIOD_ID,
                                        WelfareId = l.WELFARE_ID,
                                        BirthDate = l.BIRTH_DATE,
                                        CountChild = l.COUNT_CHILD,
                                        GenderId = l.GENDER_ID,
                                        Seniority = l.SENIORITY,
                                        Money = l.MONEY,
                                        EffectiveDate = l.EFFECTIVE_DATE,
                                        ExpirationDate = l.EXPIRATION_DATE,
                                    }).FirstOrDefaultAsync();
                return new() { InnerBody = r };
            }
            catch (Exception ex)
            {
                return new() { InnerBody = 400 };
            }
        }
        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuWelfareAutoDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuWelfareAutoDTO> dtos, string sid)
        {
            var add = new List<HuWelfareAutoDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuWelfareAutoDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuWelfareAutoDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetAllPeriodYear()
        {
            var response = await (from p in _dbContext.AtSalaryPeriods.AsNoTracking().DefaultIfEmpty()
                                  select new
                                  {
                                      Id = p.ID,
                                      Name = p.NAME
                                  }).ToListAsync();
            return new FormatedResponse() { InnerBody = response};
        }

        public async Task<ResultWithError> Calculate(long? orgId, long? welfareId, long? periodId, string? calculateDate)
        {
            try
            {
                var x = new
                {
                    P_USER_ID    = _dbContext.CurrentUserId,
                    P_ORGID      = orgId,
                    P_ISDISSOLVE = -1,
                    P_WELFARE_ID = welfareId,
                    P_PERIOD_ID  = periodId,
                    P_CALCULATE_DATE = Convert.ToDateTime(calculateDate.Replace("_", "/"))
                };
                var data = QueryData.ExecuteStoreToTable(Procedures.PKG_PROFILE_BUSSINESS_GET_WELFARE_AUTO,
                    x, false);
                if ((int)data.Tables[0].Rows[0][0] == 1)
                {
                    return new ResultWithError(200,(int)data.Tables[0].Rows[0][0]);
                }
                else
                {

                    return new ResultWithError(400);
                }
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
    }
}

