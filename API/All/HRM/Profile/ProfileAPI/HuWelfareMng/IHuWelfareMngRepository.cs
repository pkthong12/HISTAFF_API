using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuWelfareMng
{
    public interface IHuWelfareMngRepository: IGenericRepository<HU_WELFARE_MNG, HuWelfareMngDTO>
    {
        Task<GenericPhaseTwoListResponse<HuWelfareMngDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuWelfareMngDTO> request);
    }
}

