namespace API.All.HRM.Profile.ProfileDAL.Bussiness.Contract
{
    public class ModelIndex1
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string ContractNo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? SignerName { get; set; }
        public string? SignPos { get; set; }
        public DateTime? SignDate { get; set; }
        public string? StatusName { get; set; }
        public string? Note { get; set; }
        public string? ContractType { get; set; }
        public List<ModelIndex2>? Appendix { get; set; }
        public decimal? SalInsu {  get; set; }
    }
}
