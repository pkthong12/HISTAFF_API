namespace API.All.HRM.Attendance.AttendanceAPI.Business.AtTimeTimesheetDaily
{
    public class GetAttendantNoteByMonthDTO
    {
        public long? EmployeeId { get; set; }
        public int? Year { get; set; }
        public int? MonthIndex { get; set; }

        public int? LastDay { get; set; }
        public int? Date { get; set; }
        //public DateTime FisrtDateOfMonth { get; set; }
        //public DateTime LastDateOfMonth { get; set; }
    }
}
