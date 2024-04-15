using Common.Extensions;
using CORE.DTO;
using OfficeOpenXml;
using System.Data;

namespace API.All.SYSTEM.CoreAPI.Excel
{
    public interface IExcelRespository
    {
        Task<byte[]> ExportTempImportSalary(string filePath, DataSet dtDataValue, DataTable dtColname);

        Task<byte[]> ExportTempImportBasic(string filePath, DataSet dtDataValue);

        Task<FormatedResponse> ImportTempSalary(long? salObj, long? periodId,long? dateCalId, string base64, List<string> lstColVal, long? recordSuccess, long? year,string typeImport);

        Task<byte[]> ExportTempImportTimeSheet(string filePath, DataSet dtDataValue, long? periodId);
        Task<FormatedResponse> ImportTimeSheetDaily( string base64, long? PeriodID);

        Task<byte[]> CheckValidImportTimeSheet(string errorFilePath, string base64, long? PeriodID);


        Task<byte[]> ExportTempImportShiftSort(string filePath, DataSet dtDataValue, long? periodId);
        Task<FormatedResponse> ImportShiftSort(string base64, long? PeriodID);

        Task<byte[]> CheckValidImportShiftSort(string errorFilePath, string base64, long? PeriodID);


        Task<byte[]> CheckValidImportDeclareSeniority(string errorFilePath, string base64);


        Task<FormatedResponse> ImportDeclareSeniority(string base64);

        Task<FormatedResponse> ImportRegisterLeave(string base64);
        Task<FormatedResponse> ImportRegisterOT(string base64);

        Task<byte[]> CheckValidImportRegisterLeave(string errorFilePath, string base64);


        Task<byte[]> CheckValidImportRegisterOT(string errorFilePath, string base64);


    }
}
