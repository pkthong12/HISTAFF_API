using Common.Paging;
using PayrollDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public class KpiPositionRepository : RepositoryBase<PA_KPI_POSITION>, IKpiPositionRepository
    {
        private PayrollDbContext _appContext => (PayrollDbContext)_context;
        public KpiPositionRepository(PayrollDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<KpiPositionDTO>> GetAll(KpiPositionDTO param)
        {
            var queryable = from p in _appContext.KpiPositions
                            join t in _appContext.KpiTargets on p.KPI_TARGET_ID equals t.ID
                            join g in _appContext.KpiGroups on t.KPI_GROUP_ID equals g.ID
                            
                            orderby t.NAME
                            select new KpiPositionDTO
                            {
                                Id = p.ID,
                                KpiGroupId = g.ID,
                                KpiTargetId = t.ID,
                                KpiTargetName = t.NAME,
                                KpiGroupName = g.NAME,
                                PositionId = p.POSITION_ID,
                            };

           
            if (!string.IsNullOrWhiteSpace(param.KpiGroupName))
            {
                queryable = queryable.Where(p => p.KpiGroupName.ToUpper().Contains(param.KpiGroupName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.KpiTargetName))
            {
                queryable = queryable.Where(p => p.KpiTargetName.ToUpper().Contains(param.KpiTargetName.ToUpper()));
            }
            if (param.PositionId != null)
            {
                queryable = queryable.Where(p => p.PositionId == param.PositionId);
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
                var r = await (from p in _appContext.KpiPositions
                               join t in _appContext.KpiTargets on p.KPI_TARGET_ID equals t.ID
                               join po in _appContext.Positions on p.POSITION_ID equals po.ID
                               join g in _appContext.KpiGroups on t.KPI_GROUP_ID equals g.ID
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   KpiGroupId = g.ID,
                                   KpiTargetId = t.ID,
                                   KpiTargetName = t.NAME,
                                   KpiGroupName = g.NAME,
                                   PositionId = po.ID,
                                   PositionName = po.NAME,
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }


        public async Task<ResultWithError> CreateAsync(KpiPositionInputDTO param)
        {
            try
            {


                var g2 = _appContext.Positions.Where(c => c.ID == param.PositionId).Count();
                if (g2 == 0)
                {
                    return new ResultWithError(Message.RECORD_NOT_FOUND);
                }
                var list = new List<PA_KPI_POSITION>();
                foreach (var item in param.KpiTargetId)
                {
                    var g3 = _appContext.KpiPositions.Where(c => c.KPI_TARGET_ID == item && c.POSITION_ID == param.PositionId).Count();
                    if (g3 == 0)
                    {
                        var x = new PA_KPI_POSITION();
                        x.KPI_TARGET_ID = item;
                        x.POSITION_ID = param.PositionId;
                        list.Add(x);
                    }
                }
                await _appContext.KpiPositions.AddRangeAsync(list);
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
        public async Task<ResultWithError> Removes(List<long> ids)
        {
            try
            {
                var list = await _appContext.KpiPositions.Where(c => ids.Contains(c.ID)).ToArrayAsync();
                _appContext.KpiPositions.RemoveRange(list);
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
