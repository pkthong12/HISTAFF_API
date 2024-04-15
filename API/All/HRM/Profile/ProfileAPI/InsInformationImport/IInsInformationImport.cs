using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;

namespace API.All.HRM.Profile.ProfileAPI.InsInformationImport
{
    public interface IInsInformationImport
    {
        Task<GenericPhaseTwoListResponse<InsInformationImportDTO>> SinglePhaseQueryList(
            GenericQueryListDTO<InsInformationImportDTO> request
            );
        Task<FormatedResponse> Save(ImportQueryListBaseDTO request);
    }
}
