using API.All.HRM.Attendance.AttendanceAPI.Business.AtTimeTimesheetDaily;
using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;
using API.DTO.PortalDTO;

namespace API.Controllers.AtTimeTimesheetDaily
{
    public interface IPortalAtTimeTimesheetDailyRepository: IGenericRepository<AT_TIME_TIMESHEET_DAILY, AtTimeTimesheetDailyDTO>
    {
       Task<GenericPhaseTwoListResponse<AtTimeTimesheetDailyDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtTimeTimesheetDailyDTO> request);
        Task<FormatedResponse> GetAttendantNoteByMonth(GetAttendantNoteByMonthDTO request);
        Task<FormatedResponse> GetAttendatByDay(GetAttendantNoteByMonthDTO request);
        Task<FormatedResponse> GetListSymbolType();
        Task<FormatedResponse> InsertExplainTime(PortalExplainWorkDTO param, AppSettings appSettings);
        Task<FormatedResponse> GetInfoByMonth(GetAttendantNoteByMonthDTO request);
    }
}

