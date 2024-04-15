using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace ProfileDAL.ViewModels
{
    public class SalaryTypeDTO : Pagings
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Orders { get; set; }
        public bool? IsActive { get; set; }
        public string? Note { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public long? SalaryTypeGroup { get; set; }
        public string? SalaryTypeGroupName { get; set; }
        public DateTime? EffectDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    } 
}
