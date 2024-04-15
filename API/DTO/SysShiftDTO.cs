namespace API.DTO
{
    public class SysShiftDTO
    {
        public long? Id { get; set; }
        public long? AreaId { get; set; }
        public long? TimeTypeId { get; set; }
        public long? MonId { get; set; }
        public long? TueId { get; set; }
        public long? WedId { get; set; }
        public long? ThuId { get; set; }
        public long? FriId { get; set; }
        public long? SatId { get; set; }
        public long? SunId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? HoursStart { get; set; }
        public DateTime? HoursStop { get; set; }
        public DateTime? BreaksFrom { get; set; }
        public DateTime? BreaksTo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? TimeLate { get; set; }
        public int? TimeEarly { get; set; }
        public bool? IsNoon { get; set; }
        public bool? IsBreak { get; set; }
        public bool? IsActive { get; set; }
    }
}
