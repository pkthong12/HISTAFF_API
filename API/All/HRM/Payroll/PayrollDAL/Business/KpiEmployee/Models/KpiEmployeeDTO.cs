using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace PayrollDAL.ViewModels
{
    public class KpiEmployeeDTO : Pagings
    {
        public long Id { get; set; }
        public long? PeriodId { get; set; }
        public long? OrgId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string PositionName { get; set; }
        public long? KpiTargetId { get; set; }
        public string KpiTargetName { get; set; }
        public decimal? RealValue { get; set; }
        public decimal? StartValue { get; set; }
        public decimal? EqualValue { get; set; }
        public decimal? KpiSalary { get; set; }
        public bool? IsPaySalary { get; set; }
        public string Note { get; set; }
    }

    public class KpiEmployeeInputDTO
    {
        public long? Id { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public string Code { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public long KpiGroupId { get; set; }
        public string Unit { get; set; }
        public int? MaxValue { get; set; }
        public bool? IsRealValue { get; set; }
        public bool? IsPaySalary { get; set; }
        public bool? IsImportKpi { get; set; }
        public bool? IsActive { get; set; }
        public int? Orders { get; set; }
        public string Note { get; set; }

    }
    public class KpiEmployeeInput
    {
        [Required(ErrorMessage = "{0_Required}")]
        public long OrgId { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public int PeriodId { get; set; }
        public int? KpiTargetId { get; set; }

    }
    public class KpiEmployeeOutput
    {
        public int Order { get; set; }
        public long KpiTargetId { get; set; }
        public long EmployeeId { get; set; }
        public string KpiName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Pname { get; set; }
        public decimal StartValue { get; set; }
        public decimal RealValue { get; set; }
        public string Note { get; set; }

    }
    public class KpiEmployeeImport
    {
        public int PeriodId { get; set; }
        public int OrgId { get; set; }
        public IFormFile file { get; set; }
    }
}
