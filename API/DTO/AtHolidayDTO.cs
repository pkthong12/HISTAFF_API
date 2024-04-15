namespace API.DTO
{
    public class AtHolidayDTO
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public DateTime? StartDayoff { get; set; }
        public DateTime? EndDayoff { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
