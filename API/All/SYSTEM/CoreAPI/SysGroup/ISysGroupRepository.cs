using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysGroup
{
    public interface ISysGroupRepository : IGenericRepository<SYS_GROUP, SysGroupDTO>
    {
        Task<GenericPhaseTwoListResponse<SysGroupDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysGroupDTO> request);
        Task<FormatedResponse> QueryOrgPermissionList(long groupId); // user is param
        Task<FormatedResponse> QueryFunctionActionPermissionList(long groupId); // user is param
        Task<FormatedResponse> Clone(GenericUnitOfWork _uow, SysGroupDTO dto, string sid);
    }
}

