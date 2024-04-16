namespace API.All.HRM.ProfessionalEmployee.ProfessionalEmployeeAPI.PeEmployeeAssessment
{
    public class EmployeeModel
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? Fullname { get; set; }
        public string? OrgName { get; set; }
        public string? PositionName { get; set; }
        public long? OrgId { get; set; }
        public long? HuCompetencyPeriodId { get; set; }
    }
}