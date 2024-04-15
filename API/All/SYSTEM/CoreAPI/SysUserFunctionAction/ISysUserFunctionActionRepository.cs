using API.All.SYSTEM.CoreAPI.SysUserFunctionAction;
using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;
using DocumentFormat.OpenXml.Bibliography;

namespace API.Controllers.SysUserFunctionAction
{
    public interface ISysUserFunctionActionRepository : IGenericRepository<SYS_USER_FUNCTION_ACTION, SysUserFunctionActionDTO>
    {
        Task<GenericPhaseTwoListResponse<SysUserFunctionActionDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysUserFunctionActionDTO> request);
        Task<FormatedResponse> UpdateUserFunctionActionPermissionRange(UpdateUserFunctionActionPermissionRangeDTO request, string sid);
        Task<FormatedResponse> DeleteByUserId(string userId);
    }
}

