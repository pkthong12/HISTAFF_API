using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class ReportDTO
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public long? ParentId { get; set; }
        public List<ReportDTO> Child { get; set; }
    }

    public class ReportInputDTO : Pagings
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string ParentName { get; set; }
        public string Note { get; set; }
    }
    public class ReportInsInputDTO : Pagings
    {
        public string EMPLOYEE_CODE { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public int? YearId { get; set; }
        public int? OrgId { get; set; }
    }

    public class ReportEmployeeDTO : Pagings
    {
        public DateTime? ToDate { get; set; }
        public DateTime? DateLeave { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDay { get; set; }
        public string PositionName { get; set; }
        public long? OrgId { get; set; }
        public string OrgName { get; set; }
        public string Qualification { get; set; }
        public string Education { get; set; }
        public string Language { get; set; }
        public DateTime? DateJoin { get; set; }
    }

    public class ReportInsByOrgDTO 
    {
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public int? OrgId { get; set; }
    }
    public class ReportParam
    {
        public string Todate { get; set; }
        public int? OrgId { get; set; }
    }

}
