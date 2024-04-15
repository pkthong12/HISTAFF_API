using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;

namespace API.All.HRM.Profile.ProfileAPI.HuEmployeeImport
{
    public interface IHuEmployeeImport
    {
        Task<GenericPhaseTwoListResponse<HuEmployeeImportDTO>> SinglePhaseQueryList(
            GenericQueryListDTO<HuEmployeeImportDTO> request
            );
        Task<FormatedResponse> Save(ImportQueryListBaseDTO request);
    }
}
