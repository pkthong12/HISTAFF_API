using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuEvaluate
{
    public interface IHuEvaluationComImportRepository
    {
        Task<GenericPhaseTwoListResponse<HuEvaluationComImportDTO>> SinglePhaseQueryList(
             GenericQueryListDTO<HuEvaluationComImportDTO> request
             );
        Task<FormatedResponse> Save(ImportQueryListBaseDTO request);
    }
}

