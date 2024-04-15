using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using ProfileDAL.ViewModels;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IInsChangeRepository : IRepository<INS_CHANGE>
    {
        Task<PagedResult<InsChangeDTO>> GetAll(InsChangeDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(InsChangeInputDTO param);
        Task<ResultWithError> UpdateAsync(InsChangeInputDTO param);
        Task<ResultWithError> RemoveAsync(List<long> id);
        Task<ResultWithError> TemplateImport(long orgId);
        Task<ResultWithError> ImportTemplate(ImportInsParam param);
        Task<ResultWithError> PortalGetAll();
    }
}
