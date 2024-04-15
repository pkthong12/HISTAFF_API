using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IReportRepository : IRepository<HU_REPORT>
    {
        Task<PagedResult<ReportInputDTO>> GetAll(ReportInputDTO param);
        Task<ResultWithError> GetTreeView();
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(ReportInputDTO param);
        Task<ResultWithError> UpdateAsync(ReportInputDTO param);
        Task<ResultWithError> GetList();
        Task<ResultWithError> GetListReport();
        Task<ResultWithError> Delete(long id);
        Task<ResultWithError> ReportIns(ReportInsInputDTO param);
        Task<PagedResult<ReportEmployeeDTO>> ReportEmployee(ReportEmployeeDTO param);
        Task<ResultWithError> ReportInsByOrg(ReportInsByOrgDTO param);
        Task<ResultWithError> REPORT_HU001(ReportParam param);
        Task<ResultWithError> REPORT_HU009(ReportParam param);
    }
}
