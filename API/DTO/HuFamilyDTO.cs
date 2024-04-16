using API.Main;
using CORE.Services.File;

namespace API.DTO
{
    public class HuFamilyDTO : BaseDTO
    {

        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public long? RelationshipId { get; set; }
        public string? RelationshipName { get; set; }
        public string? Fullname { get; set; }
        public long? Gender { get; set; }
        public string? GenderName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? PitCode { get; set; }
        public bool? SameCompany { get; set; }
        public bool? IsDead { get; set; }
        public bool? IsDeduct { get; set; }
        public DateTime? DeductFrom { get; set; }
        public DateTime? DeductTo { get; set; }
        public DateTime? RegistDeductDate { get; set; }
        public bool? IsHousehold { get; set; }
        public string? IdNo { get; set; }
        public string? Career { get; set; }
        public long? Nationality { get; set; }
        public string? NationalityName { get; set; }
        public string? BirthCerPlace { get; set; }
        public long? BirthCerProvince { get; set; }
        public string? BirthCerProvinceName { get; set; }
        public long? BirthCerDistrict { get; set; }
        public string? BirthCerDistrictName { get; set; }
        public long? BirthCerWard { get; set; }
        public string? BirthCerWardName { get; set; }
        public string? UploadFile { get; set; }
        public long? StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? Note { get; set; }
        public AttachmentDTO? UploadFileBuffer { get; set; }
        public int? JobOrderNum { get; set; }
    }
}
