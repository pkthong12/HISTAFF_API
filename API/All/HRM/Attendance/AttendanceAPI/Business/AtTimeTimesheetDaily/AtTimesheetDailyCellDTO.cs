namespace API.All.HRM.Attendance.AttendanceAPI.Business.AtTimeTimesheetDaily
{
    public class AtTimesheetDailyCellValue
    {
        public int Day { get; set; }
        public string? Value { get; set; }
    }

    public class AtTimesheetDailyCellColor
    {
        public int Day { get; set; }
        public int? ColorCode { get; set; }
    }
    public class AtTimesheetDailyCellDTO
    {
        // Nếu bạn thêm các member mới cho class này, vui lòng không dùng các chữ cái D ở đầu tên gọi (vì D đã dành riêng cho thuật toán đã dùng)
        public long STT { get; set; }
        public string? VN_FULLNAME { get; set; }
        public string? EMPLOYEE_CODE { get; set; }
        public string? EMPLOYEE_OBJ { get; set; }
        public string? POSITION_NAME { get; set; }
        public long? ORG_ID { get; set; }
        public string? ORG_NAME { get; set; }
        public string? ORG_PATH { get; set; }
        public string? TITLE_NAME { get; set; }
        public int? JOB_ORDER_NUM { get; set; }
        public string? D01 { get; set; }
        public string? D02 { get; set; }
        public string? D03 { get; set; }
        public string? D04 { get; set; }
        public string? D05 { get; set; }
        public string? D06 { get; set; }
        public string? D07 { get; set; }
        public string? D08 { get; set; }
        public string? D09 { get; set; }
        public string? D10 { get; set; }
        public string? D11 { get; set; }
        public string? D12 { get; set; }
        public string? D13 { get; set; }
        public string? D14 { get; set; }
        public string? D15 { get; set; }
        public string? D16 { get; set; }
        public string? D17 { get; set; }
        public string? D18 { get; set; }
        public string? D19 { get; set; }
        public string? D20 { get; set; }
        public string? D21 { get; set; }
        public string? D22 { get; set; }
        public string? D23 { get; set; }
        public string? D24 { get; set; }
        public string? D25 { get; set; }
        public string? D26 { get; set; }
        public string? D27 { get; set; }
        public string? D28 { get; set; }
        public string? D29 { get; set; }
        public string? D30 { get; set; }
        public string? D31 { get; set; }
        public int? D1_COLOR { get; set; }
        public int? D2_COLOR { get; set; }
        public int? D3_COLOR { get; set; }
        public int? D4_COLOR { get; set; }
        public int? D5_COLOR { get; set; }
        public int? D6_COLOR { get; set; }
        public int? D7_COLOR { get; set; }
        public int? D8_COLOR { get; set; }
        public int? D9_COLOR { get; set; }
        public int? D10_COLOR { get; set; }
        public int? D11_COLOR { get; set; }
        public int? D12_COLOR { get; set; }
        public int? D13_COLOR { get; set; }
        public int? D14_COLOR { get; set; }
        public int? D15_COLOR { get; set; }
        public int? D16_COLOR { get; set; }
        public int? D17_COLOR { get; set; }
        public int? D18_COLOR { get; set; }
        public int? D19_COLOR { get; set; }
        public int? D20_COLOR { get; set; }
        public int? D21_COLOR { get; set; }
        public int? D22_COLOR { get; set; }
        public int? D23_COLOR { get; set; }
        public int? D24_COLOR { get; set; }
        public int? D25_COLOR { get; set; }
        public int? D26_COLOR { get; set; }
        public int? D27_COLOR { get; set; }
        public int? D28_COLOR { get; set; }
        public int? D29_COLOR { get; set; }
        public int? D30_COLOR { get; set; }
        public int? D31_COLOR { get; set; }

        public List<AtTimesheetDailyCellValue>? Values { get; set; }
        public List<AtTimesheetDailyCellColor>? ColorCodes { get; set; }

    }
}
