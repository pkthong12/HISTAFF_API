using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;

namespace API.All.HRM.Profile.ProfileAPI.HuWorkingHslPcImport
{
    public interface IHuWorkingHslPcImport
    {
        Task<GenericPhaseTwoListResponse<HuWorkingHslPcImportDTO>> SinglePhaseQueryList(
            GenericQueryListDTO<HuWorkingHslPcImportDTO> request
            );
        Task<FormatedResponse> Save(ImportQueryListBaseDTO request);
    }
}
