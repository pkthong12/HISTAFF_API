using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;

namespace API.All.HRM.Profile.ProfileAPI.HuCommendImport
{
    public interface IHuCommendImportRepository
    {
        Task<GenericPhaseTwoListResponse<HuCommendImportDTO>> SinglePhaseQueryList(
           GenericQueryListDTO<HuCommendImportDTO> request
           );
        Task<FormatedResponse> Save(ImportQueryListBaseDTO request);
    }
}
