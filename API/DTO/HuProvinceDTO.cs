using API.Main;

namespace API.DTO
{
    public class HuProvinceDTO : BaseDTO
    {
        
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public string? IsActiveStr { get; set; }
        public bool? IsActive { get; set; }
        public long? NationId { get; set; }
		public string? NationName { get; set; }

	}
}
