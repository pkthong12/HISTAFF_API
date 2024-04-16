﻿using API.Main;

namespace API.DTO
{
    public class RcCandidateCvDTO : BaseDTO
    {
        public long? CandidateId { get; set; }
        public long? GenderId { get; set; }
        public long? MaritalStatus { get; set; }
        public long? NationId { get; set; }
        public long? ReligionId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BirthAddress { get; set; }
        public long? NationalityId { get; set; }
        public string? IdNo { get; set; }
        public DateTime? IdDate { get; set; }
        public DateTime? IdDateExpire { get; set; }
        public long? IdPlace { get; set; }
        public string? PerAddress { get; set; }
        public long? PerProvince { get; set; }
        public long? PerDistrict { get; set; }
        public long? PerWard { get; set; }
        public string? ContactAddressTemp { get; set; }
        public long? ContactProvinceTemp { get; set; }
        public long? ContactDistrictTemp { get; set; }
        public long? ContactWardTemp { get; set; }
        public string? PerEmail { get; set; }
        public string? MobilePhone { get; set; }
        public string? FinderSdt { get; set; }
        public bool? IsWorkPermit { get; set; }
        public DateTime? WorkPermitStart { get; set; }
        public DateTime? WorkPermitEnd { get; set; }
        public long? EducationLevelId { get; set; }
        public long? LearningLevelId { get; set; }
        public long? GraduateSchoolId { get; set; }
        public long? MajorId { get; set; }
        public int? YearGraduation { get; set; }
        public string? Rating { get; set; }
        public long? RcComputerLevelId { get; set; }
        public long? TypeClassificationId { get; set; }
        public long? LanguageId { get; set; }
        public long? LanguageLevelId { get; set; }
        public string? Mark { get; set; }
        public long? PosWish1Id { get; set; }
        public long? PosWish2Id { get; set; }
        public decimal? ProbationSalary { get; set; }
        public decimal? WishSalary { get; set; }
        public string? DesiredWorkplace { get; set; }
        public DateTime? StartDateWork { get; set; }
        public string? LevelDesired { get; set; }
        public string? NumExperience { get; set; }
        public bool? IsHsvHv { get; set; }
        public string? OtherSuggestions { get; set; }
        public string? ReName { get; set; }
        public long? ReRelationship { get; set; }
        public string? RePhone { get; set; }
        public string? ReAddress { get; set; }
        public string? InName { get; set; }
        public string? InPhone { get; set; }
        public string? InNote { get; set; }
        public double? Height { get; set; }
        public string? EarNoseThroat { get; set; }
        public double? Weight { get; set; }
        public string? Dentomaxillofacial { get; set; }
        public string? BloodGroup { get; set; }
        public string? Heart { get; set; }
        public string? BloodPressure { get; set; }
        public string? LungsAndChest { get; set; }
        public string? LeftEyeVision { get; set; }
        public string? RightEyeVision { get; set; }
        public string? HepatitisB { get; set; }
        public string? LeatherVenereal { get; set; }
        public long? HealthType { get; set; }
        public string? NoteSk { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }

    }
}
