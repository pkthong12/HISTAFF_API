using API.All.SYSTEM.CoreAPI.SysGroupFunctionAction;
using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysGroupFunctionAction
{
    public interface ISysGroupFunctionActionRepository : IGenericRepository<SYS_GROUP_FUNCTION_ACTION, SysGroupFunctionActionDTO>
    {
        Task<GenericPhaseTwoListResponse<SysGroupFunctionActionDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysGroupFunctionActionDTO> request);
        Task<FormatedResponse> UpdateGroupFunctionActionPermissionRange(UpdateGroupFunctionActionPermissionRangeDTO request, string sid);
    }
}

