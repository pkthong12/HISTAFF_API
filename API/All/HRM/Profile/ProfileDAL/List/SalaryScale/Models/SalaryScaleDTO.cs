using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class SalaryScaleDTO : Pagings
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Orders { get; set; }
        public bool? IsActive { get; set; }
        public string? Note { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public DateTime EffectiveDate { get; set; }
        public bool? IsTableScore { get; set; }
    }
    public class SalaryScaleViewDTO : Pagings
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Orders { get; set; }
        public bool? IsActive { get; set; }
        public string? Note { get; set; }
        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public bool? IsTableScore { get; set; }
        public string? Status { get; set; }
    }


    public class SalaryScaleInputDTO
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Orders { get; set; }
        public string Note { get; set; }
        public bool? IsTableScore { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }
    public class SalaryScaleEditDTO
    {
        public long? Id { get; set; }
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

}
