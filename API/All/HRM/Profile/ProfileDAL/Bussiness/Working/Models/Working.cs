namespace ProfileDAL.Models
{
    /*
    [Table("HU_WORKING")]// Quyết định
    public class Working : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }
        public int? TENANT_ID { get; set; }
        [ForeignKey("Employee")]
        public Int64 EMPLOYEE_ID { get; set; }
        [ForeignKey("Position")]
        public int POSITION_ID { get; set; }
        [ForeignKey("Organization")]
        public int ORG_ID { get; set; }
        public DateTime? EFFECT_DATE { get; set; }
        public DateTime? EXPIRE_DATE { get; set; }
        [Required]
        [MaxLength(30)]
        public string DECISION_NO { get; set; } // Số Quyết định
        [ForeignKey("OtherList")]
        public int TYPE_ID { get; set; } // Loại Quyết định
        [ForeignKey("SalaryType")]
        public long? SALARY_TYPE_ID { get; set; } // Bảng lương
        [ForeignKey("SalaryScale")]
        public long? SALARY_SCALE_ID { get; set; } // Thang lương
        [ForeignKey("SalaryRank")]
        public long? SALARY_RANK_ID { get; set; } // Ngạch lương
        [ForeignKey("SalaryLevel")]
        public int? SALARY_LEVEL_ID { get; set; } // Bậc lương
        public decimal? COEFFICIENT { get; set; }
        public decimal? SAL_BASIC { get; set; } // Lương cơ bản
        public decimal? SAL_TOTAL { get; set; } // Tổng lương
        public decimal? SAL_PERCENT { get; set; } // Tỷ lệ hưởng lương

        public bool? IS_CHANGE_SAL { get; set; }// Là quyết định thay đổi lương, khi tính lương sẽ check quyết định này
        [ForeignKey("Signer")]
        public Int64? SIGN_ID { get; set; } // Người ký
        [MaxLength(50)]
        public string SIGNER_NAME { get; set; } // Tên người ký
        [MaxLength(50)]
        public string SIGNER_POSITION { get; set; } // Chức danh người ký
        public DateTime? SIGN_DATE { get; set; } // Ngày ký
        [MaxLength(550)]
        public string NOTE { get; set; }
        //[ForeignKey("OtherListFix")]
        //[Column(Order = 1)]
        public int STATUS_ID { get; set; } // Trạng thái

        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public Employee Employee { get; set; }
        public Employee Signer { get; set; }
        public Position Position { get; set; }
        public Organization Organization { get; set; }
        //public OtherListFix OtherListFix { get; set; }
        public OtherList OtherList { get; set; }
        public SalaryType SalaryType { get; set; }
        public SalaryScale SalaryScale { get; set; }
        public SalaryRank SalaryRank { get; set; }
        public SalaryLevel SalaryLevel { get; set; }
    }
    [Table("TMP_HU_WORKING")]// Quyết định tạm
    public class WorkingTmp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(150)]
        public string REF_CODE { get; set; }
        [MaxLength(150)]
        public string CODE { get; set; } // Mã nhân viên
        public Int64? EMPLOYEE_ID { get; set; }
        [MaxLength(250)]
        public string POS_NAME { get; set; } // Mã nhân viên
        public int? POSITION_ID { get; set; }
        public int ORG_ID { get; set; }
        public DateTime? EFFECT_DATE { get; set; }
        public DateTime? EXPIRE_DATE { get; set; }
        [MaxLength(50)]
        public string DECISION_NO { get; set; } // Số Quyết định
        public string TYPE_NAME { get; set; } // Tên loại QĐ
        public int? TYPE_ID { get; set; } // Loại Quyết định
        [MaxLength(50)]
        public string SALARY_TYPE_NAME { get; set; } // Tên Bảng lương
        public long? SALARY_TYPE_ID { get; set; } // Bảng lương
        public int? SALARY_LEVEL_ID { get; set; } // Bậc lương
        public decimal? SAL_BASIC { get; set; } // Lương cơ bản
        public decimal? COEFFICIENT { get; set; }
        public decimal? SAL_TOTAL { get; set; } // Tổng lương
        public decimal? SAL_PERCENT { get; set; } // Tỷ lệ hưởng lương
        [MaxLength(50)]
        public string STATUS_NAME { get; set; } // Tên Bảng lương
        public long? STATUS_ID { get; set; } // Trạng thái
        public DateTime? SIGN_DATE { get; set; } // Ngày ký
        [MaxLength(50)]
        public string SIGNER_NAME { get; set; } // Tên người ký
        [MaxLength(50)]
        public string SIGNER_POSITION { get; set; } // Chức danh người ký
    }
    */
}
