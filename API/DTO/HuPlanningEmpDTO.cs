using API.Main;

namespace API.DTO
{
    public class HuPlanningEmpDTO : BaseDTO
    {
        public long? PlanningId { get; set; }
        public long? EmployeeId { get; set; }
        public long? PlanningTitleId { get; set; }
        public long? PlanningTypeId { get; set; }
        public long? EvaluateId { get; set; }
    }
}
