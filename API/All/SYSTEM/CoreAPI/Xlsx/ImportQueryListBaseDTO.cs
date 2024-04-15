namespace API.All.SYSTEM.CoreAPI.Xlsx
{
    public class ImportQueryListBaseDTO
    {
        public string XlsxSid { get; set; } = null!;
        public string XlsxExCode { get; set; } = null!;
        public long XlsxSession { get; set; }
    }
}
