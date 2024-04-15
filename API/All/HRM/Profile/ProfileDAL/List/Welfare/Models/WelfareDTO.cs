using Common.Paging;
namespace ProfileDAL.ViewModels
{
    public class WelfareDTO : Pagings
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? Monney { get; set; }
        public int? Seniority { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string? ContractTypeName { get; set; }
        public string? ContractTypeIds { get; set; }
        
        public bool? IsActive { get; set; }
        public string? Active { get; set; }
        public string? Note { get; set; }
        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? seniorityAbove { get; set; }
        public bool? isAutoActive { get; set; }
        public bool? isCalTax { get; set; }
        public DateTime? paymentDate { get; set; }
        public int? percentage { get; set; }
        public string genderId { get; set; }
        public int? ageFrom { get; set; }
        public int? ageTo { get; set; }
        public int? workLeaveNopayFrom { get; set; }
        public int? workLeaveNopayTo { get; set; }
        public int? monthsPendFrom { get; set; }
        public int? monthsPendTo { get; set; }
        public int? monthsWorkInYear { get; set; }
    }

    public class WelfareInputDTO
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public decimal? Monney { get; set; }
        public int? Seniority { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        //public List<long> ContractTypes { get; set; }
        public string? Note { get; set; }
        public int? seniorityAbove { get; set; }
        public bool? isAutoActive { get; set; }
        public bool? isCalTax { get; set; }
        public DateTime? paymentDate { get; set; }
        public int? percentage { get; set; }
        public long? genderId { get; set; }
        public int? ageFrom { get; set; }
        public int? ageTo { get; set; }
        public int? workLeaveNopayFrom { get; set; }
        public int? workLeaveNopayTo { get; set; }
        public int? monthsPendFrom { get; set; }
        public int? monthsPendTo { get; set; }
        public int? monthsWorkInYear { get; set; }
        public List<long?>? Ids { get; set; }
        public bool? ValueToBind { get; set; }
    }

}
