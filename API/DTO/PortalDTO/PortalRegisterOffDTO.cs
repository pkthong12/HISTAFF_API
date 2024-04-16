using API.Main;

namespace API.DTO.PortalDTO
{
    public class PortalRegisterOffDTO : BaseDTO
    {
        public DateTime? WorkingDay { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public long? TimeTypeId { get; set; }
        public long? ApproveId { get; set; }
        public string? Note { get; set; }
        public long? EmployeeId { get; set; }
        public string? TypeCode { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public bool? IsChangeToFund { get; set; }
        public bool? IsTimeKeeping { get; set; }
        public bool? IsWorkingOvernight { get; set; }
        public int? TotalDay { get; set; }
        public decimal? TotalOt { get; set; }
        public string? IdReggroup { get; set; }
        public long? TypeId { get; set; }
        public int? TimeLate { get; set; }
        public int? TimeEarly { get; set; }
        public bool? IsEachDay { get; set; }

    }

    public class PortalExplainWorkDTO : BaseDTO
    {
        public DateTime? ExplainDate { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public long? ApproveId { get; set; }
        public long? SymbolId { get; set; }
        public string? Note { get; set; }
        public long? EmployeeId { get; set; }
        public string? TypeCode { get; set; }
        public string? IdReggroup { get; set; }
    }

    public class PortalDateTimeSearch
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
