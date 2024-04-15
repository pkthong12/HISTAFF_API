using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface ISalaryLevelRepository : IRepository<HU_SALARY_LEVEL>
    {
        Task<PagedResult<SalaryLevelDTO>> GetAll(SalaryLevelDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(SalaryLevelInputDTO param);
        Task<ResultWithError> UpdateAsync(SalaryLevelInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList(int rankId);
    }
}
