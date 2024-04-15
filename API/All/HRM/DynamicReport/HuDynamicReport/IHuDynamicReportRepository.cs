using API.All.HRM.DynamicReport.HuDynamicReport;
using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuDynamicReport
{
    public interface IHuDynamicReportRepository : IGenericRepository<HU_DYNAMIC_REPORT, HuDynamicReportDTO>
    {
        Task<GenericPhaseTwoListResponse<HuDynamicReportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuDynamicReportDTO> request);
        //Task<FormatedResponse> GetAllByFid(long? fid);

        Task<FormatedResponse> GetViewList();
        Task<FormatedResponse> GetColumnList(GetColumnListRequest request);
        Task<FormatedResponse> ReadAllByViewName(string? viewName);
        Task<MemoryStream> GetListByConditionToExport(DynamicReportDTO? request1);
        //Task<MemoryStream> ExportExelDynamicReport(DynamicReportDTO? request);
    }
}

