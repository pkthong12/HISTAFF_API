using API.Main;

namespace API.DTO
{
    public class HuWelfareDTO: BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public decimal? Monney { get; set; }
        public int? Seniority { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public bool? IsActive { get; set; }
    }
}
