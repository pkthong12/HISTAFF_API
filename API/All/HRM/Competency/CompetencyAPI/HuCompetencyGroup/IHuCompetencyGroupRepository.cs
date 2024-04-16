using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuCompetencyGroup
{
    public interface IHuCompetencyGroupRepository: IGenericRepository<HU_COMPETENCY_GROUP, HuCompetencyGroupDTO>
    {
       Task<GenericPhaseTwoListResponse<HuCompetencyGroupDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCompetencyGroupDTO> request);
        Task<FormatedResponse> CreateNewCode();
        Task<FormatedResponse> GetList();


    }
}

