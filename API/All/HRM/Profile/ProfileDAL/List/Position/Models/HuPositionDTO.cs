namespace API.All.HRM.Profile.ProfileDAL.List.Position.Models
{
    public class HuPositionDTO
    {
        public long? Id { get; set; }

        public long? GroupId { get; set; }
        public string? GroupName { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Note { get; set; }

        public string? JobDesc { get; set; }

		public long? TenantID { get; set; }

		public bool? IsActive { get; set; }

		public string? CreateBy { get; set; }

		public string? UpdatedBy { get; set; }

		public DateTime? CreateDate { get; set; }

		public DateTime? UpdatedDate { get; set; }

		public long? OrgId { get; set; }
		public string? OrgName { get; set; }

		public long? JobId { get; set; }

		public int? Lm { get; set; }

		public bool? Isowner { get; set; }

		public int? Csm { get; set; }

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

		public int? WorkLocation { get; set; }

		public int? HiringStatus { get; set; }
	}
}
