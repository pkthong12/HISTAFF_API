namespace API.DTO
{
    public class HuPositionDTO
    {
        public long? Id { get; set; }
        public long? GroupId { get; set; }
        public long? TenantId { get; set; }
        public long? OrgId { get; set; }
        public long? JobId { get; set; }

        //
        public string? OrgName { get; set; }
        //
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public string? JobDesc { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int? Lm { get; set; }
        public int? Csm { get; set; }
        public bool? Isowner { get; set; }
        public bool? IsNonphysical { get; set; }
        public long? Master { get; set; }
        public int? Concurrent { get; set; }

        public bool? IsPlan { get; set; }
        public long? Interim { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public string? TypeActivities { get; set; }
        public string? Filename { get; set; }
        public string? Uploadfile { get; set; }
        public int? WorkingTime { get; set; }
        public string? NameEn { get; set; }
        public long? WorkLocation { get; set; }
        public long? HiringStatus { get; set; }

        public bool? IsTdv { get; set; }
        public bool? IsNotot { get; set; }

        //khiemna_add
        public string? MasterName { get; set; }
        public string? InterimName { get; set; }
        public string? LmName { get; set; }
        public string? EmpLmName { get; set; }
        public string? LmJobName { get; set; }
        public string? CsmName { get; set; }
        public string? CsmJobName { get; set; }
        public string? JobName { get; set; }
        public string? Active { get; set; }

    }
}
