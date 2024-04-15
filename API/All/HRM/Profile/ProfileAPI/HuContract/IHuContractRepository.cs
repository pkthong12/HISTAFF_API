using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuContract
{
    public interface IHuContractRepository: IGenericRepository<HU_CONTRACT, HuContractDTO>
    {
       Task<GenericPhaseTwoListResponse<HuContractDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuContractDTO> request);
    }
}

