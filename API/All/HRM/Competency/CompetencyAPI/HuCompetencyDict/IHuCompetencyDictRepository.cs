using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuCompetencyDict
{
    public interface IHuCompetencyDictRepository: IGenericRepository<HU_COMPETENCY_DICT, HuCompetencyDictDTO>
    {
       Task<GenericPhaseTwoListResponse<HuCompetencyDictDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCompetencyDictDTO> request);
    }
}

