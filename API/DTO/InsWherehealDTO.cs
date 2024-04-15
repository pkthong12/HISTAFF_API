using API.Main;

namespace API.DTO
{
    public class InsWherehealthDTO: BaseDTO
    {
        public string? Code { get; set; }
        public string? NameVn { get; set; }
        public string? Address { get; set; }
        public string?  Actflg { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public bool? IsActive { get; set; }
        public string? ProvinceName { get; set; }
        public string? DistrictName { get; set; }
        public string? Note  { get; set; }
        public string? Status { get; set; }
    }
}
