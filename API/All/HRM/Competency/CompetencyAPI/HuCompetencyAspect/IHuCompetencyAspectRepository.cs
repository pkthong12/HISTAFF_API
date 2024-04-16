using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuCompetencyAspect
{
    public interface IHuCompetencyAspectRepository: IGenericRepository<HU_COMPETENCY_ASPECT, HuCompetencyAspectDTO>
    {
       Task<GenericPhaseTwoListResponse<HuCompetencyAspectDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCompetencyAspectDTO> request);
        Task<FormatedResponse> CreateNewCode();
        Task<FormatedResponse> GetList();


    }
}

