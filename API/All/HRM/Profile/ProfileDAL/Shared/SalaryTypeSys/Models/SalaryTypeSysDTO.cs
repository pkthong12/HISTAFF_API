using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace ProfileDAL.ViewModels
{
    public class SalaryTypeSysDTO : Pagings
    {
        public long? Id { get; set; }
        public long? AreaId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Orders { get; set; }
        public bool? IsActive { get; set; }
        public string Note { get; set; }
        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    } 

    public class SalaryTypeSysInputDTO
    {
        public long? Id { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string Name { get; set; }
        public int? Orders { get; set; }
        public long? AreaId { get; set; }
        public string Note { get; set; }
    }
}
