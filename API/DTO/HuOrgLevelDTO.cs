using API.Main;

namespace API.DTO
{
    public class HuOrgLevelDTO: BaseDTO
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? OrderNum { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }

    }
}
