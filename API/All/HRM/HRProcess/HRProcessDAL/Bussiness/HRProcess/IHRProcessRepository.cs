using API.Entities;
using Common.Interfaces;
using Common.Paging;
using HRProcessDAL.ViewModels;

namespace HRProcessDAL.Repositories
{
    public interface IHRProcessRepository : IRepository<SE_HR_PROCESS_TYPE>
    {
        Task<PagedResult<SeHrProcessTypeDTO>> GetAll(SeHrProcessTypeDTO param);
       
    }
}
