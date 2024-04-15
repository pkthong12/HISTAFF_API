using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;

namespace API.All.HRM.Profile.ProfileAPI.HuAllowanceEmpImport
{
    public interface IHuAllowanceEmpImportRepository
    {
        Task<GenericPhaseTwoListResponse<HuAllowanceEmpImportDTO>> SinglePhaseQueryList(
           GenericQueryListDTO<HuAllowanceEmpImportDTO> request);
        Task<FormatedResponse> Save(ImportQueryListBaseDTO request);
    }
}
