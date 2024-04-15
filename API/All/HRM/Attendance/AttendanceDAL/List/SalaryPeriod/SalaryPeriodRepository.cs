using Common.Paging;
using AttendanceDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;
using CORE.GenericUOW;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;

namespace AttendanceDAL.Repositories
{
    public class SalaryPeriodRepository : RepositoryBase<AT_SALARY_PERIOD>, ISalaryPeriodRepository
    {
        private readonly GenericUnitOfWork _uow;
        private AttendanceDbContext _appContext => (AttendanceDbContext)_context;
        private readonly GenericReducer<AT_SALARY_PERIOD, SalaryPeriodDTO> genericReducer;
        private IGenericRepository<AT_SALARY_PERIOD, SalaryPeriodDTO> _genericRepository;
        public SalaryPeriodRepository(AttendanceDbContext context) : base(context)
        {
            genericReducer = new();
        }
        public async Task<GenericPhaseTwoListResponse<SalaryPeriodDTO>> TwoPhaseQueryList(GenericQueryListDTO<SalaryPeriodDTO> request)
        {
            var raw = _appContext.SalaryPeriods.AsQueryable();
            var phase1 = await genericReducer.FirstPhaseReduce(raw, request);

            if (phase1.ErrorType != EnumErrorType.NONE)
            {
                return new()
                {
                    ErrorType = phase1.ErrorType,
                    MessageCode = phase1.MessageCode,
                    ErrorPhase = 1
                };
            }

            if (phase1.Queryable == null)
            {
                return new()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND,
                    ErrorPhase = 1
                };
            }

            var phase1IdsResult = phase1.Queryable.ToList().Aggregate("", (prev, curr) => prev + curr.ID.ToString() + ";");
            var ids = phase1IdsResult.Split(';');

            var joined = from p in _appContext.SalaryPeriods.Where(p => ids.Contains(p.ID.ToString()))
                         select new SalaryPeriodDTO
                         {
                             Id = p.ID,
                             Name = p.NAME,
                             Year = p.YEAR,
                             Month = p.MONTH,
                             DateStart = p.START_DATE,
                             DateEnd = p.END_DATE,
                             StandardWorking = p.STANDARD_WORKING,
                             /*StandardTime = p.STANDARD_TIME,*/
                             Note = p.NOTE,
                             IsActive = p.IS_ACTIVE
                         };
            var phase2 = await genericReducer.SecondPhaseReduce(joined, request);
            return phase2;
        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<SalaryPeriodDTO>> GetAll(SalaryPeriodDTO param)
        {
            var queryable = from p in _appContext.SalaryPeriods

                            orderby p.YEAR descending, p.MONTH descending
                            select new SalaryPeriodDTO
                            {
                                Id = p.ID,
                                Name = p.NAME,
                                Year = p.YEAR,
                                Month = p.MONTH,
                                DateStart = p.START_DATE,
                                DateEnd = p.END_DATE,
                                StandardWorking = p.STANDARD_WORKING,
                                /*StandardTime = p.STANDARD_TIME,*/
                                Note = p.NOTE,
                                IsActive = p.IS_ACTIVE
                            };

            if (param.Year != null)
            {
                queryable = queryable.Where(p => p.Year == param.Year);
            }
            if (param.Month != null)
            {
                queryable = queryable.Where(p => p.Month == param.Month);
            }
            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }

            if (param.DateStart != null)
            {
                queryable = queryable.Where(p => p.DateStart == param.DateStart);
            }
            if (param.DateEnd != null)
            {
                queryable = queryable.Where(p => p.DateEnd == param.DateEnd);
            }
            if (param.StandardWorking != null)
            {
                queryable = queryable.Where(p => p.StandardWorking == param.StandardWorking);
            }
            if (param.IsActive != null)
            {
                queryable = queryable.Where(p => p.IsActive == param.IsActive);
            }
            return await PagingList(queryable, param);
        }
        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetById(long id)
        {
            try
            {
                var r = await (from p in _appContext.SalaryPeriods
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                                   Year = p.YEAR,
                                   Month = p.MONTH,
                                   DateStart = p.START_DATE,
                                   DateEnd = p.END_DATE,
                                   StandardWorking = p.STANDARD_WORKING,
                                   /*StandardTime = p.STANDARD_TIME,*/
                                   Note = p.NOTE,
                                   Dtl = (from a in _appContext.SalaryPeriodDtls
                                          join o in _appContext.Organizations on a.ORG_ID equals o.ID
                                          where a.PERIOD_ID == p.ID && a.ORG_ID != null
                                          select new
                                          {
                                              Id = a.ID,
                                              Name = o.NAME,
                                              WorkingStandard = a.WORKING_STANDARD,
                                              StandardTime = a.STANDARD_TIME,
                                              OrgId = a.ORG_ID,
                                              EmpId = a.EMP_ID
                                          }).ToList(),
                                   DtlEmp = (from b in _appContext.SalaryPeriodDtls
                                             join c in _appContext.Employees on b.EMP_ID equals c.ID
                                             join d in _appContext.Organizations on c.ORG_ID equals d.ID
                                             join e in _appContext.Positions on c.POSITION_ID equals e.ID
                                             where b.PERIOD_ID == p.ID && b.EMP_ID != null
                                             select new
                                             {
                                                 Id = b.ID,
                                                 Code = c.CODE,
                                                 Name = c.Profile.FULL_NAME,
                                                 OrgName = d.NAME,
                                                 PositionName = e.NAME,
                                                 WorkingStandard = b.WORKING_STANDARD,
                                                 StandardTime = b.STANDARD_TIME,
                                                 EmpId = b.EMP_ID
                                             }).ToList()
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(SalaryPeriodInputDTO param)
        {
            try
            {
                // check ky công lương
                var p = await _appContext.SalaryPeriods
                     .Where(f => (param.DateStart <= f.END_DATE && param.DateStart >= f.START_DATE) || (param.DateEnd >= f.START_DATE && param.DateEnd <= f.END_DATE))
                     .CountAsync();
                if (p > 0)
                {
                    return new ResultWithError("DATA_EXIST");
                }

                var data = Map(param, new AT_SALARY_PERIOD());
                data.IS_ACTIVE = true;
                var result = await _appContext.SalaryPeriods.AddAsync(data);
                await _appContext.SaveChangesAsync();
                if (param.Dtl.Count > 0)
                {
                    var lst = new List<AT_SALARY_PERIOD_DTL>();
                    foreach (var item in param.Dtl)
                    {
                        var dtl = Map(item, new AT_SALARY_PERIOD_DTL());
                        dtl.PERIOD_ID = data.ID;
                        lst.Add(dtl);
                    }
                    await _appContext.SalaryPeriodDtls.AddRangeAsync(lst);
                    await _appContext.SaveChangesAsync();
                }
                if (param.DtlEmp.Count > 0)
                {
                    var lst = new List<AT_SALARY_PERIOD_DTL>();
                    foreach (var item in param.DtlEmp)
                    {
                        var dtl = Map(item, new AT_SALARY_PERIOD_DTL());
                        dtl.PERIOD_ID = data.ID;
                        lst.Add(dtl);
                    }
                    await _appContext.SalaryPeriodDtls.AddRangeAsync(lst);
                    await _appContext.SaveChangesAsync();
                }
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(SalaryPeriodInputDTO param)
        {
            try
            {
                var r = _appContext.SalaryPeriods.Where(x => x.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                // check ky công lương
                var p = await _appContext.SalaryPeriods
                    .Where(f => f.ID != r.ID && ((param.DateStart <= f.END_DATE && param.DateStart >= f.START_DATE) || (param.DateEnd >= f.START_DATE && param.DateEnd <= f.END_DATE)))
                    .CountAsync();
                if (p > 0)
                {
                    return new ResultWithError("DATA_EXIST");
                }
                var data = Map(param, r);
                var result = _appContext.SalaryPeriods.Update(data);
                var dtl = await _appContext.SalaryPeriodDtls.Where(x => x.PERIOD_ID == param.Id).ToListAsync();
                if (dtl.Count > 0)
                {
                    _appContext.SalaryPeriodDtls.RemoveRange(dtl);
                    await _appContext.SaveChangesAsync();
                }
                if (param.Dtl.Count > 0)
                {
                    var lst = new List<AT_SALARY_PERIOD_DTL>();
                    foreach (var item in param.Dtl)
                    {
                        var dtl1 = Map(item, new AT_SALARY_PERIOD_DTL());
                        dtl1.PERIOD_ID = data.ID;
                        lst.Add(dtl1);
                    }
                    await _appContext.SalaryPeriodDtls.AddRangeAsync(lst);
                }

                if (param.DtlEmp.Count > 0)
                {
                    var lst = new List<AT_SALARY_PERIOD_DTL>();
                    foreach (var item in param.DtlEmp)
                    {
                        var dtl1 = Map(item, new AT_SALARY_PERIOD_DTL());
                        dtl1.PERIOD_ID = data.ID;
                        lst.Add(dtl1);
                    }
                    await _appContext.SalaryPeriodDtls.AddRangeAsync(lst);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// CMS Change Status Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var r = _appContext.SalaryPeriods.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.SalaryPeriods.Update(r);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Get List Group is Activce
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetList(int? Id)
        {
            try
            {
                var queryable = await (from p in _appContext.SalaryPeriods
                                       where p.IS_ACTIVE == true && p.YEAR == Id
                                       orderby p.START_DATE descending
                                       select new
                                       {
                                           Id = p.ID,
                                           Name = p.NAME,
                                           DateStart = p.START_DATE,
                                           DateEnd = p.END_DATE,
                                           StandartWorking = p.STANDARD_WORKING,
                                           /*StandardTime = p.STANDARD_TIME,*/
                                           Month = p.MONTH
                                       }).ToListAsync();
                return new ResultWithError(queryable);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> GetYear()
        {
            try
            {
                var queryable = await (from p in _appContext.SalaryPeriods
                                       where p.IS_ACTIVE == true
                                       orderby p.YEAR descending
                                       select new
                                       {
                                           Id = p.YEAR,
                                           Name = p.YEAR
                                       }).Distinct().ToListAsync();
                return new ResultWithError(queryable);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> PortalGetYear()
        {
            try
            {
                var queryable = await (from p in _appContext.SalaryPeriods
                                       where p.IS_ACTIVE == true
                                       orderby p.YEAR descending
                                       select new
                                       {
                                           Year = p.YEAR
                                       }).Distinct().ToListAsync();
                return new ResultWithError(queryable);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> PortalByYear(int year)
        {
            try
            {
                var queryable = await (from p in _appContext.SalaryPeriods
                                       where p.IS_ACTIVE == true && p.YEAR == year
                                       orderby p.START_DATE descending
                                       select new
                                       {
                                           Id = p.ID,
                                           Month = p.START_DATE.Month
                                       }).ToListAsync();
                return new ResultWithError(queryable);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
