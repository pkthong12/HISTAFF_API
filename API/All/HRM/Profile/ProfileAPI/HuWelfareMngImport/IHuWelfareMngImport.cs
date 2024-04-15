using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;

namespace API.All.HRM.Profile.ProfileAPI.HuWelfareMngImport
{
    public interface IHuWelfareMngImport
    {
        Task<GenericPhaseTwoListResponse<HuWelfareMngImportDTO>> SinglePhaseQueryList(
            GenericQueryListDTO<HuWelfareMngImportDTO> request
            );
        Task<FormatedResponse> Save(ImportQueryListBaseDTO request);
    }
}
