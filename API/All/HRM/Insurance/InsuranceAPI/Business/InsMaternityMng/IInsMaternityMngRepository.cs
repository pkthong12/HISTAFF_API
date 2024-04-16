using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.InsMaternityMng
{
    public interface IInsMaternityMngRepository: IGenericRepository<INS_MATERNITY_MNG, InsMaternityMngDTO>
    {
       Task<GenericPhaseTwoListResponse<InsMaternityMngDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsMaternityMngDTO> request);
    }
}

