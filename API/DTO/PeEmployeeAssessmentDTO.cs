using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class PeEmployeeAssessmentDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public long? HuCompetencyPeriodId { get; set; }



        public string? Code { get; set; }
        public string? FullName { get; set; }
        public string? OrgName { get; set; }
        public string? PositionName { get; set; }
    }
}