using API.All.SYSTEM.CoreAPI.SysFunctionAction;
using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysFunctionAction
{
    public interface ISysFunctionActionRepository : IGenericRepository<SYS_FUNCTION_ACTION, SysFunctionActionDTO>
    {
        Task<GenericPhaseTwoListResponse<SysFunctionActionDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysFunctionActionDTO> request);
        Task<FormatedResponse> UpdateSysFunctionActionRange(UpdateSysFunctionActionDTO request, string sid);
    }
}

