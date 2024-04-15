using API.Main;

namespace API.DTO
{
    public class HuBankBranchDTO : BaseDTO
    {
        public long? BankId { get; set; }
        public string? BankName { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
    }
}
