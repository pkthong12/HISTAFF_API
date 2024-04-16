using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuCompetency
{
    public interface IHuCompetencyRepository: IGenericRepository<HU_COMPETENCY, HuCompetencyDTO>
    {
       Task<GenericPhaseTwoListResponse<HuCompetencyDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCompetencyDTO> request);
        Task<FormatedResponse> CreateNewCode();
        Task<FormatedResponse> GetList();


    }
}

