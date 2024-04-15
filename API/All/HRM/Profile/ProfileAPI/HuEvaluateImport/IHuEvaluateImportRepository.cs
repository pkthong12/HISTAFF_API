using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuEvaluate
{
    public interface IHuEvaluateImportRepository
    {
        Task<GenericPhaseTwoListResponse<HuEvaluateImportDTO>> SinglePhaseQueryList(
             GenericQueryListDTO<HuEvaluateImportDTO> request
             );
        Task<GenericPhaseTwoListResponse<HuEvaluateConcurrentDTO>> EvaluateConcurrentQueryList(
             GenericQueryListDTO<HuEvaluateConcurrentDTO> request
             );
        Task<FormatedResponse> Save(ImportQueryListBaseDTO request);
        Task<FormatedResponse> EvaluateConcurrentSave(ImportQueryListBaseDTO request);
    }
}

