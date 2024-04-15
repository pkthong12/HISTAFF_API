namespace ProfileDAL.Models
{
    /*
    [Table("HU_CONTRACT")]
    public class Contract : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [ForeignKey("Employee")]
        public long? EMPLOYEE_ID { get; set; }
        [Required]
        [MaxLength(30)]
        public string CONTRACT_NO { get; set; }
        [ForeignKey("ContractType")]
        public int CONTRACT_TYPE_ID { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? EXPIRE_DATE { get; set; }
        [ForeignKey("Signer")]
        public long? SIGN_ID { get; set; } // Người ký
        [MaxLength(50)]
        public string SIGNER_NAME { get; set; } // Tên người ký
        [MaxLength(50)]
        public string SIGNER_POSITION { get; set; } // Chức danh người ký
        public DateTime? SIGN_DATE { get; set; }

        public decimal SAL_BASIC { get; set; }

        public decimal SAL_PERCENT { get; set; }

        [MaxLength(550)]
        public string NOTE { get; set; }
        //[ForeignKey("OtherListFix")]
        public int STATUS_ID { get; set; } // Trạng thái
        [ForeignKey("Working")]
        public Int64? WORKING_ID { get; set; } // QĐ
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
      
        public Employee Signer { get; set; }
        public Employee Employee { get; set; }
        public ContractType ContractType { get; set; }
        //public OtherListFix OtherListFix { get; set; }
        public Working Working { get; set; }

       
    }
    [Table("TMP_HU_CONTRACT")]
    public class ContractTmp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(150)]
        public string REF_CODE { get; set; }
        [MaxLength(150)]
        public string CODE { get; set; } // Mã nhân viên
        public Int64? EMPLOYEE_ID { get; set; }
        public DateTime? DATE_START { get; set; }
        public DateTime? DATE_END { get; set; }
        [MaxLength(50)]
        public string CONTRACT_NO { get; set; } // Số Quyết định
        public string CONTRACT_TYPE_NAME { get; set; } // Tên loại QĐ
        public int? CONTRACT_TYPE_ID { get; set; } // Loại Quyết định
        public decimal? SAL_BASIC { get; set; } // Lương cơ bản
        public decimal? SAL_TOTAL { get; set; } // Tổng lương
        public decimal? SAL_PERCENT { get; set; } // Tỷ lệ hưởng lương
        [MaxLength(50)]
        public string STATUS_NAME { get; set; } // Trạng thái
        public long? STATUS_ID { get; set; } // Trạng thái
        public DateTime? SIGN_DATE { get; set; } // Ngày ký
        [MaxLength(50)]
        public string SIGNER_NAME { get; set; } // Tên người ký
        [MaxLength(50)]
        public string SIGNER_POSITION { get; set; } // Chức danh người ký

    }
    */
}
