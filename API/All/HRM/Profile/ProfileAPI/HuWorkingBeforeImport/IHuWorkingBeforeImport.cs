using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;

namespace API.All.HRM.Profile.ProfileAPI.HuWorkingBeforeImport
{
    public interface IHuWorkingBeforeImport
    {
        Task<GenericPhaseTwoListResponse<HuWorkingBeforeImportDTO>> SinglePhaseQueryList(
            GenericQueryListDTO<HuWorkingBeforeImportDTO> request
            );
        Task<FormatedResponse> Save(ImportQueryListBaseDTO request);
    }
}
