using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace AttendanceDAL.ViewModels
{
    public class SalaryPeriodDTO : Pagings
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public double? StandardWorking { get; set; }
        public decimal? StandardTime { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }

    }
    public class SalaryPeriodInputDTO
    {

        public long Id { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public int? Year { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public DateTime? DateStart { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public DateTime? DateEnd { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public double? StandardWorking { get; set; }
        public decimal? StandardTime { get; set; }
        public string Note { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public int? Month { get; set; }
        public List<SalaryPeriodDtlDTO> Dtl { get; set; }
        public List<SalaryPeriodDtlDTO> DtlEmp { get; set; }
    }

    public class SalaryPeriodDtlDTO
    {
        public int? OrgId { get; set; }
        public long? EmpId { get; set; }
        public decimal? WorkingStandard { get; set; }
        public decimal? StandardTime { get; set; }
    }
}
