using API.All.SYSTEM.CoreAPI.Report;
using CORE.DTO;
using CORE.GenericUOW;
using Microsoft.AspNetCore.Mvc;

namespace API.All.HRM.XlsxReport
{
    public interface IXlsxReportRepository
    {
        Task<FormatedResponse> GetListReport();

        Task<MemoryStream> GetReport(ReportDTO request, string location, string sid, string username);
    }
}
