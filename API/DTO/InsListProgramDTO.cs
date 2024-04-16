using API.Main;

namespace API.DTO
{
    public class InsListProgramDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedLog { get; set; }
        public string? ModifiedLog { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
    }
}
