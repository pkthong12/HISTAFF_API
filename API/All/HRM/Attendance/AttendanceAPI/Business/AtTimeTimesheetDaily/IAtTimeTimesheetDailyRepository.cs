using API.All.HRM.Attendance.AttendanceAPI.Business.AtTimeTimesheetDaily;
using API.DTO;
using AttendanceDAL.ViewModels;
using Common.Extensions;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.AtTimeTimesheetDaily
{
    public interface IAtTimeTimesheetDailyRepository : IGenericRepository<AT_TIME_TIMESHEET_DAILY, AtTimeTimesheetDailyDTO>
    {
        Task<FormatedResponse> SinglePhaseQueryList(GenericQueryListDTO<AtTimeTimesheetDailyDTO> request);
        Task<FormatedResponse> GetListTimeSheet(AtTimeTimesheetDailyDTO param, AppSettings appSettings);
        Task<FormatedResponse> GetByImportEdit(long id);

        Task<FormatedResponse>  UpdateImportEdit(GenericUnitOfWork _uow, AtTimesheetDailyImportDTO dto, string sid);
        Task<ResultWithError> Calculate(AtTimeTimesheetDailyDTO request);
        Task<ResultWithError> Confirm(AtTimeTimesheetDailyDTO request);
    }
}

