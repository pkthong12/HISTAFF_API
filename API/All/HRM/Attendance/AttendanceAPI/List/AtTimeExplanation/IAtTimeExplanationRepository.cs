using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.AtTimeExplanation
{
    public interface IAtTimeExplanationRepository: IGenericRepository<AT_TIME_EXPLANATION, AtTimeExplanationDTO>
    {
       Task<GenericPhaseTwoListResponse<AtTimeExplanationDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtTimeExplanationDTO> request);
        Task<FormatedResponse> GetListSalaryPeriod();
    }
}

