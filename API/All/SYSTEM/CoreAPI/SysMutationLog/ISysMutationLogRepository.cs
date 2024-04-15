using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysMutationLog
{
    public interface ISysMutationLogRepository: IGenericRepository<SYS_MUTATION_LOG, SysMutationLogDTO>
    {
       Task<GenericPhaseTwoListResponse<SysMutationLogDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysMutationLogDTO> request);
    }
}

