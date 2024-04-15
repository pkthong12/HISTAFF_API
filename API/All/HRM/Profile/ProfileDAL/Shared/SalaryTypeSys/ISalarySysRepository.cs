using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface ISalarySysRepository : IRepository<SYS_SALARY_TYPE>
    {
        Task<PagedResult<SalaryTypeSysDTO>> GetAll(SalaryTypeSysDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(SalaryTypeSysInputDTO param);
        Task<ResultWithError> UpdateAsync(SalaryTypeSysInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList(long areaId);
    }
}
