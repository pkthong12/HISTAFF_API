namespace API.Entities.REPORT
{
    public class TRANSFER_PAYROLL_REPORT
    {
        public long STT { get; set; }
        public string? EMPLOYEE_CODE { get; set; }
        public string? FULL_NAME { get; set; }
        public string? BANK_NO { get; set; }
        public double? PAYROLL_TOTAL { get; set; }
        public string? CURRENCY { get; set; }
    }
}
