using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysFunctionIgnore
{
    public interface ISysFunctionIgnoreRepository : IGenericRepository<SYS_FUNCTION_IGNORE, SysFunctionIgnoreDTO>
    {
        Task<GenericPhaseTwoListResponse<SysFunctionIgnoreDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysFunctionIgnoreDTO> request);
        Task<FormatedResponse> ReadAllPathOnly();
    }
}

