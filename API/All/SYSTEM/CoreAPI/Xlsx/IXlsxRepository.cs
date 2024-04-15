using CORE.DTO;

namespace API.All.SYSTEM.CoreAPI.Xlsx
{
    public interface IXlsxRepository
    {
        Task<MemoryStream> ExportCorePageListGridToExcel(ExportCorePageListGridToExcelDTO request, string location, string sid, string username);
        Task<MemoryStream> DownloadTemplate(DownloadTemplateDTO request, string location, string sid);
        Task<MemoryStream> GenerateTemplate(ExObject request, string location, string sid, string username);
        Task<MemoryStream> ImportXlsxToDb(ImportXlsxToDbDTO request, string location, string sid, string username);

    }
}
