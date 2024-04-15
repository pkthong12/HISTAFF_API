using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace ProfileDAL.ViewModels
{
    public class SalaryElementSysDTO : Pagings
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public bool? IsSystem { get; set; }
        public bool? IsActive { get; set; }
        public int Orders { get; set; }
        public string Note { get; set; }
        public long DataType { get; set; } // 0: kiểu số; 1 kiểu text
        public long? AreaId { get; set; }
    }

    public class SalaryElementSysInputDTO
    {
        public long? Id { get; set; }
        [Required(ErrorMessage ="{0_Required}")]
        public string Code { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public long? GroupId { get; set; }
        public long? AreaId { get; set; }
        public int Orders { get; set; }
        public string Note { get; set; }
    }
    public class GroupElementSysDTO
    {
        public long? groupId { get; set; }
        public long? AreaId { get; set; }
    }
}
