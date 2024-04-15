using API.Entities;
using Common.Extensions;
using Common.Interfaces;
using Common.Paging;
using CoreDAL.ViewModels;

namespace CoreDAL.Repositories
{
    public interface IApproveTemplateRepository : IRepository<SE_APP_TEMPLATE>
    {
        Task<PagedResult<ApproveTemplateDTO>> GetApproveTemplate(ApproveTemplateDTO param);
        Task<ResultWithError> GetApproveTemplateById(long id);
        Task<ResultWithError> CreateApproveTemplate(ApproveTemplateDTO param);
        Task<ResultWithError> UpdateApproveTemplate(ApproveTemplateDTO param);
        Task<ResultWithError> DeleteApproveTemplate(List<long> ids);
        Task<PagedResult<ApproveTemplateDetailDTO>> GetApproveTemplateDetail(int templateId);
        Task<ResultWithError> GetApproveTemplateDetailById(long id);
        Task<ResultWithError> CreateApproveTemplateDetail(ApproveTemplateDetailDTO param);
        Task<ResultWithError> UpdateApproveTemplateDetail(ApproveTemplateDetailDTO param);
        Task<ResultWithError> DeleteApproveTemplateDetail(List<long> ids);
        Task<ResultWithError> GetListPosition();
    }
}
