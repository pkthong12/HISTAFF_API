using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Training.TrainingAPI.TrAssessmentResult
{
    public interface ITrAssessmentResultRepository : IGenericRepository<TR_ASSESSMENT_RESULT, TrAssessmentResultDTO>
    {
        Task<GenericPhaseTwoListResponse<TrAssessmentResultDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrAssessmentResultDTO> request);
        Task<FormatedResponse> GetDropDownEducationProgram();
    }
}