namespace API.DTO
{
    public class HuDynamicReportOnSaveDTO
    {
        public long? Id { get; set; }
        public List<HuDynamicReportDtlTotalDTO>? ListReportDetailTotal { get; set; }
        public string? ViewName { get; set; }
        public string? ViewDescription { get; set; }
        public string? OrderNo { get; set; }
        public long? Fid { get; set; }
    }
}
