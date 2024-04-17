namespace API.Entities.REPORT
{
    public class EMPLOYEES_EXPIRING_CONTRACTS_REPORT
    {
        public long STT { get; set; }
        public string? CODE { get; set; }
        public string? FULL_NAME { get; set; }
        public string? POSITION { get; set; }
        public string? ORG_NAME { get; set; }
        public string? TYPE_CONTRACT { get; set; }
        public string? START_DATE { get; set; }
        public string? EXPIRE_DATE { get; set; }
    }
}
