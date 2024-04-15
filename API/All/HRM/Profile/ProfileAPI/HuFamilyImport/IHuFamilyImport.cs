using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;

namespace API.All.HRM.Profile.ProfileAPI.HuFamilyImport
{
    public interface IHuFamilyImport
    {
        Task<GenericPhaseTwoListResponse<HuFamilyImportDTO>> SinglePhaseQueryList(
            GenericQueryListDTO<HuFamilyImportDTO> request
            );
        Task<FormatedResponse> Save(ImportQueryListBaseDTO request);
    }
}