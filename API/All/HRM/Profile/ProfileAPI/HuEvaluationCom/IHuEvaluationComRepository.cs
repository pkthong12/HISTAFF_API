using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Profile.ProfileAPI.HuEvaluationCom
{
    public interface IHuEvaluationComRepository : IGenericRepository<HU_EVALUATION_COM, HuEvaluationComDTO>
    {
        Task<GenericPhaseTwoListResponse<HuEvaluationComDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEvaluationComDTO> request);
    }
}
