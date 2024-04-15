namespace API.Entities.REPORT
{
    public class PAYROLL_KSPN_REPORT
    {
        public long STT { get; set; }
        public string? FULL_NAME { get; set; }
        public string? POSITION { get; set; }
        public string? SALARY_RANK { get; set; }
        public double SALARY { get; set; }
        public double SALARY_RATE { get; set; }
        public double WORKING_HOLIDAY { get; set; }
        public double HOLIDAY_OFF { get; set; }
        public double WORKING_DAYS { get; set; }
        public double ANNUAL_LEAVE { get; set; }
        public double HOLIDAY_PAY { get; set; }
        public double TOTAL_WORKING_DAYS { get; set; }
        public double NET_SALARY { get; set; }
        public double SOCIAL_INSURANCE_CONTRIBUTION { get; set; }
        public double REMAINING_AMOUNT { get; set; }
        public string? NOTE { get; set; }

    }
}
