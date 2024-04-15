using API.Main;

namespace API.DTO
{
    public class SysOtherListTypeDTO: BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? Orders { get; set; }
        public bool? IsActive { get; set;}
        public bool? IsSystem { get; set; }
        public string? Note { get; set; }

        // More
        public string? Status { get; set; }

    }
}
