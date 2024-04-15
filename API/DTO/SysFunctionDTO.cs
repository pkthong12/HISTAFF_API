using API.All.SYSTEM.CoreAPI.Authorization.SysAction;
using API.Main;

namespace API.DTO
{
    public class SysFunctionDTO: BaseDTO
    {
        public long? ModuleId { get; set; }
        public string? Code { get; set; }
        public bool? RootOnly { get; set; }
        public string? Name { get; set; }
        public long? GroupId { get; set; }
        public string? Path { get; set; }
        public bool? PathFullMatch { get; set; }
        public bool? IsActive { get; set; }

        // More

        public string? ModuleName { get; set; }
        public string? GroupName { get; set; }
        public string? ModuleCode { get; set; }
        public string? GroupCode { get; set; }
        public string? Status { get; set; }
        public string? UserId { get; set; }
        public List<SysActionTagDTO>? AppActions { get; set; }
        public List<long>? UserActions { get; set; }
        public long? MenuId { get; set; } // A Childless MenuItem must have a FunctionId (reverted)
        public List<string>? actionCodes { get; set; }
    }
}
