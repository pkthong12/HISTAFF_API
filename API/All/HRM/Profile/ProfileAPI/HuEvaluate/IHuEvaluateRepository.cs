using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuEvaluate
{
    public interface IHuEvaluateRepository: IGenericRepository<HU_EVALUATE, HuEvaluateDTO>
    {
       Task<GenericPhaseTwoListResponse<HuEvaluateDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEvaluateDTO> request);
    }
}

