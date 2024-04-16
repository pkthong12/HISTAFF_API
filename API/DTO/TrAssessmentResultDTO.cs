using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class TrAssessmentResultDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public long? StatusId { get; set; }
        public bool? IsLocked { get; set; }
        public string? Question1 { get; set; }
        public string? Question2 { get; set; }
        public string? Question3 { get; set; }
        public string? Question4 { get; set; }
        public long? TrProgramId { get; set; }



        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? OrgName { get; set; }
        public string? PositionName { get; set; }
        public string? StatusName { get; set; }
        public long? Year { get; set; }
    }
}