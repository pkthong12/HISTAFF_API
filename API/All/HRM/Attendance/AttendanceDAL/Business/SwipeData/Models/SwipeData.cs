namespace AttendanceDAL.Models
{
    /*
    [Table("AT_SWIPE_DATA")]
    public class SwipeData : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [ForeignKey("employee")]
        public long EMP_ID { get; set; }
        [MaxLength(50)]
        public string ITIME_ID { get; set; }
        public DateTime TIME_POINT { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        [MaxLength(450)]
        public string LATITUDE { get; set; }
        [MaxLength(450)]
        public string LONGITUDE { get; set; }
        [MaxLength(450)]
        public string MODEL { get; set; }
        [MaxLength(450)]
        public string IMAGE { get; set; }
        public string MAC { get; set; }
        [MaxLength(450)]
        public string OPERATING_SYSTEM { get; set; }
        [MaxLength(450)]
        public string OPERATING_VERSION { get; set; }
        [MaxLength(50)]
        public string WIFI_IP { get; set; }
        [MaxLength(550)]
        public string BSS_ID { get; set; }
        [DefaultValue("1")]
        public bool? IS_PORTAL { get; set; }
        public Employee employee { get; set; }
    }
    [Table("AT_SWIPE_DATA_TMP")]
    public class SwipeDataTmp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public int? TENANT_ID { get; set; }
        [MaxLength(150)]
        public string REF_CODE { get; set; }
        [ForeignKey("employee")]
        public long? EMP_ID { get; set; }
        [MaxLength(50)]
        public string ITIME_ID { get; set; }
        public DateTime TIME_POINT { get; set; }
        [MaxLength(450)]
        public string LATITUDE { get; set; }
        [MaxLength(450)]
        public string LONGITUDE { get; set; }
        [MaxLength(450)]
        public string MODEL { get; set; }
        [MaxLength(450)]
        public string IMAGE { get; set; }
        [MaxLength(450)]
        public string MAC { get; set; }
        [MaxLength(450)]
        public string OPERATING_SYSTEM { get; set; }
        [MaxLength(450)]
        public string OPERATING_VERSION { get; set; }
        [MaxLength(50)]
        public string WIFI_IP { get; set; }
        [MaxLength(550)]
        public string BSS_ID { get; set; }
        public Employee employee { get; set; }
    }
    [Table("AT_TIMESHEET_MACHINE")]
    public class TimesheetMachine  // Bảng swipe data tu bang cham cong goc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int? TENANT_ID { get; set; }
        public int? PERIOD_ID { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public DateTime? WORKINGDAY { get; set; }
        public long? TIMETYPE_ID { get; set; }
        public string TIME_POINT1 { get; set; }
        public string TIME_POINT4 { get; set; }
        public string TIME_POINT_OT { get; set; }
        public string OT_START { get; set; }
        public string OT_END { get; set; }
        public int? OT_LATE_IN { get; set; }
        public int? OT_EARLY_OUT { get; set; }
        public int? OT_TIME { get; set; }
        public int? OT_TIME_NIGHT { get; set; }
        public bool? IS_REGISTER_OFF { get; set; }
        public bool? IS_REGISTER_LATE_EARLY { get; set; }
        public bool? IS_HOLIDAY { get; set; }
        public int? LATE_IN { get; set; }
        public int? EARLY_OUT { get; set; }
        public string HOURS_START { get; set; }
        public string HOURS_STOP { get; set; }
        public bool? IS_EDIT_IN { get; set; }
        public bool? IS_EDIT_OUT { get; set; }
        public string NOTE { get; set; }
        public long? MORNING_ID { get; set; }
        public long? AFTERNOON_ID { get; set; }

    }
    //[Table("SYS_USER_ORG_TMP")]
    //public class UserOrgTemp  // Bảng swipe data tu bang cham cong goc
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public long Id { get; set; }
    //    public string USER_ID { get; set; }
    //    public long? ORG_ID { get; set; }
    //    public long? TENANT_ID { get; set; }
    //}

    [Table("SYS_SETTING_MAP")]
    public class SettingMap : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(1500)]
        public string ADDRESS { get; set; }
        public decimal? RADIUS { get; set; }
        public string LAT { get; set; }
        public string LNG { get; set; }
        public int? ZOOM { get; set; }
        public string CENTER { get; set; }
        public int? ORG_ID { get; set; }
        [MaxLength(150)]
        public string IP { get; set; }
        [MaxLength(150)]
        public string BSSID { get; set; }
        [MaxLength(450)]
        public string QRCODE { get; set; }
        
        [DefaultValue("1")]
        public bool? IS_ACTIVE { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

    }
    [Table("AT_SWIPE_DATA_WRONG")]
    public class SwipeDataWrong
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public int? TENANT_ID { get; set; }
        public long? EMP_ID { get; set; }
        [MaxLength(50)]
        public string ITIME_ID { get; set; }
        public DateTime? TIME_POINT { get; set; }
        [MaxLength(450)]
        public string LATITUDE { get; set; }
        [MaxLength(450)]
        public string LONGITUDE { get; set; }
        [MaxLength(450)]
        public string MODEL { get; set; }
        [MaxLength(450)]
        public string IMAGE { get; set; }
        [MaxLength(450)]
        public string MAC { get; set; }
        [MaxLength(450)]
        public string OPERATING_SYSTEM { get; set; }
        [MaxLength(450)]
        public string OPERATING_VERSION { get; set; }
        [MaxLength(50)]
        public string WIFI_IP { get; set; }
        [MaxLength(550)]
        public string BSS_ID { get; set; }
    }
    [Table("AT_TIMESHEET_MACHINE_EDIT")]
    public class TimesheetMachineEdit  // Bảng swipe data tu bang cham cong goc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int? TENANT_ID { get; set; }
        public long? PERIOD_ID { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public DateTime? WORKINGDAY { get; set; }
        public string TIME_POINT1 { get; set; }
        public string TIME_POINT4 { get; set; }
        public bool? IS_EDIT_IN { get; set; }
        public bool? IS_EDIT_OUT { get; set; }
        public string NOTE { get; set; }

    }
    */

}
