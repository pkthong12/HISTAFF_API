using API.Main;

namespace API.DTO
{
    public class InsListContractDTO : BaseDTO
    {
        public string? ContractInsNo { get; set; }
        public int? Year { get; set; }
        public long? OrgInsurance { get; set; }
        public string? OrgInsuranceName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? ValCo { get; set; }
        public DateTime? BuyDate { get; set; }
        public string? Note { get; set; }
        public int? ProgramId { get; set; }
        public bool? IsDeleted { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedLog { get; set; }
        public string? ModifiedLog { get; set; }
        public decimal? SalInsu { get; set; }
        public string? Program { get; set; }
        public bool? IsActive { get; set; }
    }
}
