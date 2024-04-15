using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface ISalaryRankRepository : IRepository<HU_SALARY_RANK>
    {
        Task<PagedResult<SalaryRankDTO>> GetAll(SalaryRankDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(SalaryRankInputDTO param);
        Task<ResultWithError> UpdateAsync(SalaryRankInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetListByScale(int? scaleId);
        Task<ResultWithError> GetRankList();
        Task<ResultWithError> GetRankListAll();
        Task<ResultWithError> UpdateLevelStart(SalaryRankStart param);
    }
}
