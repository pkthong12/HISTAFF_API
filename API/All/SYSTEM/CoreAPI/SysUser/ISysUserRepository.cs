using API.All.SYSTEM.CoreAPI.SysUser;
using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysUser
{
    public interface ISysUserRepository : IGenericRepository<SYS_USER, SysUserDTO>
    {
        Task<GenericPhaseTwoListResponse<SysUserDTO>> SinglePhaseQueryList(GenericQueryListStringIdDTO<SysUserDTO> request);
        Task<FormatedResponse> CreateUser(SysUserCreateUpdateRequest request, string sid);
        Task<FormatedResponse> UpdateUser(SysUserCreateUpdateRequest request, string sid);
        Task<FormatedResponse> SynchronousAccount(string sid);
        Task<FormatedResponse> ResetAccount(List<string> userIds);
        Task<FormatedResponse> LockAccount(List<string> userIds, string sid);
        Task<FormatedResponse> UnlockAccount(List<string> userIds, string sid);
        Task<FormatedResponse> ChangePassword(SysUserChangePasswordRequest request);
        Task<FormatedResponse> QueryOrgPermissionList(SYS_USER user); // For Logging-in User
        Task<FormatedResponse> QueryOrgWithPositions(SYS_USER user); // For Logging-in User
        Task<FormatedResponse> QueryUserOrgPermissionList(SYS_USER user, bool? useGroupIfEmpty = true); // user is param
        Task<FormatedResponse> QueryFunctionActionPermissionList(SYS_USER user, bool? useGroupIfEmpty = true); // user is param
        Task<FormatedResponse> ChangePasswordPortal(SysUserChangePasswordRequest request);
        Task<FormatedResponse> SubmitUsernameWhenForgotPassword(ResetPasswordRequest request);
        Task<FormatedResponse> SubmitVerificationCode(ResetPasswordRequest request);
        Task<FormatedResponse> ChangePasswordWhenForgotPassword(ResetPasswordRequest request);
    }
}