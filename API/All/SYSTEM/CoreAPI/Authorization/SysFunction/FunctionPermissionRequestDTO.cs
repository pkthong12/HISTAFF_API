using API.DTO;
using CORE.DTO;

namespace API.All.SYSTEM.CoreAPI.Authorization.SysFunction
{
    public class FunctionPermissionRequestDTO
    {
        public string? UserId { get; set; }
        public required GenericQueryListDTO<SysFunctionDTO> QueryListRequest { get; set; }
    }
}
