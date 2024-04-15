using API.Main;

namespace API.DTO
{
    public class SysActionDTO: BaseDTO
    {
        public string? Code { get; set; }
        public bool? IsActive { get; set; }
    }
}
