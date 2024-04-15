namespace API.DTO
{
    public class RptJobPosHisDTO
    {
        public long? Id { get; set; }
        public long? ParentId { get; set; }
        public string? NameEn { get; set; }
        public string? NameVn { get; set; }
        public string? Jobgroup { get; set; }
        public string? Code { get; set; }
        public decimal? Level1 { get; set; }
        public decimal? LyFte { get; set; }
        public decimal? YtdFte { get; set; }
        public decimal? PlanFte { get; set; }
        public decimal? VsLyFte { get; set; }
        public decimal? VsPlanFte { get; set; }
        public decimal? NodeHasChild { get; set; }
        public decimal? Order1 { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal? LyFteV2 { get; set; }

    }
}
