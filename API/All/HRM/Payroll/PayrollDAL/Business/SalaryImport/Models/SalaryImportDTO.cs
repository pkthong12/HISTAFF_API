using Common.Paging;

namespace PayrollDAL.ViewModels
{
    public class SalaryImportDTO : Pagings
    {
        public long Id { get; set; }
        public int? PeriodId { get; set; }
        public int? OrgId { get; set; }
        public string OrgName { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string PositionName { get; set; }
        public string Note { get; set; }
        public int Year { get; set; }
        public string Period { get; set; }
        public DateTime? AdvanceDate { get; set; }  //Ngày ứng lương
        public long Money { get; set; } // Số tiền ứng
        public long? StatusId { get; set; } // Trạng thái
        public string StatusName { get; set; }
        public string SignerName { get; set; } // Tên người ký
        public string SignerPosition { get; set; } // Chức danh người ký
        public DateTime? SignDate { get; set; }

        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class SalImpSearchParam
    {
        public int OrgId { get; set; }
        public int PeriodId { get; set; }
        public int SalTypeId { get; set; }
    }

    public class SalImpImportParam
    {
        public dynamic Data { get; set; }
        public int PeriodId { get; set; }
        public int? OrgId { get; set; }
        public int SalTypeId { get; set; }
    }

    public class SalImportDTO : Pagings
    {
        public string FullName { get; set; }
        public string EmpCode { get; set; }
        public string PName { get; set; }
        public string OName { get; set; }
        public decimal OrgId { get; set; }
        public decimal? PeriodId { get; set; }
        public decimal? SalaryTypeId { get; set; }
    }
    public class SalImportDelParam
    {
        public List<long> Ids { get; set; }
        public decimal? PeriodId { get; set; }
        public decimal? SalaryTypeId { get; set; }
    }
}
