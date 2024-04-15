using API.Main;

namespace API.DTO
{
    public class HuContractTypeDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? Period { get; set; }
        public int? DayNotice { get; set; }
        public string? Note { get; set; }
        public bool? IsLeave { get; set; }
        public long? TypeId { get; set; }
        public string? TypeName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsBhxh { get; set; }
        public bool? IsBhyt { get; set; }
        public bool? IsBhtn { get; set; }
        public bool? IsBhtnldBnn { get; set; }
        public string? Status { get; set; }
    }
}
