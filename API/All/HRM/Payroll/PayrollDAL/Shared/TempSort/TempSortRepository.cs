using PayrollDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public class TempSortRepository : RepositoryBase<PA_KPI_FORMULA>
    {
        private PayrollDbContext _appContext => (PayrollDbContext)_context;
        public TempSortRepository(PayrollDbContext context) : base(context)
        {

        }
        

        /// <summary>
        /// CMS Edit Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(List<TempSortInputDTO> param, int type)
        {
            try
            {
                var lst = new List<SYS_TMP_SORT>();
                foreach (var item in param)
                {
                    var data = Map(item, new SYS_TMP_SORT());
                    //data.TYPE_ID = type;
                    //
                    lst.Add(data);
                }

                await _appContext.SysTempSorts.AddRangeAsync(lst);
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
