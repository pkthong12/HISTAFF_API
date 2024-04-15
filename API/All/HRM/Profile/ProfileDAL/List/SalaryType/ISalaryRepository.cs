using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface ISalaryRepository : IRepository<HU_SALARY_TYPE>
    {
        Task<PagedResult<SalaryTypeDTO>> GetAll(SalaryTypeDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList();
    }
}
