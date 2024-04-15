using Common.Extensions;
using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace ProfileDAL.ViewModels
{
    public class CommendDTO : Pagings
    {
        public long? Id { get; set; }
        public int? CommendObjId { get; set; } // Đối tượng khen thưởng
        public string CommendObjName { get; set; } // Đối tượng khen 
        public List<ReferParam> Employees { get; set; }
        public string EmployeeName { get; set; }
        public long? OrgId { get; set; }
        public string OrgName { get; set; }
        public string SignerName { get; set; }
        public string SignerPosition { get; set; }
        public DateTime? SignDate { get; set; }
        public string No { get; set; }
        public DateTime? EffectDate { get; set; }
        public string CommendType { get; set; } // Hình thức khen thưởng
        public double? Money { get; set; } // Mức thưởng
        public string SourceCostName { get; set; }
        public int? Year { get; set; }
        public bool? IsTax { get; set; }
        public string Reason { get; set; }
        public long? StatusId { get; set; } // Trạng thái
        public string StatusName { get; set; } // Trạng thái
        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CommendInputDTO
    {
        public long? Id { get; set; }
        public long? TenantId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public int? CommendObjId { get; set; } // Đối tượng khen 
        public int? OrgId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string No { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public Int64? SignId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public DateTime? EffectDate { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public int? SourceCostId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string CommendType { get; set; } // Hình thức khen thưởng
        [Required(ErrorMessage = "{0}_Required")]
        public string Reason { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public long? StatusId { get; set; } // Trạng thái
        [Required(ErrorMessage = "{0}_Required")]
        public double? Money { get; set; } // Mức thưởng
        [Required(ErrorMessage = "{0}_Required")]
        public bool? IsTax { get; set; }
        public int? PeriodId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public int? Year { get; set; } //năm
        [Required(ErrorMessage = "{0}_Required")]
        public string SignerName { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string SignerPosition { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public DateTime? SignDate { get; set; }

        public List<long> Emps { get; set; }
    }
    public class CommendOutputDTO
    {
        public long? Id { get; set; }
        public int? CommendObjId { get; set; } // Đối tượng khen 
        public string CommendObjCode { get; set; }
        public int? OrgId { get; set; }
        public string OrgName { get; set; }
        public string No { get; set; }
        public Int64? SignId { get; set; }
        public string SignerName { get; set; }
        public string SignerPosition { get; set; }
        public DateTime? SignDate { get; set; }
        public DateTime? EffectDate { get; set; }
        public int? SourceCostId { get; set; }
        public string CommendType { get; set; } // Hình thức khen thưởng
        public string Reason { get; set; }
        public long? StatusId { get; set; } // Trạng thái
        public double? Money { get; set; } // Mức thưởng
        public bool? IsTax { get; set; }
        public int? PeriodId { get; set; }
        public int? Year { get; set; } //năm
        public List<EmployeeDTO> Employees { get; set; }
    }
}
