using API.DTO;

namespace API.All.SYSTEM.CoreAPI.ExportFile
{
    public interface IExportFileRepository
    {
        Task<FileReturnDTO> ExportExel(ExportExelRequest request);
    }
}
