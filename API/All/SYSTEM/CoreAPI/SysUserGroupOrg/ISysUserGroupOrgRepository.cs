using API.All.SYSTEM.CoreAPI.SysUserGroupOrg;
using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysUserGroupOrg
{
    public interface ISysUserGroupOrgRepository : IGenericRepository<SYS_USER_GROUP_ORG, SysUserGroupOrgDTO>
    {
        Task<GenericPhaseTwoListResponse<SysUserGroupOrgDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysUserGroupOrgDTO> request);
        Task<FormatedResponse> UpdateGroupOrgPermissionRange(UpdateGroupOrgPermissionRangeDTO request, string sid);
    }
}

