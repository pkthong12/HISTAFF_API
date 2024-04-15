using Common.Paging;
using PayrollDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;
using CORE.GenericUOW;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;

namespace PayrollDAL.Repositories
{
    public class KpiGroupRepository : RepositoryBase<PA_KPI_GROUP>, IKpiGroupRepository
    {
        private PayrollDbContext _appContext => (PayrollDbContext)_context;
        private readonly GenericReducer<PA_KPI_GROUP, KpiGroupDTO> genericReducer;
        public KpiGroupRepository(PayrollDbContext context) : base(context)
        {
            genericReducer = new();
        }
        public async Task<GenericPhaseTwoListResponse<KpiGroupDTO>> TwoPhaseQueryList(GenericQueryListDTO<KpiGroupDTO> request)
        {
            var raw = _appContext.KpiGroups.AsQueryable();
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

            var joined = from p in _appContext.KpiGroups
                         where p.IS_ACTIVE == true
                         select new KpiGroupDTO
                         {
                             Id = p.ID,
                             Name = p.NAME,
                             Orders = p.ORDERS,
                             Note = p.NOTE
                         };
            var phase2 = await genericReducer.SecondPhaseReduce(joined, request);
            return phase2;
        }
        /// <summary>
        /// CMS Get All Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<KpiGroupOutDTO>> GetAll(KpiGroupDTO param)
        {
            var queryable = from p in _appContext.KpiGroups
                            where p.IS_ACTIVE == true
                            select new KpiGroupOutDTO
                            {
                                Id = p.ID,
                                Name = p.NAME,
                                Orders = p.ORDERS,
                                Note = p.NOTE
                            };

            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Note))
            {
                queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
            }
            return await PagingList(queryable, param);
        }
        /// <summary>
        /// CMS Get Detail for System
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetById(long id)
        {
            try
            {
                var r = await (from p in _appContext.KpiGroups
                               where p.ID == id 
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                                   Orders = p.ORDERS,
                                   Note = p.NOTE,
                                   IsActive = p.IS_ACTIVE,
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
       
        public async Task<ResultWithError> GetList()
        {
            try
            {
                var r = await (from p in _appContext.KpiGroups
                               where p.IS_ACTIVE == true 
                               orderby p.ORDERS
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Create Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(KpiGroupInputDTO param)
        {
            try
            {
                var data = Map(param, new PA_KPI_GROUP());
                data.IS_ACTIVE = true;
                var result = await _appContext.KpiGroups.AddAsync(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// CMS Edit Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(KpiGroupInputDTO param)
        {
            try
            {

                var r = _appContext.KpiGroups.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }

                var data = Map(param, r);
                var result = _appContext.KpiGroups.Update(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
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
                    var r = _appContext.KpiGroups.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.KpiGroups.Update(r);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }
    }
}
