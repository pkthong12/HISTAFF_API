using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace ProfileDAL.ViewModels
{
    public class InsChangeDTO : Pagings
    {
        public long Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public long? OrgId { get; set; }
        public string OrgName { get; set; }
        public string  PositionName { get; set; }
        public string ChangeTypeName { get; set; } // Loại biến động
        public DateTime? ChangeMonth { get; set; } //Tháng biến động
        public DateTime? TerEffectDate { get; set; } //Tháng biến động
        public double? SalaryOld { get; set; }// Hệ số kỳ trước
        public double? SalaryNew { get; set; }// Hệ số kỳ này
        public long? WorkStatusId { get; set; }
        public bool? IsBhxh { get; set; }// BHXH
        public bool? IsBhyt { get; set; }// BHYT
        public bool? IsBhtn { get; set; }// BHTN
        public bool? IsBnn { get; set; }// BNN
    }

    public class InsChangeInputDTO
    {
        public long Id { get; set; }
        public long Tenant_Id { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int? ChangeTypeId { get; set; } // Loại biến động
        [Required(ErrorMessage = "{0}_Required")]
        public DateTime? ChangeMonth { get; set; } //Tháng biến động
        [Required(ErrorMessage = "{0}_Required")]
        public double? SalaryOld { get; set; }// Hệ số kỳ trước
        [Required(ErrorMessage = "{0}_Required")]
        public double? SalaryNew { get; set; }// Hệ số kỳ này
        public string Note { get; set; }
        public bool? IsBhxh { get; set; }// BHXH
        public bool? IsBhyt { get; set; }// BHYT
        public bool? IsBhtn { get; set; }// BHTN
        public bool? IsBnn { get; set; }// BNN

    }

    public class ImportInsDtlParam
    {
        public string Code { get; set; }
        public string FullName { get; set; }
        public string TypeName { get; set; }
        public string ChangeMonth { get; set; }
        public string SalaryOld { get; set; }
        public string SalaryNew { get; set; }
        public string Bhxh { get; set; }
        public string Bhyt { get; set; }
        public string Bhtn { get; set; }
        public string Bnn { get; set; }
    }

    public class ImportInsParam
    {
        public List<ImportInsDtlParam> Data { get; set; }
    }

}
