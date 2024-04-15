using API.Main;

namespace API.DTO
{
    public class HuConcurrentlyDTO : BaseDTO
    {
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? SigningDate { get; set; }
        public bool? IsActive { get; set; }
        public string? DecisionNumber { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public long? PositionId { get; set; }
        public string? PositionTitle { get; set; }
        public long? SigningEmployeeId { get; set; }
        public string? SigningEmployeeName { get; set; }
        public string? SigningPositionName { get; set; }
        public string? FullNameOnConcurrently { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
        public long? EmployeeId { get; set; }
        public long? StatusId { get; set; }
        public bool? IsFromWorking { get; set; }
        public long? PositionPoliticalId { get; set; }
        public string? PositionPoliticalName { get; set; }

        //Add by Datnv
        public string? PositionConcurrentName { get; set; }
        public string? OrgConcurrentName { get; set; }
        public string? Code { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? Name { get; set; }
        //
    }
}
