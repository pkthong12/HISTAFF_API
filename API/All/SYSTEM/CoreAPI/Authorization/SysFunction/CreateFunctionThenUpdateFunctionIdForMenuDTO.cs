using API.DTO;

namespace API.All.SYSTEM.CoreAPI.Authorization.SysFunction
{
    public class CreateUpdateFunctionThenAsignFunctionIdToMenuDTO
    {
        public SysFunctionDTO Function { get; set; } = null!;
        public long MenuId { get; set; }
    }
}
