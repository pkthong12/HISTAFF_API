using API.Main;

namespace API.DTO
{
    public class PaListFundSourceDTO:BaseDTO
    {
        public string? Code { get; set; } 
        public string? Name { get; set; } 
        public long? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
    }
}
