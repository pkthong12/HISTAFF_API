using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace ProfileDAL.ViewModels
{
    public class AllowanceViewDTO : Pagings
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? TypeId { get; set; }
        public string? TypeName { get; set; }
        public bool? IsActive { get; set; }
        public string IsActiveStr { get; set; }
        public string? Note { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool? IsInsurance { get; set; }
        public bool? IsCoefficient { get; set; }
        public bool? IsSal { get; set; }
    }

    public class AllowanceInputDTO
    {
        public long? Id { get; set; }
        //[Required(ErrorMessage = "{0}_Required")]
        public string? Code { get; set; }
        //[Required(ErrorMessage = "{0}_Required")]
        public string? Name { get; set; }

        public long? TypeId { get; set; }
        public string? Note { get; set; }
        public bool? IsInsurance { get; set; }
        public bool? IsCoefficient { get; set; }
        public bool? IsSal { get; set; }
        public List<long>? ids { get; set; }
        public bool? ValueToBind { get; set; }
    }
    public class AllowanceEditDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? TypeId { get; set; }
        public string? TypeName { get; set; }
        public bool? IsActive { get; set; }
        public string IsActiveStr { get; set; }
        public string? Note { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool? IsInsurance { get; set; }
        public bool? IsCoefficient { get; set; }
        public bool? IsSal { get; set; }
    }

}
