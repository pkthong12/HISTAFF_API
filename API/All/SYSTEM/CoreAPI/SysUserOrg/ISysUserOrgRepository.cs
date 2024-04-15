using API.All.SYSTEM.CoreAPI.SysUserOrg;
using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysUserOrg
{
    public interface ISysUserOrgRepository : IGenericRepository<SYS_USER_ORG, SysUserOrgDTO>
    {
        Task<GenericPhaseTwoListResponse<SysUserOrgDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysUserOrgDTO> request);
        Task<FormatedResponse> UpdateUserOrgPermissionRange(UpdateUserOrgPermissionRangeDTO request, string sid);
    }
}

