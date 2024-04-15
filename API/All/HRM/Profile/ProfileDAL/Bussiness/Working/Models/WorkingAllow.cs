using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class WorkingAllowDTO : Pagings
    {
        public long? Id { get; set; }
        public long? WorkingId { get; set; }
        public long? AllowanceId { get; set; }
        public decimal? Amount { get; set; }
    }

    public class WorkingAllowInputDTO
    {
        public long? Id { get; set; }
        public long? WorkingId { get; set; }
        public long? AllowanceId { get; set; }
        public string AllowanceName { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Effectdate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? Coefficient { get; set; }
    }
    public class WorkingAllowViewDTO : Pagings
    {
        public long? Id { get; set; }
        public long? WorkingId { get; set; }
        public string? AllowanceName { get; set; }
        public decimal? Coefficient { get; set; }
        public DateTime? Effectdate { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
