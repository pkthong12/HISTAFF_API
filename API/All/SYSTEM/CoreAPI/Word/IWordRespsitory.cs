using System.Data;

namespace API.All.SYSTEM.CoreAPI.Word
{
    public interface IWordRespsitory
    {
        Task<byte[]> ExportFileWord(DataSet dsData, string filePath, string fileImagePath, int left, int top, int height, int width);
        Task<byte[]> ExportWordNoImage(DataSet dsData, string filePath);
    }
}
