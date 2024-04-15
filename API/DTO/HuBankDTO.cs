using API.Main;

namespace API.DTO
{
    public class HuBankDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Note { get; set; }
        public int? Order { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
    }
}
