namespace AttendanceDAL.Models
{
    /*
    [Table("AT_APPROVED")]
    public class Approved : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [ForeignKey("RegisterOff")]
        public long REGISTER_ID { get; set; }       
        // [ForeignKey("employee")]
        public long EMP_RES_ID { get; set; } // Người duyệt
        // [ForeignKey("employee")]
        public long APPROVE_ID { get; set; } // Người duyệt
        [MaxLength(150)]
        public String APPROVE_NAME { get; set; } // Người duyệt
        [MaxLength(150)]
        public string APPROVE_POS { get; set; } // Chức vụ người phê duyệt
        public DateTime? APPROVE_DAY { get; set; }// Ngày duyệt       
        [MaxLength(550)]
        public string APPROVE_NOTE { get; set; }
        public int TYPE_ID { get; set; } // Loại đăng ký
        public int IS_REG { get; set; } // Đăng ký, phê duyệt
        public int STATUS_ID { get; set; } // Trang thai phe 
        public int IS_READ { get; set; } // Trang thai đã xem 
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public Employee employee { get; set; }
        public RegisterOff RegisterOff { get; set; }
        public TimeType TimeType { get; set; }
    }
    */
}
