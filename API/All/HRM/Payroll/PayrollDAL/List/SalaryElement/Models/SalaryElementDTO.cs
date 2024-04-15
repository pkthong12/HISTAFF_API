using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace PayrollDAL.ViewModels
{
    public class SalaryElementDTO : Pagings
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? GroupId { get; set; }
        public bool? IsSystem { get; set; }
        public bool? IsActive { get; set; }
        public int Orders { get; set; }
        public string Note { get; set; }
        public long DataType { get; set; } // 0: kiểu số; 1 kiểu text
    }

    public class SalaryElementInputDTO
    {
        public long? Id { get; set; }
        [Required(ErrorMessage ="{0_Required}")]
        public string Code { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public long? GroupId { get; set; }
        public int? DataType { get; set; }
        public int Orders { get; set; }
        public string Note { get; set; }
    }
    public class AllowanToElementDTO
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Orders { get; set; }
        public string Note { get; set; }
    }
}
