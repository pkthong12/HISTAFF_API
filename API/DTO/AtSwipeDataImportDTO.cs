namespace API.DTO
{
    public class AtSwipeDataImportDTO: AtSwipeDataDTO
    {
        public string? XlsxUserId { get; set; }
        public string? XlsxExCode { get; set; }
        public DateTime? XlsxInsertOn { get; set; }
        public long? XlsxSession { get; set; }
        public string? XlsxFileName { get; set; }
        public int? XlsxRow { get; set; }
    }
}
