using API.Main;

namespace API.DTO
{
    public class HuEmployeeDTO: BaseDTO
    {
        public long? ProfileId { get; set; }
        public string? Avatar { get; set; }
        public string? Code { get; set; }
        public string? ProfileCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Fullname { get; set; }
        public string? FullNameOnConcurrently { get; set; }
        public string? IdNo { get; set; }
        public string? Image { get; set; }
        public long? OrgId { get; set; }
        public long? ContractId { get; set; }
        public long? ContractTypeId { get; set; }
        public long? PositionId { get; set; }
        public long? ResidentId { get; set; }
        public long? LastWorkingId { get; set; }
        public long? DirectManagerId { get; set; }
        public long? GenderId { get; set; }
        public long? ReligionId { get; set; }
        public long? NativeId { get; set; }
        public long? NationalityId { get; set; }
        public long? IdPlace { get; set; }
        public string? AddressIdentity { get; set; }
        public string? Address { get; set; }
        public string? BirthPlace { get; set; }
        public string? CurAddress { get; set; }
        public string? ItimeId { get; set; }
        public long? WorkStatusId { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? WardId { get; set; }
        public long? CurProvinceId { get; set; }
        public string? CurProvinceName { get; set; }
        public long? CurDistrictId { get; set; }
        public string? CurDistrictName { get; set; }
        public long? CurWardId { get; set; }
        public string? CurWardName { get; set; }
        public long? SalaryTypeId { get; set; }

        public DateTime? ContractExpired { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? JoinDateState { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? IdDate { get; set; }
        public DateTime? EffectDate { get; set; }
        public DateTime? TerEffectDate { get; set; }


        public string? TaxCode { get; set; }
        
        public string? MobilePhone { get; set; }
        public string? WorkEmail { get; set; }
        public string? Email { get; set; }

        public long? MaritalStatusId { get; set; }
        public string? MaritalStatusName { get; set; }
        public string? PassNo { get; set; }
        public string? PassPlace { get; set; }
        public string? VisaNo { get; set; }
        public string? VisaPlace { get; set; }
        
        public string? WorkPermit { get; set; }
        public DateTime? PassDate { get; set; }
        public DateTime? WorkPermitDate { get; set; }
        public DateTime? PassExpire { get; set; }
        public DateTime? VisaDate { get; set; }
        public DateTime? VisaExpire { get; set; }
        public DateTime? WorkPermitExpire { get; set; }
        public DateTime? WorkDate { get; set; }
        public string? SchoolId { get; set; }
        public long? BankId { get; set; }
        public long? Qualificationid { get; set; }

        public long? EmployeeObjectId { get; set; }

        public int? Seniority { get; set; }
        public string? WorkPermitPlace { get; set; }
        public string? WorkNo { get; set; }
        public string? WorkPlace { get; set; }
        public string? WorkScope { get; set; }
        public string? ContactPer { get; set; }
        public string? ContactPerPhone { get; set; }
        public string? BankNo { get; set; }
        public string? BankBranch { get; set; }
        public string? QualificationId { get; set; }
        public string? Language { get; set; }
        public string? LanguageMark { get; set; }
        public long? TrainingFormId { get; set; }
        public long? LearningLevelId { get; set; }
        public long? Schoolid { get; set; }
        public long? StaffRankId { get; set; }
        public decimal? SalTotal { get; set; }
        public decimal? SalBasic { get; set; }
        public decimal? SalRate { get; set; }
        public decimal? DayOf { get; set; }

        // RELATED

        public long? JobId { get; set; }
        public string? OrgName { get; set; }

        public string? PositionName { get; set; }
        public int? JobOrderNum { get; set; }
        public string? PositionNameOnConcurrently { get; set; }
        public string? GenderName { get; set; }
        public string? NativeName { get; set; }
        public string? WorkStatusName { get; set; }
        public string? ProvinceName { get; set; }
        public string? DistrictName { get; set; }
        public string? WardName { get; set; }

        public string? ContractNo { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string? ReligionName { get; set; }
        public string? MemberPosition { get; set; }
        public string? LivingCell { get; set; }
        public string? GovernmentManagement { get; set; }
        public string? YellowFlag { get; set; }
        public string? Relations { get; set; }
        public string? UnionistPosition { get; set; }
        public DateTime? UnionistDate { get; set; }
        public string? UnionistAddress { get; set; }
        public DateTime? YouthSaveNationDate { get; set; }
        public string? YouthSaveNationPosition { get; set; }
        public string? License { get; set; }
        public long? Presenter { get; set; }
        public string? PresenterPhoneNumber { get; set; }
        public string? PresenterAddress { get; set; }
        public string? LandlinePhone { get; set; }
        public string? NameOnProfileEmployee { get; set; }
        public string? Company { get; set; }

        public string? EmployeeObjectName { get; set; }
        public string? OtherName { get; set; }
        public string? NationalityName { get; set; }
        public string? Domicile { get; set; }
        public string? BankName { get; set; }
        public string? BankBranchName { get; set; }
        public string? BirthRegisAddress { get; set; }
        public string? IdPlaceName { get; set; }

        // thêm trường IsMember "là Đảng viên"
        // vào trong DTO
        public bool? IsMember { get; set; }
        public string? AddressReffererEmployee { get; set; } // lấy ra thông tin người giới thiệu trong hồ sơ nhân viên

        public bool? IsLeaveWork { get; set; }
        public bool? IsRepresentative { get; set; } // Get representative of state capital

        public string? StatusDocument { get; set; }
    }
}
