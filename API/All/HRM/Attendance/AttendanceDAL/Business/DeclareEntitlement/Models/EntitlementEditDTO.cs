using Common.Paging;

namespace AttendanceDAL.ViewModels
{
    public class EntitlementEditDTO : Pagings
    {
        public long Id { get; set; }
        public long? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public long? OrgId { get; set; }
        public string OrgName { get; set; }
        public string PositionName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal NumberChange { get; set; }
        public string Note { get; set; }
    }

    public class EntitlementEditInputDTO
    {
        public long? Id { get; set; }
        public long EmployeeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal NumberChange { get; set; }
        public string Note { get; set; }
    }

    public class EntitlementEditTmpDTO
    {
        public string Code { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal NumberChange { get; set; }
        public string Note { get; set; }
    }

    public class EntitlementEditImp
    {
        public string Code { get; set; }
        public string FullName { get; set; }
        public string OrgName { get; set; }
        public string PosName { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string NumberChange { get; set; }
        public string Note { get; set; }
    }

    public class EntitlementEditParam
    {
        public List<EntitlementEditImp> Data { get; set; }
    }
}
