namespace API.DTO
{
    public class AtShiftDTO
    {

        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public DateTime? HoursStart { get; set; }
        public DateTime? HoursStop { get; set; }
        public DateTime? BreaksFrom { get; set; }

        public string? HoursStopStr { get; set; }
        public string? HoursStartStr { get; set; }
        public DateTime? BreaksTo { get; set; }
        public int? TimeLate { get; set; }
        public int? TimeEarly { get; set; }
        public long? TimeTypeId { get; set; }
        public string? TimeTypeName { get; set; }
        public bool? IsBreak { get; set; }
        public bool? IsBoquacc { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? IsActiveStr { get; set; }
        public string? TimeStart { get; set; }
        public string? TimeStop { get; set; }
        public string? BreaksFromStr { get; set; }
        public string? BreaksToStr { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? MinHoursWork { get; set; }
        public bool? IsNight { get; set; }
        public bool? IsSunday { get; set; }
        public long? Sunday { get; set; }
        public long? Saturday { get; set; }
    }
}
