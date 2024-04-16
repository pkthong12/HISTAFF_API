using API.Main;

namespace API.DTO
{
    public class HuAssetMngDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public long? OrgId { get; set; }
        public long? AssetId { get; set; }
        public long? SerialNum { get; set; }
        public decimal? ValueAsset { get; set; }
        public DateTime? DateIssue { get; set; }
        public DateTime? RevocationDate { get; set; }
        public long? StatusAssetId { get; set; }
        public string? Note { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }

        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? PositionName { get; set; }
        public string? OrganizationName { get; set; }
        public string? JobName { get; set; }
        public string? AssetName { get; set; }
        public string? GroupAssetName { get; set; }
        public string? StatusAssetName { get; set; }
        public bool? IsLeaveWork { get; set; }
        public long? WorkStatusId { get; set; }
        public string? WorkStatusName { get; set; }
    }
}
