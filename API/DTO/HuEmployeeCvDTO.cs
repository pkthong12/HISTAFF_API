using API.Main;

namespace API.DTO
{
    public class HuEmployeeCvDTO : BaseDTO
    {
        public string? Avatar { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? FullName { get; set; }
        public string? ProfileCode { get; set; }

        public long? GenderId { get; set; }
        public string? Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? IdNo { get; set; }

        public DateTime? IdDate { get; set; }
        public DateTime? IdExpireDate { get; set; }

        public long? IdPlace { get; set; }

        public long? ReligionId { get; set; }

        public long? NativeId { get; set; }

        public long? NationalityId { get; set; }

        public string? Address { get; set; }

        public string? BirthPlace { get; set; }

        public long? ProvinceId { get; set; }
        public string? ProvinceName { get; set; }

        public long? DistrictId { get; set; }
        public string? DistrictName { get; set; }

        public long? WardId { get; set; }
        public string? WardName { get; set; }

        public string? CurAddress { get; set; }

        public long? CurProvinceId { get; set; }
        public string? CurProvinceName{ get; set; }

        public long? CurDistrictId { get; set; }
        public string? CurDistrictName { get; set; }

        public long? CurWardId { get; set; }
        public string? CurWardName { get; set; }

        public string? TaxCode { get; set; }

        public string? MobilePhone { get; set; }
        public string? MobilePhoneLand { get; set; }

        public string? WorkEmail { get; set; }

        public string? Email { get; set; }

        public long? MaritalStatusId { get; set; }

        public string? PassNo { get; set; }

        public DateTime? PassDate { get; set; }

        public DateTime? PassExpire { get; set; }

        public string? PassPlace { get; set; }

        public string? VisaNo { get; set; }
        public string? PrisonNote { get; set; }
        public string? FamilyDetail { get; set; }

        public DateTime? VisaDate { get; set; }

        public DateTime? VisaExpire { get; set; }

        public string? VisaPlace { get; set; }

        public string? WorkPermit { get; set; }

        public DateTime? WorkPermitDate { get; set; }

        public DateTime? WorkPermitExpire { get; set; }

        public string? WorkPermitPlace { get; set; }

        public string? WorkNo { get; set; }

        public string? WorkPlace { get; set; }

        public string? WorkScope { get; set; }

        public string? ContactPer { get; set; }

        public string? ContactPerPhone { get; set; }

        public long? BankId { get; set; }

        public long? BankBranch { get; set; }
        public long? BankBranchId { get; set; }
        public long? BankId2 { get; set; }
        public long? BankBranch2 { get; set; }
        public long? BankBranchId2 { get; set; }

        public string? BankNo { get; set; }

        public long? SchoolId { get; set; }
        public long? SchoolId2 { get; set; }
        public long? SchoolId3 { get; set; }

        public string? QualificationId { get; set; }

        public long? Qualificationid { get; set; }
        public long? Qualificationid2 { get; set; }
        public long? Qualificationid3 { get; set; }

        public long? TrainingFormId { get; set; }
        public long? TrainingFormId2 { get; set; }
        public long? TrainingFormId3 { get; set; }

        public long? LearningLevelId { get; set; }

        public string? Language { get; set; }

        public string? LanguageMark { get; set; }

        public string? Image { get; set; }

        // EXTRA

        public string? LastWordFromFullname { get; set; }
        public string? BankName { get; set; }
        public string? BankBranchName { get; set; }
        public string? BankNo2 { get; set; }
        public string? BankName2 { get; set; }
        public string? BankBranchName2 { get; set; }
        public string? Height { get; set; }
        public string? Weight { get; set; }
        public string? Domicile { get; set; }
        public string? BirthRegisAddress { get; set; }
        public string? Heart { get; set; }
        public string? Carrer { get; set; }
        public string? InsurenceNumber { get; set; }
        public string? InsurenceArea { get; set; }
        public string? HealthCareAddress { get; set; }
        public string? InsCardNumber { get; set; }
        public string? FamilyMember { get; set; }
        public string? FamilyPolicy { get; set; }
        public string? PoliticalTheory { get; set; }
        public string? CarrerBeforeRecuitment { get; set; }
        public string? TitleConferred { get; set; }
        public string? SchoolOfWork { get; set; }
        public DateTime? TaxCodeDate { get; set; }
        public bool? IsUnionist { get; set; }
        public string? UnionistPosition { get; set; }
        public DateTime? UnionistDate { get; set; }
        public string? UnionistAddress { get; set; }
        public bool? IsJoinYouthGroup { get; set; }
        public bool? IsMember { get; set; }
        public string? MemberPosition { get; set; }
        public DateTime? MemberDate { get; set; }
        public DateTime? MemberOfficalDate { get; set; }
        public string? MemberAddress { get; set; }
        public string? LivingCell { get; set; }
        public string? CardNumber { get; set; }
        public string? PoliticalTheoryLevel { get; set; }
        public string? ResumeNumber { get; set; }
        public DateTime? VateransMemberDate { get; set; }
        public string? VateransPosition { get; set; }
        public string? VateransAddress { get; set; }
        public DateTime? EnlistmentDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string? HighestMilitaryPosition { get; set; }
        public string? CurrentPartyCommittee { get; set; }
        public string? PartytimePartyCommittee { get; set; }
        public string? ComputerSkill { get; set; }
        public string? License { get; set; }
        public long? TaxCodeAddress { get; set; }
        public string? LandlinePhone { get; set; }
        public long? EducationLevelId { get; set; }
        public long? InsWherehealthId { get; set; }
        public long? EmployeeId { get;  set; }
        public long? IdentityAddress { get; set; }
        public string? JobName { get; set; }
        public long? ComputerSkillId { get; set; }
        public long? LicenseId { get; set; }
    }

    public class HuEmployeeCvInputDTO
    {
        public List<long>? OrgIds { get; set;}
        public bool? ShowDissolved { get; set; }
    }
}
