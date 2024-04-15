using Common.Paging;

namespace AttendanceDAL.ViewModels
{
    public class SwipeDataDTO : Pagings
    {
        public int ORG_ID { get; set; }
        public int PERIOD_ID { get; set; }
        public int IS_QUIT { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string EMPLOYEE_CODE { get; set; }
        public string ORG_NAME { get; set; }
        public string POSITION_NAME { get; set; }
    }
    public class SwipeDataGPRSDTO
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Model { get; set; }// ten may
        public string Image { get; set; }
        public string OperatingSystem { get; set; }
        public string OperatingVersion { get; set; }
        public string Mac { get; set; }
        public string WifiIp { get; set; }
        public string Bssid { get; set; }
    }

    public class SwipeDataReport
    {
        public int OrgId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
