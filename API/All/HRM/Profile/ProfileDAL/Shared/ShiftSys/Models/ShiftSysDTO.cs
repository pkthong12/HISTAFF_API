using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace ProfileDAL.ViewModels
{
    public class ShiftSysDTO : Pagings
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? HoursStart { get; set; }
        public DateTime? HoursStop { get; set; }
        public DateTime? BreaksFrom { get; set; }
        public DateTime? BreaksTo { get; set; }
        public int? TimeLate { get; set; }
        public int? TimeEarly { get; set; }
        public long? AreaId { get; set; }
        public long TimeTypeId { get; set; }
        public string TimeTypeName { get; set; }
        public bool? IsNoon { get; set; }
        public bool? IsBreak { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class ShiftSysInputDTO
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public DateTime HoursStart { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public DateTime HoursStop { get; set; }
       // [Required(ErrorMessage = "{0}_Required")]
        public DateTime? BreaksFrom { get; set; }
       // [Required(ErrorMessage = "{0}_Required")]
        public DateTime? BreaksTo { get; set; }
        public int? TimeLate { get; set; }
        public int? TimeEarly { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public int AreaId { get; set; }

        [Required(ErrorMessage = "{0}_Required")]
        public long TimeTypeId { get; set; }
        public string TimeTypeName { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public bool? IsNoon { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public bool? IsBreak { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }

        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class ShiftCycleSysInput
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public long? MonId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public long? TueId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public long? WedId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public long? ThuId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public long? FriId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public long? SatId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public long? SunId { get; set; }
        public long? AreaId { get; set; }
    }
}
