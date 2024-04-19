using API.All.HRM.Profile.ProfileAPI.HuOrganization;
using API.Main;
using CORE.Services.File;

namespace API.DTO
{
    public class HuOrganizationDTO : BaseDTO
    {
        public long? TenantId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool? Status { get; set; }
        public string? StatusString { get; set; }
        public long? ParentId { get; set; }
        public long? CompanyId { get; set; }
        public long? OrgLevelId { get; set; }
        public int? OrderNum { get; set; }
        public long? HeadPosId { get; set; }
        public string? HeadPosName { get; set; }
        public long? MngId { get; set; }
        public DateTime? FoundationDate { get; set; }
        public DateTime? DissolveDate { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? Address { get; set; }
        public string? BusinessNumber { get; set; }
        public DateTime? BusinessDate { get; set; }
        public string? TaxCode { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? ShortName { get; set; }
        public string? NameEn { get; set; }
        public float? UyBan { get; set; }
        public float? Groupproject { get; set; }
        public int? LevelOrg { get; set; }

        // Related properties

        public string? ParentName { get; set; }
        public string? ParentNameEn { get; set; }
        public string? OrgLevelName { get; set; }
        public string? CompanyNameVn { get; set; }
        public string? CompanyNameEn { get; set; }
        public string? HeadEmployeeNames { get; set; }

        public int? OrderNumAuto { get; set; }

        public AttachmentDTO? AttachedFileBuffer { get; set; }
        public string? AttachedFile { get; set; }
    }

}
