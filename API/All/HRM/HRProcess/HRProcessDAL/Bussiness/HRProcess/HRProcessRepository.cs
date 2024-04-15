using Common.Paging;
using HRProcessDAL.ViewModels;
using Common.Repositories;
using API.All.DbContexts;
using API.Entities;

namespace HRProcessDAL.Repositories
{
    public class HRProcessRepository : RepositoryBase<SE_HR_PROCESS_TYPE>, IHRProcessRepository
    {
        private HRProcessDbContext _appContext => (HRProcessDbContext)_context;
        public HRProcessRepository(HRProcessDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<SeHrProcessTypeDTO>> GetAll(SeHrProcessTypeDTO param)
        {

            var queryable = from p in _appContext.SeHrProcessType
                            
                            orderby p.CODE
                            select new SeHrProcessTypeDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME
                            };

            return await PagingList(queryable, param);
        }

       
    }
}
