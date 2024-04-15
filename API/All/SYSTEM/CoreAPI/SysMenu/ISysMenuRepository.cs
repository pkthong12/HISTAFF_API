using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysMenu
{
    public interface ISysMenuRepository : IGenericRepository<SYS_MENU, SysMenuDTO>
    {
        Task<FormatedResponse> ReadAllActive();
        Task<GenericPhaseTwoListResponse<SysMenuDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysMenuDTO> request);
        Task<FormatedResponse> GetPermittedLinearList(string userId);
    }
}

