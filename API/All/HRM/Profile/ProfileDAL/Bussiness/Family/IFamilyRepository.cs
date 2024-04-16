using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.Family
{
    public interface IFamilyRepository: IGenericRepository<HU_FAMILY, HuFamilyDTO>
    {
        Task<GenericPhaseTwoListResponse<HuFamilyDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuFamilyDTO> request);
    }
}

