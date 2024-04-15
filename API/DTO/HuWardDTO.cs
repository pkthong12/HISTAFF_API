using API.Main;

namespace API.DTO
{
	public class HuWardDTO : BaseDTO
	{
		public long? DistrictId { get; set; }
		public string? DistrictName { get; set; }
		public long? ProvinceId { get; set; }
		public string? ProvinceName { get; set; }
		public string? Code { get; set; }
		public string? Name { get; set; }
		public string? Note { get; set; }
		public string? Status { get; set; }
		public bool? IsActive { get; set; }

		public long? NationId { get; set; }
		public string? NationName { get; set; }
	}
}
