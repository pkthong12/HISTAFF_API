using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace ProfileDAL.ViewModels
{
    public class EmployeeDTO : Pagings
    {
        public long? Id { get; set; }
        public long? TenantID { get; set; }
        public string? Code { get; set; }
        public string? ContractNo { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Fullname { get; set; }
        public string? Image { get; set; }
        public string? Avatar { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public int? PositionId { get; set; }
        public string? PositionName { get; set; }
        public long? DirectManagerId { get; set; }
        public string? DirectManagerName { get; set; }
        public string? DirectManagerTitleName { get; set; }
        public int? GenderId { get; set; }
        public string? GenderName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? IdNo { get; set; }
        public DateTime? IdDate { get; set; }
        public long? IdPlace { get; set; }
        public int? ReligionId { get; set; }
        public string? ReligionName { get; set; }
        public int? NativeId { get; set; }
        public string? NativeName { get; set; }
        public int? NationalityId { get; set; }
        public string? NationalityName { get; set; }
        public string? Address { get; set; }
        public string? BirthPlace { get; set; }
        public DateTime? JoinDate { get; set; }
        public long? WorkStatusId { get; set; }
        public string? WorkStatusName { get; set; }
        public long? ProvinceId { get; set; }
        public string? ProvinceName { get; set; }
        public long? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int? WardId { get; set; }
        public string? WardName { get; set; }
        public long? ContractId { get; set; }
        public string? ContractName { get; set; }
        public DateTime? ContractExpired { get; set; }
        public long? LastWorkingId { get; set; }
        public string? LastWorkingNo { get; set; }
        public DateTime? TerEffectDate { get; set; }
        public string? ItimeId { get; set; }
        public int? SalaryTypeId { get; set; }
        public string? ObjectSalaryName { get; set; }
        public string? TaxCode { get; set; }
        public string? MobilePhone { get; set; }
        public string? WorkEmail { get; set; }
        public string? Email { get; set; }
        public int? MaritalStatusId { get; set; }
        public string? MaritalStatusName { get; set; }
        public string? PassNo { get; set; }
        public DateTime? PassDate { get; set; }
        public DateTime? PassExpire { get; set; }
        public string? PassPlace { get; set; }
        public string? VisaNo { get; set; }
        public DateTime? VisaDate { get; set; }
        public DateTime? VisaExpire { get; set; }
        public string? VisaPlace { get; set; }
        public string? WorkPermit { get; set; }
        public DateTime? WorkPermitDate { get; set; }
        public DateTime? WorkPermitExpire { get; set; }
        public string? WorkPermitPlace { get; set; }
        public string? ContactPer { get; set; }
        public string? ContactPerPhone { get; set; }
        public int? BankId { get; set; }
        public string? BankName { get; set; }
        public string? BankBranch { get; set; }
        public string? BankNo { get; set; }
        public string? SchoolId { get; set; }
        public string? SchoolName { get; set; }
        public string? QualificationId { get; set; }
        public string? QualificationName { get; set; }
        public int? TrainingFormId { get; set; }
        public string? TrainingFormName { get; set; }
        public int? LearningLevelId { get; set; }
        public string? LearningLevelName { get; set; }
        public string? Language { get; set; }
        public string? LanguageMark { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Extra

        public string? MemberPosition { get; set; }
        public string? LivingCell { get; set; }

        public string? PlanningTitleName { get; set; }
        public long? PlanningTitleId { get; set; }
        public string? PlanningTypeName { get; set; }
        public long? PlanningTypeId { get; set; }
    }

    public class EmployeeInput
    {
        public long? Id { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string LastName { get; set; }
        public string Fullname { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public int? OrgId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public int? PositionId { get; set; }
        public int? StaffRankId { get; set; }
        public string Avatar { get; set; }
        public Int64? DirectManagerId { get; set; }
        public int? GenderId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string IdNo { get; set; }
        public DateTime? IdDate { get; set; }
        public string IdPlace { get; set; }
        public int? ReligionId { get; set; }
        public int? NativeId { get; set; }
        public int? ResidentId { get; set; }
        public int? BankId { get; set; }
        public int? NationalityId { get; set; }
        public string Address { get; set; }
        public string BirthPlace { get; set; }
        public DateTime? JoinDate { get; set; }
        public long? WorkStatusId { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public int? WardId { get; set; }
        public string CurAddress { get; set; }
        public int? CurProvinceId { get; set; }
        public int? CurDistrictId { get; set; }
        public int? CurWardId { get; set; }
        //public Int64? ContractId { get; set; }
        public Int64? LastWorkingId { get; set; }
        public DateTime? TerEffectDate { get; set; }
        public string ItimeId { get; set; }
        public Int64? ObjectSalaryId { get; set; }
        public string TaxCode { get; set; }
        public string MobilePhone { get; set; }
        public string WorkEmail { get; set; }
        public string Email { get; set; }
        public int? MaritalStatusId { get; set; }
        public string PassNo { get; set; }
        public DateTime? PassDate { get; set; }
        public DateTime? PassExpire { get; set; }
        public string PassPlace { get; set; }
        public string VisaNo { get; set; }
        public DateTime? VisaDate { get; set; }
        public DateTime? VisaExpire { get; set; }
        public string VisaPlace { get; set; }
        public string WorkPermit { get; set; }
        public DateTime? WorkPermitDate { get; set; }
        public DateTime? WorkPermitExpire { get; set; }
        public string WorkPermitPlace { get; set; }
        public string WorkNo { get; set; } // so cchn
        public DateTime? WorkDate { get; set; }
        public string WorkPlace { get; set; }
        public string WorkScope { get; set; }
        public string ContactPer { get; set; }
        public string ContactPerPhone { get; set; }
        public string BankBranch { get; set; }
        public string BankNo { get; set; }
        public string SchoolId { get; set; }
        public string QualificationId { get; set; }
        public int? TrainingFormId { get; set; }
        public int? LearningLevelId { get; set; }
        public string Language { get; set; }
        public string LanguageMark { get; set; }
    }
    public class EmployeePopup : Pagings
    {
        public Int64? EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public long? OrgId { get; set; }
        public string OrgName { get; set; }
        public long? PositionId { get; set; }
        public string PositionName { get; set; }
        public DateTime? TerEffectDate { get; set; }
        public long? WorkStatusId { get; set; }
        public long? WorkingId { get; set; }
        public string OrgParentName { get; set; }
        public DateTime? ContractExpired { get; set; }
        public string NationalityName { get; set; }

    }

    public class SituationDTO
    {
        public long? Id { get; set; }
        public long? TENANT_ID { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public int? RelationshipId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public long EmployeeId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string Name { get; set; }
        public string No { get; set; }
        public string TaxNo { get; set; }
        public string FamilyNo { get; set; }
        public string FamilyName { get; set; }
        public string Address { get; set; }
        public DateTime? Birth { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }

    public class EmployeeOutput
    {

        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public long? PositionId { get; set; }
        public string PositionName { get; set; }
        public long? OrgId { get; set; }
        public string OrgName { get; set; }
        public string OrgParentName { get; set; }
        public string ContractNo { get; set; }
        public DateTime? ContracExpired { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? StartDate { get; set; }
        public long? WorkingId { get; set; }
        public string Avatar { get; set; }
        public long? WorkStatusId { get; set; }
    }
    public class EmpPopupDTO : Pagings
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Fullname { get; set; }
        public long? OrgId { get; set; }
        public string OrgName { get; set; }
        public string PositionName { get; set; }
        public long? WorkStatusId { get; set; }
        public DateTime? TerEffectDate { get; set; }
    }
    public class EmployeeInputImport
    {
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrgName { get; set; }
        public string ItimeId { get; set; }
        public string Position { get; set; }
        public string TaxCode { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string BirthPlace { get; set; }
        public string SchoolId { get; set; } // Tên trường
        public string QualificationId { get; set; } // Trình độ chuyên môn
        public string LearningLevel { get; set; } // Trình độ học vấn
        public string TrainingForm { get; set; } // Hình thức đào tạo
        public string Language { get; set; }
        public string IdNo { get; set; }
        public string IdDate { get; set; }
        public string IdPlace { get; set; }
        public string Nationality { get; set; }
        public string Native { get; set; }
        public string Religion { get; set; }
        public string MaritalStatus { get; set; }
        public string Resident { get; set; }
        public string CurProvince { get; set; }
        public string CurDistrict { get; set; }
        public string CurWard { get; set; }
        public string CurAddress { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Address { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string WorkEmail { get; set; }
        public string ContactPer { get; set; }
        public string ContactPerPhone { get; set; }
        public string PassNo { get; set; } // số hộ chiếu
        public string PassDate { get; set; }
        public string PassExpire { get; set; }
        public string PassPlace { get; set; }
        public string VisaNo { get; set; } // số visa
        public string VisaDate { get; set; }
        public string VisaExpire { get; set; }
        public string VisaPlace { get; set; }
        public string WorkPermit { get; set; } // so giay phep lao dong
        public string WorkPermitDate { get; set; }
        public string WorkPermitExpire { get; set; }
        public string WorkPermitPlace { get; set; }
        public string WorkNo { get; set; } // so cchn
        public string WorkDate { get; set; }
        public string WorkPlace { get; set; }
        public string WorkScope { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; } // Chi nhánh Ngân hàng
        public string BankNo { get; set; } // Số tài 
    }

    public class EmpImportParam
    {
        public List<EmployeeInputImport> Data { get; set; }
    }
    public class EmployeeFileImport
    {
        public IFormFile file { get; set; }
    }

    public class PaperInput
    {
        public long Id { get; set; }
        public long EmpId { get; set; }
        public int PaperId { get; set; }
        public DateTime DateInput { get; set; }
        public string Url { get; set; }
        public string Note { get; set; }
        public bool? StatusId { get; set; }// 1:miễn nộp
    }
    public class ListPaperView
    {
        public decimal? Id { get; set; }
        public decimal? PaperId { get; set; }
        public decimal? TypeId { get; set; }
        public string PageName { get; set; }
        public DateTime? DateInput { get; set; }
        public bool? statusId { get; set; }
        public string Url { get; set; }
        public string Note { get; set; }
    }

    public class InfoView
    {
        public DateTime? Birth { get; set; }
        public string TaxNo { get; set; }
        public string InsureNo { get; set; }
        public Int64 ContractTypeId { get; set; }
        public string ContractTypeName { get; set; }
        public decimal Seniority { get; set; }
        public decimal? DayOff { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string MngName { get; set; }
        public decimal? MngId { get; set; }

    }
    public class PhoneBookView
    {
        public string Code { get; set; }
        public string FullName { get; set; }
        public string BirthDay { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string TitleName { get; set; }
        public string OgName { get; set; }
        public string OrgName { get; set; }
        public string Org2Name { get; set; }
        public string MngName { get; set; }
        public string Address { get; set; }
        public Int64 EmployeeId { get; set; }
        public decimal? Stt { get; set; }
    }
    public class EmployeeMainInfoDTO
    {
        //Thông tin chính
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
    public class EmployeeInfoDTO
    {
        //Thông tin chính
        public int? GenderId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }
    }

    //Địa chỉ 
    public class EmployeeAddressDTO
    {
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public int? wardId { get; set; }
        public string address { get; set; }

    }
    //Địa chỉ thường trú
    public class EmployeeCurAddressDTO
    {
        public int? CurProvinceId { get; set; }
        public int? CurDistrictId { get; set; }
        public int? CurWardId { get; set; }
        public string CurAddress { get; set; }
    }

    //Thông tin liên hệ
    public class EmployeeContactInfoDTO
    {
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string WorkEmail { get; set; }
        public string ContactPer { get; set; }
        public string ContactPerPhone { get; set; }
    }

    //Thông tin hộ chiếu
    public class EmployeePassportDTO
    {
     
        public string PassNo { get; set; }
        public DateTime? PassDate { get; set; }
        public DateTime? PassExpire { get; set; }
        public string PassPlace { get; set; }

    }
    //Visa
    public class EmployeeVisaDTO
    {
   
        public string visaNo { get; set; }
        public DateTime? visaDate { get; set; }
        public DateTime? visaExpire { get; set; }
        public string visaPlace { get; set; }
    }

    //Giấy phép lao động
    public class EmployeeWorkPermitDTO
    {
        public string workPermit { get; set; }
        public DateTime? workPermitDate { get; set; }
        public DateTime? workPermitExpire { get; set; }
        public string workPermitPlace { get; set; }

    }

    //Chứng chỉ hành nghề
    public class EmployeeCertificateDTO
    {
        public string workNo { get; set; }
        public DateTime? workDate { get; set; }
        public string workScope { get; set; }
        public string workPlace { get; set; }

    }

    //Trình độ học vấn
    public class EmployeeEducationDTO
    {
        public string SchoolName { get; set; }
        public string SchoolId { get; set; }
        public string QualificationId { get; set; }
        public string TrainingFormName { get; set; }
        public string LearningLevelName { get; set; }
        public string LanguageMark { get; set; }
        public string Language { get; set; }
       // public int? TrainingformId { get; set; }
        public int? LearningLevelId { get; set; }



    }

    //Trình độ học vấn
    public class EmployeeBankDTO
    {
        public string BankNo { get; set; }
        public int? BankId { get; set; }
        public string BankBranch { get; set; }

    }
    public class EmployeeEditInput
    {
        public int? EmployeeId { get; set; } // ID
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string Avatar { get; set; }
        public string LastName { get; set; }
        public string Fullname { get; set; }
        public int? OrgId { get; set; }
        public int? PositionId { get; set; }
        public string PositionName { get; set; }
        public int? DirectManagerId { get; set; }
        public int? GenderId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string IdNo { get; set; }
        public DateTime? IdDate { get; set; }

        public string IdPlace { get; set; } //Địa chỉ thường trú
        public long? ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public long? DistrictId { get; set; }
        public int? WardId { get; set; }

        //Thông tin cá nhân
        public int? ReligionId { get; set; }
        public int? NativeId { get; set; }
        public int? NationalityId { get; set; }
        public string Address { get; set; }
        public string BirthPlace { get; set; }
        public DateTime? JoinDate { get; set; }
        public long? WorkStatusId { get; set; }

        public int? ContractId { get; set; }
        public int? LastWorkingId { get; set; }
        public DateTime? TerEffectDate { get; set; }
        public string ItimeId { get; set; }
        public int? ObjectSalaryId { get; set; }
        public string TaxCode { get; set; }
        public string MobilePhone { get; set; }
        public string WorkEmail { get; set; }
        public string Email { get; set; }

        //Hộ chiếu
        public int? MaritalStatusId { get; set; }
        public string PassNo { get; set; } //Số hộ chiếu
        public DateTime? PassDate { get; set; } //Ngày cấp
        public DateTime? PassExpire { get; set; } //Ngày hết hạn
        public string PassPlace { get; set; }  //Nơi cấp

        //Visa
        public string VisaNo { get; set; } 
        public DateTime? VisaDate { get; set; }
        public DateTime? VisaExpire { get; set; }
        public string VisaPlace { get; set; }

        //Giấy phép lao động
        public string WorkPermit { get; set; }
        public DateTime? WorkPermitDate { get; set; }
        public DateTime? WorkPermitExpire { get; set; }
        public string WorkPermitPlace { get; set; }
        public string ContactPer { get; set; }
        public string ContactPerPhone { get; set; }

        //Tài khoản
        public int? BankId { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public int? BankNo { get; set; }

        //Trình độ học vấn
        public string SchoolName { get; set; }
        public string QualificationId { get; set; }
        public string TrainingFormName { get; set; }
        public int? LearningLevelName { get; set; }
        public string Language { get; set; }
        public int? LearningLevelId { get; set; }
        public int? LanguageMark { get; set; }
        public int? ResidentId { get; set; }
        public int? SalTotal { get; set; }

        //Địa chỉ thường trú
        public int? CurProvinceId { get; set; }
        public int? CurDistrictId { get; set; }
        public int? CurWardId { get; set; }
        public string CurAddress { get; set; }

        //Chứng chỉ hành nghề
        public string WorkNo { get; set; }
        public string WorkDate { get; set; }
        public string WorkScope { get; set; }
        public string WorkPlace { get; set; }

    }
    public class SituationEditDTO
    {
        [Required(ErrorMessage = "{0}_Required")]
        public int? RelationshipId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string Name { get; set; }
        public string No { get; set; }
        public string TaxNo { get; set; }
        public string FamilyNo { get; set; }
        public string FamilyName { get; set; }
        public string Address { get; set; }
        public DateTime? Birth { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
    public class EmployeeEditDTO : Pagings
    {
        public Int64? Id { get; set; }
        public string Code { get; set; }
        public string Fullname { get; set; }
        public string Image { get; set; }
        public long? OrgId { get; set; }
        public long? StatusId { get; set; }
        public string OrgName { get; set; }
        public string PositionName { get; set; }
        public string StatusName { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class FamilyEditDTO : Pagings
    {
        public Int64? Id { get; set; }
        public string Code { get; set; }
        public string Fullname { get; set; }
        public string Avatar { get; set; }
        public long? OrgId { get; set; }
        public string OrgName { get; set; }
        public string PName { get; set; }
        public string RelationName { get; set; }
        public string StatusName { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDay { get; set; }
        public string No { get; set; } // cmnd
        public string TaxNo { get; set; } // ma so thue
    }
}
