using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using ProfileDAL.ViewModels;

namespace API.DTO
{
    public class InsHealthInsuranceDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public string? PosName { get; set; }
        public string? IdNo { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Year { get; set; }
        public long? InsContractId { get; set; }
        public string? InsContractNo { get; set; }
        public long? OrgInsurance { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? ValCo { get; set; }
        public bool? CheckBhnt { get; set; }
        public long? FamilyId { get; set; }
        public string? FamilyMemberName { get; set; }
        public string? RelationshipName { get; set; }
        public DateTime? FamilyMemberBirthDate { get; set; }
        public string? FamilyMemberIdNo { get; set; }
        public long? DtChitra { get; set; }
        public string? DtChitraName { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? EffectDate { get; set; }
        public long? MoneyIns { get; set; }
        public DateTime? ReduceDate { get; set; }
        public decimal? Refund { get; set; }
        public DateTime? DateReceiveMoney { get; set; }
        public string? EmpReceiveMoney { get; set; }
        public string? Note { get; set; }
        public long? InsContractDeId { get; set; }
        public int? JobOrderNum { get; set; }
        public List<InsClaimInsuranceDTO>? InsClaimInsurances { get; set; }

        // InsClaimInsurance
        public DateTime? ExamineDate { get; set; }
        public string? DiseaseName { get; set; }
        public decimal? AmountOfClaims { get; set; }
        public decimal? AmountOfCompensation { get; set; }
        public DateTime? CompensationDate { get; set; }
    }
}
