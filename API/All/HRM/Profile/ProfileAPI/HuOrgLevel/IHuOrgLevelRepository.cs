using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuOrgLevel
{
    public interface IHuOrgLevelRepository: IGenericRepository<HU_ORG_LEVEL, HuOrgLevelDTO>
    {
       Task<GenericPhaseTwoListResponse<HuOrgLevelDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuOrgLevelDTO> request);
    }
}

