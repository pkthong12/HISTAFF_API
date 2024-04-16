using API.Main;

namespace API.DTO
{
    public class TrPrepareDTO : BaseDTO
    {
        public long? TrProgramId { get; set; }
        public string? TrProgramName { get; set; }
        public long? TrListPrepareId { get; set; }
        public string? TrListPrepareName { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
    }
}
