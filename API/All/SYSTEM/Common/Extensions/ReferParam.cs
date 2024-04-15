namespace Common.Extensions
{
    public class ReferParam 
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
    }

    public class ReferFileParam
    {
        public IFormFile file { get; set; }
    }

    public class DataParam
    {
        public dynamic Data { get; set; }
        public int PeriodId { get; set; }
        public int? OrgId { get; set; }
        public int SalTypeId { get; set; }
    }
    public class ApproveDTO
    {
        public long Id { get; set; }
        public string ApproveNote { get; set; }
    }

    public class ValuesDTO
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class ParaOrg
    {
        public int OrgId { get; set; }
    }

    public class ParaInputReport
    {
        public int? Year { get; set; }
        public int? OrgId { get; set; }
        public int? PeriodId { get; set; }
        public int? SalaryTypeId { get; set; }
    }

    public class NotifyHomeView
    {
        public decimal TotalApprove { get; set; }
        public decimal TotalNew { get; set; }
        public decimal TotalNoty { get; set; }
    }
     public class ReminderParam 
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Avatar { get; set; }
        public long? OrgId { get; set; }
    }
}
