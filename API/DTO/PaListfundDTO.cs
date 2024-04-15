using API.Main;

namespace API.DTO
{
    public class PaListfundDTO : BaseDTO
    {
        public string? ListfundCode { get; set; }
        public string? ListfundName { get; set; }
        public long? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? Note { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public string? Status { get; set; }
        public bool? IsActive { get;set; }
    }
}
