namespace AttendanceDAL.Models
{
    /*
    [Table("AT_REGISTER_OFF")]
    public class RegisterOff : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [ForeignKey("employee")]
        public long EMPLOYEE_ID { get; set; }
        //public DateTime WORKINGDAY { get; set; } 
        public DateTime DATE_START { get; set; }
        public DateTime DATE_END { get; set; }
        public DateTime? TIME_START { get; set; } // Thời gian bắt đầu
        public DateTime? TIME_END { get; set; } // Thời gian kết thúc
        public DateTime? WORKING_DAY { get; set; } // Ngày đăng ký
        public int? TIME_LATE { get; set; }// so phut di muon
        public int? TIME_EARLY { get; set; }// so phut ve tre
        [ForeignKey("TimeType")]
        public long? TIMETYPE_ID { get; set; }
        [MaxLength(550)]
        public string NOTE { get; set; }
        public int TYPE_ID { get; set; } // Loại đăng ký
        public int STATUS_ID { get; set; } // Trang thai phe 
        public long? APPROVE_ID { get; set; } // Người duyệt
        [MaxLength(150)]
        public String APPROVE_NAME { get; set; } // Người duyệt
        [MaxLength(150)]
        public string APPROVE_POS { get; set; } // Chức vụ người phê duyệt
        public DateTime? APPROVE_DAY { get; set; }// Ngày duyệt       
        [MaxLength(550)]
        public string APPROVE_NOTE { get; set; }
        [DefaultValue(true)]
        public bool IS_ACTIVE { get; set; }// Trang thai da xoa hay chua
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public TimeType TimeType { get; set; }
        public Employee employee { get; set; }
    }
    */
}
