using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace PayrollDAL.ViewModels
{
    public class KpiTargetDTO : Pagings
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? GroupId { get; set; }
        public string Unit { get; set; }
        public int? Orders { get; set; }
    }

    public class KpiTargetOutDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SalaryElement { get; set; }
        public string Unit { get; set; }
        public bool? IsRealValue { get; set; }
        public bool? IsPaySalary { get; set; }
        public bool? IsImportKpi { get; set; }
        public int? Orders { get; set; }
        public bool? IsActive { get; set; }
    }

    public class KpiTargetInputDTO
    {
        public long? Id { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public string Code { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public long KpiGroupId { get; set; }
        public string Unit { get; set; }
        public string ColName { get; set; }
        public int? ColId { get; set; }
        public int? MaxValue { get; set; }
        public bool? IsRealValue { get; set; }
        public bool? IsPaySalary { get; set; }
        public bool? IsImportKpi { get; set; }
        public bool? IsActive { get; set; }
        public int? TypeId { get; set; }
        public int? Orders { get; set; }
        public string Note { get; set; }
    
    }
    public class KpiTargetQickDTO
    {
        public long? Id { get; set; }
        public bool? IsRealValue { get; set; }
        public bool? IsPaySalary { get; set; }
        public bool? IsImportKpi { get; set; }
    }
}
