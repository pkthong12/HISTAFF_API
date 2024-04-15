namespace API.DTO
{
    public class HuJobDTO
    {
        public long? Id { get; set; }
        public long? PhanLoaiId { get; set; }
        public long? JobBandId { get; set; }
        public long? JobFamilyId { get; set; }
        public string? Code { get; set; }
        public string? NameVn { get; set; }
        public string? NameEn { get; set; }
        public string? Actflg { get; set; }
        public string? Request { get; set; }
        public string? Purpose { get; set; }
        public string? Note { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? CreatedLog { get; set; }
        public string? ModifiedLog { get; set; }
        public int? Orders { get; set; }

        // RELATED
        public int? EmployeeCount { get; set; }
    }
}
