namespace API.DTO
{
    public class HuEmployeeEditDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? OrgId { get; set; }
        public long? ContractId { get; set; }
        public long? ContractTypeId { get; set; }
        public long? PositionId { get; set; }
        public long? LastWorkingId { get; set; }
        public long? DirectManagerId { get; set; }
        public long? GenderId { get; set; }
        public long? ReligionId { get; set; }
        public long? NativeId { get; set; }
        public long? NationalityId { get; set; }
        public long? WorkStatusId { get; set; }
        public string? Code { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Fullname { get; set; }
        public string? DirectManager { get; set; }
        public string? IdNo { get; set; }
        public string? IdPlace { get; set; }
        public string? Address { get; set; }
        public string? BirthPlace { get; set; }

        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? WardId { get; set; }
        public long? SalaryTypeId { get; set; }
        public long? MaritalStatusId { get; set; }
        public DateTime? TerEffectDate { get; set; }
        public DateTime? JoinDate { get; set; }
        public string? ItimeId { get; set; }
        public string? TaxCode { get; set; }
        public string? MobilePhone { get; set; }
        public string? WorkEmail { get; set; }
        public string? Email { get; set; }
        public string? PassNo { get; set; }
        public string? PassPlace { get; set; }
        public string? VisaNo { get; set; }
        public string? VisaPlace { get; set; }
        public string? WorkPermitPlace { get; set; }
        public string? WorkPermit { get; set; }
        public string? WorkNo { get; set; }
        public string? WorkScope { get; set; }
        public string? WorkPlace { get; set; }
        public string? ContactPer { get; set; }
        public string? ContactPerPhone { get; set; }
        public DateTime? PassDate { get; set; }
        public DateTime? PassExpire { get; set; }
        public DateTime? VisaDate { get; set; }
        public DateTime? VisaExpire { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? WorkPermitDate { get; set; }
        public DateTime? WorkPermitExpire { get; set; }
        public DateTime? WorkDate { get; set; }

        public int? Seniority { get; set; }
        public int? Status { get; set; }
        public long? BankId { get; set; }
        public long? SchoolId { get; set; }
        public string? BankBranch { get; set; }
        public string? BankNo { get; set; }
        public string? Schoolname { get; set; }
        public string? Trainingformname { get; set; }

        public string? Learninglevelname { get; set; }
        public long? QualificationId { get; set; }
        public int? Qualificationid { get; set; }
        public string? LanguageMark { get; set; }
        public long? TrainingFormId { get; set; }
        public long? LearningLevelId { get; set; }
        public long? ResidentId { get; set; }
        public long? CurWardId { get; set; }
        public long? CurDistrictId { get; set; }
        public long? CurProvinceId { get; set; }
        public int? SalTotal { get; set; }
        public string? Language { get; set; }

        public DateTime? IdDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CurAddress { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? StaffRankId { get; set; }
    }
}
