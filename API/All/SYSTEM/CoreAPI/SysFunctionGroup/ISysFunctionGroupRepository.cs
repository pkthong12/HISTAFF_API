using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysFunctionGroup
{
    public interface ISysFunctionGroupRepository : IGenericRepository<SYS_FUNCTION_GROUP, SysFunctionGroupDTO>
    {
        Task<GenericPhaseTwoListResponse<SysFunctionGroupDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysFunctionGroupDTO> request);
    }
}

