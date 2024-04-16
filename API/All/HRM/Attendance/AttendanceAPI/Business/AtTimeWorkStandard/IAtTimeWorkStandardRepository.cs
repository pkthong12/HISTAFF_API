using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Attendance.AttendanceAPI.Business.AtTimeWorkStandard
{
    public interface IAtTimeWorkStandardRepository : IGenericRepository<AT_TIME_WORK_STANDARD, AtTimeWorkStandardDTO>
    {
        Task<GenericPhaseTwoListResponse<AtTimeWorkStandardDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtTimeWorkStandardDTO> request);
    }
}