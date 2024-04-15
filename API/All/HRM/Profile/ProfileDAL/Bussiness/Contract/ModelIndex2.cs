namespace API.All.HRM.Profile.ProfileDAL.Bussiness.Contract
{
    public class ModelIndex2
    {
        public long? Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? ContractAppendixNo { get; set; }
        public DateTime? SignDate { get; set; }
        public decimal? Coefficient { get; set; }
        public decimal? SalInsu { get; set; }
        public string AllowanceName { get; set; }
        public long? WorkingId { get; set; }
    }
}