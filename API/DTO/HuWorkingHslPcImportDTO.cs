namespace API.DTO
{
    public class HuWorkingHslPcImportDTO : HuWorkingDTO
    {
        public string? XlsxUserId { get; set; }
        public string? XlsxExCode { get; set; }
        public DateTime? XlsxInsertOn { get; set; }
        public long? XlsxSession { get; set; }
        public string? XlsxFileName { get; set; }
        public int? XlsxRow { get; set; }
        public string? CurPositionName { get; set; }



        // 1
        public long? AllowanceId1 { get; set; }
        public decimal? Coefficient1 { get; set; }
        public DateTime? EffectDate1 { get; set; }
        public DateTime? ExpireDate1 { get; set; }


        // 2
        public long? AllowanceId2 { get; set; }
        public decimal? Coefficient2 { get; set; }
        public DateTime? EffectDate2 { get; set; }
        public DateTime? ExpireDate2 { get; set; }


        // 3
        public long? AllowanceId3 { get; set; }
        public decimal? Coefficient3 { get; set; }
        public DateTime? EffectDate3 { get; set; }
        public DateTime? ExpireDate3 { get; set; }


        // 4
        public long? AllowanceId4 { get; set; }
        public decimal? Coefficient4 { get; set; }
        public DateTime? EffectDate4 { get; set; }
        public DateTime? ExpireDate4 { get; set; }


        // 5
        public long? AllowanceId5 { get; set; }
        public decimal? Coefficient5 { get; set; }
        public DateTime? EffectDate5 { get; set; }
        public DateTime? ExpireDate5 { get; set; }
    }
}