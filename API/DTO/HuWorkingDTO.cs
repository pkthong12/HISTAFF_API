namespace API.DTO
{
    public class HuWorkingDTO
    {
        public long? Id { get; set; }
        public long? WageId { get; set; }
        public long? TenantId { get; set; }
        public long? EmployeeId { get; set; }
        public long? PositionId { get; set; }
        public long? OrgId { get; set; }
        public long? TypeId { get; set; }
        public long? SalaryScaleId { get; set; }
        public long? SalaryTypeId { get; set; }
        public long? SalaryRankId { get; set; }
        public long? SalaryLevelId { get; set; }
        public long? StatusId { get; set; }
        public long? SignId { get; set; }
        public long? OrganizationId { get; set; }
        public DateTime? EffectDate { get; set; }
        public string? EffectDateStr { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? ExpireDateStr { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? SignDate { get; set; }
        public string? SignDateStr { get; set; }
        public DateTime? ExpireUpsalDate { get; set; }
        public string? ExpireUpsalDateStr { get; set; }
        public string? BhxhStatusName { get; set; }
        public string? BhytStatusName { get; set; }
        public string? BhtnStatusName { get; set; }
        public string? BhtnldBnnStatusName { get; set; }

        public decimal? SalBasic { get; set; }
        public decimal? SalTotal { get; set; }
        public decimal? SalPercent { get; set; }

        public string? DecisionNo { get; set; }
        public string? SignerName { get; set; }
        public string? SignerPosition { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public decimal? Coefficient { get; set; }
        public bool? IsChangeSal { get; set; }

        // Extra
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public string? SignerCode { get; set; }
        public string? OrgName { get; set; }
        public string? StatusName { get; set; }
        public string? TypeName { get; set;}
        public string? SalaryType { get; set; }
        public string? SalaryScaleName { get; set; }
        public string? SalaryRankName { get; set; }
        public string? SalaryLevelName { get; set; }
        public decimal? ShortTempSalary { get; set; }
        public string? TaxTableName { get; set; }
        public decimal? CoefficientDcv { get; set; }
        public string? SalaryScaleDcvName { get; set; }
        public string? SalaryRankDcvName { get; set; }
        public string? SalaryLevelDcvName { get; set; }
        public string? RegionName { get; set; }
        public long? EmpWorkStatus { get; set; }
        public DateTime? EffectUpsalDate { get; set; }
        public string? ReasonUpsal { get; set; }

        public List<long>? AllowanceIds { get; set; }
        public long? EmployeeObjectId { get; set; }
        public string? EmployeeObjName { get; set; }
        public string? WorkPlaceName { get; set; }
        public long? CurPositionId { get; set; }
        public bool? IsResponsible { get; set; }
        public long? EmployeeObjId { get; set; }
        public int? JobOrderNum { get; set; }
    }
}
