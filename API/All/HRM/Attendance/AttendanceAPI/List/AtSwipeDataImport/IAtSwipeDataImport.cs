using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;

namespace API.All.HRM.Attendance.AttendanceAPI.List.AtSwipeDataImport
{
    public interface IAtSwipeDataImport
    {
        Task<GenericPhaseTwoListResponse<AtSwipeDataImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtSwipeDataImportDTO> request);
        Task<FormatedResponse> Save(ImportQueryListBaseDTO request);
    }
}
