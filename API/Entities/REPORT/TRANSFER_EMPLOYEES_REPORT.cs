namespace API.Entities.REPORT
{
    public class TRANSFER_EMPLOYEES_REPORT
    {
        public long STT { get; set; }
        public string? CODE { get; set; }
        public string? FULL_NAME { get; set; }
        public string? EFFECT_DATE { get; set; }
        public string? POSITION_NAME_OLD { get; set; }
        public string? ORG_NAME_OLD { get; set; }
        public string? COMPANY_OLD { get; set; }
        public string? POSITION_NAME_NEW { get; set; }
        public string? ORG_NAME_NEW { get; set; }
        public string? COMPANY_NEW { get; set; }
    }
}
