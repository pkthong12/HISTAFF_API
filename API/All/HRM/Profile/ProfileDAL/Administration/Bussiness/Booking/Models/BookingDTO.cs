using Common.Paging;
namespace ProfileDAL.ViewModels
{
    public class BookingDTO : Pagings
    {
        public long Id { get; set; }
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public long RoomId { get; set; }
        public string RoomName { get; set; }
        public string OrgName { get; set; }
        public DateTime BookingDay { get; set; }
        public DateTime HourFrom { get; set; }
        public DateTime HourTo { get; set; }
        public string Note { get; set; }
        public long? StatusId { get; set; }
        public string StatusName { get; set; }
        public string ApproveName { get; set; }
        public string ApproveNote { get; set; }
        public DateTime? ApproveDate { get; set; }
        public DateTime? CreateDate { get; set; }
    }

    public class BookingInputDTO
    {
        public long? Id { get; set; }
        public long RoomId { get; set; }
        public DateTime BookingDay { get; set; }
        public DateTime HourFrom { get; set; }
        public DateTime HourTo { get; set; }
        public string Note { get; set; }
    }
    
}
