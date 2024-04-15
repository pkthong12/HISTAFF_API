using API.Main;

namespace API.DTO
{
    public class AdBookingDTO: BaseDTO
    {
        public long? EmpId { get; set; }
        public long? RoomId { get; set; }
        public DateTime? BookingDay { get; set; }
        public DateTime? HourFrom { get; set; }
        public DateTime? HourTo { get; set; }
        public string? Note { get; set; }
        public long? StatusId { get; set; }
        public long? ApprovedId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedNote { get; set; }
    }
}
