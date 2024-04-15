using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;

namespace API.Controllers.AtSignDefaultImport
{
    public interface IAtSignDefaultImportRepository
    {
        Task<GenericPhaseTwoListResponse<AtSignDefaultImportDTO>> SinglePhaseQueryList(
           GenericQueryListDTO<AtSignDefaultImportDTO> request
           );
        Task<FormatedResponse> Save(ImportQueryListBaseDTO request);
    }
}
