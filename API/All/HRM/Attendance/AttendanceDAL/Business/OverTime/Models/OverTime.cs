namespace AttendanceDAL.Models
{
    /*
    [Table("AT_OVERTIME")]
    public class OverTime : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [ForeignKey("employee")]
        public long EMPLOYEE_ID { get; set; }
        public int? PERIOD_ID { get; set; }
        public DateTime? WORKING_DAY { get; set; }// Ngày
        public DateTime? TIME_START { get; set; } // Thời gian bắt đầu
        public DateTime? TIME_END { get; set; } // Thời gian kết thúc
        public long? STATUS_ID { get; set; } // Trang thái
        public bool? IS_ACTIVE { get; set; } // Trang thái xóa
        public string NOTE { get; set; } // Ghi chú 
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

    }
    [Table("AT_OVERTIME_CONFIG")]
    public class OverTimeConfig : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        public int? HOUR_MIN { get; set; }
        public int? HOUR_MAX { get; set; }
        public decimal? FACTOR_NT { get; set; }// he so ngay thuong
        public decimal? FACTOR_NN { get; set; }// ngay nghi
        public decimal? FACTOR_NL { get; set; }// ngay le
        public decimal? FACTOR_DNT { get; set; }// dem ngay thuong
        public decimal? FACTOR_DNN { get; set; }// dem ngay nghi
        public decimal? FACTOR_DNL { get; set; }// dem ngay le
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

    }
    */
}
