using PayrollDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public interface IAdvanceRepository : IRepository<PA_ADVANCE>
    {
        Task<PagedResult<AdvanceDTO>> GetAll(AdvanceDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(AdvanceInputDTO param);
        Task<ResultWithError> UpdateAsync(AdvanceInputDTO param);
        Task<ResultWithError> Delete(List<long> ids);
        Task<ResultWithError> TemplateImport(AdvanceTmpParam param);
        Task<ResultWithError> ImportTemplate(AdvanceTmpParam param);
    }
}
