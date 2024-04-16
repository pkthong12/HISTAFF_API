using API.Main;

namespace API.DTO
{
    public class SysGroupDTO: BaseDTO
    {
        public string? Name { get; set; }
        public string? Note { get; set; }
        public string? Code { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSystem { get; set; }

        // Thêm

        public string? CloneFrom { get; set; }

    }
}
