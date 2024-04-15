using API.Main;

namespace API.DTO
{
    public class HuEvaluateDTO: BaseDTO
    {
        public long? EvaluateType { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public long? PositionId { get; set; }
        public string? PositionName { get; set; }
        public int? Year { get; set; }
        public string? YearSearch { get; set; }
        public string? PointSearch { get; set; }
        public long? ClassificationId { get; set; }
        public int? Point { get; set; }
        public string? Note { get; set; }
        public string? EvaluateName { get; set; }
        public string? ClassificationName { get; set; }
        public string? PositionConcurrentName { get; set; }
        public string? OrgConcurrentName { get; set; }
        public long? PositionConcurrentId { get; set; }
        public long? WorkStatusId { get; set; }
        public string? WorkStatusName { get; set; }
        public long? EmployeeStatus { get; set; }
        public long? EmployeeConcurrentId { get; set; }
        public string? EmployeeConcurrentName { get; set; }
        public long? OrgConcurrentId { get; set; }
        public long? ProfileId { get; set; }
        public int? JobOrderNum { get; set; }


    }
     public class HuEvaluateCountDTO
    {
        public long? EmpId { get; set; }
        public long? OrgId { get; set; }
        public int? EvaluateCount { get; set;}

    }
}
