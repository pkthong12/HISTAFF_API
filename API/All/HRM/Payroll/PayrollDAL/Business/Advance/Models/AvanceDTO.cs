using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace PayrollDAL.ViewModels
{
    public class AdvanceDTO : Pagings
    {
        public long Id { get; set; }
        public int? PeriodId { get; set; }
        public long? OrgId { get; set; }
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

    public class AdvanceInputDTO
    {
        public long? Id { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public long employeeId { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public int Year { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public int periodId { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public int Money { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public DateTime AdvanceDate { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public int StatusId { get; set; } // Trạng thái
        public long SignId { get; set; } // Người ký
        [Required(ErrorMessage = "{0}_Required")]
        public string SignerName { get; set; } // Tên người ký
        [Required(ErrorMessage = "{0}_Required")]
        public string SignerPosition { get; set; } // Chức danh người 
        [Required(ErrorMessage = "{0}_Required")]
        public DateTime? SignDate { get; set; } // Ngày ký
        public string Note { get; set; }
    }

    public class AdvanceTmpParam
    {
        public int PeriodId { get; set; }
        public int Year { get; set; }
        public int OrgId { get; set; }
        public List<AdvanceTmpDtlParam> Data { get; set; }
    }

    public class AdvanceTmpDtlParam
    {
        public string EmpCode { get; set; }
        public string Year { get; set; }
        public string PeriodName { get; set; }
        public string Money { get; set; }
        public string AdvanceDate { get; set; }
        public string StatusName { get; set; } // Trạng thái
        public string SignerName { get; set; } // Tên người ký
        public string SignerPosition { get; set; } // Chức danh người 
        public string SignDate { get; set; } // Ngày ký
        public string Note { get; set; }

    }


}
