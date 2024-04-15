namespace PayrollDAL.Models
{
    /*
    [Table("PA_ADVANCE")] // tạm ứng lương
    public class Advance : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [ForeignKey("Employee")]
        public long EMPLOYEE_ID { get; set; }
        public int YEAR { get; set; } // kỳ ứng lương
        public int PERIOD_ID { get; set; } // kỳ ứng lương
        public int MONEY { get; set; } // số tiền ứng

        public DateTime? ADVANCE_DATE { get; set; }
        [ForeignKey("Signer")]
        public long? SIGN_ID { get; set; } // Người ký
        [MaxLength(50)]
        public string SIGNER_NAME { get; set; } // Tên người ký
        [MaxLength(50)]
        public string SIGNER_POSITION { get; set; } // Chức danh người ký
        public DateTime? SIGN_DATE { get; set; }


        [MaxLength(550)]
        public string NOTE { get; set; }
        //[ForeignKey("OtherListFix")]
        public int STATUS_ID { get; set; } // Trạng thái
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

        public Employee Signer { get; set; }
        public Employee Employee { get; set; }
        //public OtherListFix OtherListFix { get; set; }
    }
    */
    /*
    [Table("PA_ADVANCE_TMP")] // Tạm ứng lương Import
    public class AdvanceTmp 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        [MaxLength(50)]
        public string CODE_REF { get; set; } // Tên người ký
        [MaxLength(50)]
        public string EMPLOYEE_CODE { get; set; } // Tên người ký
        public long? EMPLOYEE_ID { get; set; }
        public int MONEY { get; set; } // số tiền ứng
        public DateTime? ADVANCE_DATE { get; set; }
        [MaxLength(50)]
        public string SIGNER_NAME { get; set; } // Tên người ký
        [MaxLength(50)]
        public string SIGNER_POSITION { get; set; } // Chức danh người ký
        public DateTime? SIGN_DATE { get; set; }
        [MaxLength(550)]
        public string NOTE { get; set; }
        [MaxLength(50)]
        public string STATUS_NAME { get; set; }
        public long? STATUS_ID { get; set; } // Trạng thái
    }
    */
}
