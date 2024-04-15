using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;

namespace API.All.HRM.Profile.ProfileAPI.HuCertificateImport
{
    public interface IHuCertificateImportRepository
    {
        Task<GenericPhaseTwoListResponse<HuCertificateImportDTO>> SinglePhaseQueryList(
            GenericQueryListDTO<HuCertificateImportDTO> request
            );
        Task<FormatedResponse> Save(ImportQueryListBaseDTO request);
    }
}
