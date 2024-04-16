using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Training.TrainingAPI.TrResultEvaluation
{
    public interface ITrResultEvaluationRepository : IGenericRepository<TR_RESULT_EVALUATION, TrResultEvaluationDTO>
    {
        Task<GenericPhaseTwoListResponse<TrResultEvaluationDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrResultEvaluationDTO> request);
        Task<GenericPhaseTwoListResponse<TrResultEvaluationDTO>> SinglePhaseQueryListForEmployee(GenericQueryListDTO<TrResultEvaluationDTO> request);
    }
}