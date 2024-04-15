using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class HuCompanyDTO :BaseDTO
	{
		public string? NameVn { get; set; }
		public string? NameEn { get; set; }
		public long? OrgId { get; set; }
		public string? GpkdAddress { get; set; }
		public long? RegionId { get; set; }
		public string? RegionName { get; set; }
		public string? PhoneNumber { get; set; }
		public string? WorkAddress { get; set; }
		public long? InsUnit { get; set; }
		public string? InsUnitName { get; set; }
		public long? ProvinceId { get; set; }
		public long? DistrictId { get; set; }
		public long? WardId { get; set; }
		public string? FileLogo { get; set; }
		public string? BankAccount { get; set; }
		public long? BankId { get; set; }
		public long? BankBranchId { get; set; }
		public string? FileHeader { get; set; }
		public string? PitCode { get; set; }
		public string? PitCodeChange { get; set; }
		public DateTime? PitCodeDate { get; set; }
		public string? FileFooter { get; set; }
		public long? RepresentativeId { get; set; }
		public long? SignId { get; set; }
		public string? PitCodePlace { get; set; }
		public string? RepresentativeName { get; set; }
		public string? RepresentativeTitle { get; set; }
		public string? GpkdNo { get; set; }
		public DateTime? GpkdDate { get; set; }
		public string? Website { get; set; }
		public string? Fax { get; set; }
		public string? Note { get; set; }
		public bool? IsActive { get; set; }
		public string? BankBranch { get; set; }
		public string? IsActiveStr { get; set; }
		public string? Code { get; set; }
		public int? Order { get; set; }
		public string? ShortName { get; set; }
		public string? Email { get; set; }
	}
}
