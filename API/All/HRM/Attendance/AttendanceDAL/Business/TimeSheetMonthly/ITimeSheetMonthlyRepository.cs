using AttendanceDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using System.Dynamic;
using API.Entities;
using CORE.DTO;

namespace AttendanceDAL.Repositories
{
    public interface ITimeSheetMonthlyRepository : IRepository<AT_TIMESHEET_MONTHLY>
    {
        Task<GenericPhaseTwoListResponse<TimeSheetMonthlyDTO>> SinglePhaseQueryList(GenericQueryListDTO<TimeSheetMonthlyDTO> request);
        Task<ResultWithError> GetByEmployeeId(long empId);
        Task<ResultWithError> Calculate(TimeSheetInputDTO request);
        Task<ResultWithError> Lock(TimeSheetInputDTO request);
        Task<ResultWithError> CheckLock(LockDataInput request);
        Task<ResultWithError> ImportSwipeMachine(List<SwipeDataInput> param);
        ///// <summary>
        ///// getall bang cong goc
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //Task<PagedResult<ExpandoObject>> ListTimeSheetMonthly(TimeSheetMonthlyDTO param);
        ///// <summary>
        ///// get list formula cho grid ben phai
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //Task<PagedResult<TimeSheetFomulaDTO>> GetListFormula(TimeSheetFomulaDTO param);
        //Task<ResultWithError> UpdateFormula(TimeSheetFomulaInputDTO param);
        //Task<ResultWithError> SumWork(TimeSheetInputDTO param);

        ///// <summary>
        ///// getall bang cong goc
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //Task<ResultWithError> ListSwipeData(SwipeDataDTO param);
        ///// <summary>
        ///// Import Dư liệu chấm công tư máy chấm công
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //Task<ResultWithError> ImportSwipeData(List<SwipeDataInput> param);
        //Task<ResultWithError> ImportSwipeDataNew(SwipeImportnput param);
        ///// <summary>
        ///// Khóa bảng công
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //Task<ResultWithError> LockTimeSheet(TimeSheetLockInputDTO param);
        ///// <summary>
        ///// Check time sheet lock
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //Task<ResultWithError> IsLockTimeSheet(TimeSheetLockInputDTO param);
        ///// <summary>
        ///// Portal Get By Id
        ///// </summary>
        ///// <param name="periodId"></param>
        ///// <returns></returns>
        //Task<ResultWithError> PortalGetBY(int periodId);
        //Task<ResultWithError> UpdateTimeSheetMachine(MaChineInput param);
        //Task<PagedResult<ExpandoObject>> ListEntitlement(EntitlementDTO param);
        //Task<ResultWithError> ReportSwipeData(SwipeDataReport param);
        //Task<ResultWithError> ReportSwipeDataExp(SwipeDataReport param);
        //Task<ResultWithError> CalEntitlement(TimeSheetInputDTO param);
        //Task<ResultWithError> ReadMCC(TimeSheetInputDTO param);

    }
}
