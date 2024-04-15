using Common.Extensions;
using PayrollDAL.ViewModels;


namespace PayrollDAL.Repositories
{
    public interface IReportRepository 
    {
        Task<ResultWithError> PA001(ParaInputReport param);
        Task<ResultWithError> PA002(ParaInputReport param);
        Task<ResultWithError> ThuNhapBinhQuan(int? orgId);
        Task<ResultWithError> CoCauNhanSu(CCNSParam param);
        Task<ResultWithError> TongHopQuyLuong(CCNSParam param);

    }
}
