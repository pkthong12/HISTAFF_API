using API.Main;

namespace API.DTO
{
    public class SysContractTypeDTO : BaseDTO
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? Period { get; set; }
        public int? DayNotice { get; set; }
        public string? Note { get; set; }
        public bool? IsLeave { get; set; }
        public bool? IsActive { get; set; }
    }
}
