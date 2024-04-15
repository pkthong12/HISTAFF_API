using Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace ProfileDAL.ViewModels
{
    public class FormListDTO
    {


        public string Name { get; set; }
        public int? IdMap { get; set; }
        public long? ParentId { get; set; }
        public int? TypeId { get; set; }
        public int? IdOrigin { get; set; }
        public int? HasChild { get; set; }
        public string Text { get; set; }

    }

    public class SettingRemindDTO
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int? Day { get; set; }
        public bool? IsActive { get; set; }
    };
    public class RemindDTO
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int? Day { get; set; }
        public int Count { get; set; }
        public string State { get; set; }
        public List<ReferParam> Value { get; set; }
    };
    public class PayrollInputDTO
    {
        [Required(ErrorMessage = "{0}_Required")]
        public decimal OrgId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public decimal PeriodId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public decimal SalaryTypeId { get; set; }

    }
}
