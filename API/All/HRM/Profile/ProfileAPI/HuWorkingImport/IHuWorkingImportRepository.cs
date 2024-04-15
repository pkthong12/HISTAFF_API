using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuEvaluate
{
    public interface IHuWorkingImportRepository
    {
        Task<GenericPhaseTwoListResponse<HuWorkingImportDTO>> SinglePhaseQueryList(
             GenericQueryListDTO<HuWorkingImportDTO> request
             );
        Task<FormatedResponse> Save(ImportQueryListBaseDTO request);
    }
}

