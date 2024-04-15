using API.Main;

namespace API.DTO
{
    public class HuDistrictDTO
    {
		public long Id { get; set; }
		public string? Code { get; set; }
		public long? ProvinceId { get; set; }
		public string? Name { get; set; }
		public string? ProvinceName { get; set; }
		public bool? IsActive { get; set; }
		public string? Note { get; set; }
		public string? CreatedBy { get; set; }
		public string? IsActiveStr { get; set; }
		public string? UpdatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }

		public long? NationId { get; set; }
		public string? NationName { get; set; }
	}
}
