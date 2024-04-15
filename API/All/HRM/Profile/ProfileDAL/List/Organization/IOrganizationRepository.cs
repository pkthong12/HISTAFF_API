using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IOrganizationRepository : IRepository<HU_ORGANIZATION>
    {
        Task<PagedResult<OrganizationInputDTO>> GetAll(OrganizationInputDTO param);
        Task<ResultWithError> GetTreeView();
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(OrganizationInputDTO param);
        Task<ResultWithError> UpdateAsync(OrganizationInputDTO param);
        Task<ResultWithError> GetList();
        Task<ResultWithError> Delete(long id);
        Task<ResultWithError> SortAsync(OrganizationInputDTO param);
        Task<ResultWithError> GetAllOrgChartPosition(OrgChartRptInputDTO param);
        Task<ResultWithError> GetJobPosTree(JobPositionTreeInputDTO param);
        Task<ResultWithError> UpdateCreateRptJobPosHisAsync(JobPositionTreeInputDTO param);
        Task<ResultWithError> GetJobChildTree(JobChildTreeInputDTO param);
    }
}
