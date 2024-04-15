namespace PayrollDAL.Models
{
    /*
    [Table("PA_ELEMENT")] // PHẦN TỬ LƯƠNG
    public class SalaryElement : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string CODE { get; set; }
        [Required]
        [MaxLength(100)]
        public string NAME { get; set; }
        [ForeignKey("ElementGroup")]
        public long GROUP_ID { get; set; }
        public bool IS_SYSTEM { get; set; }
        public bool IS_ACTIVE { get; set; }
        public int ORDERS { get; set; }
        public int DATA_TYPE { get; set; } // 0: Kiểu số; 1 kiểu TEXT
        [MaxLength(550)]
        public string NOTE { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public ElementGroup ElementGroup { get; set; }
    }
    [Table("HU_WORKING")]// Quyết định cuối cùng
    public class Working : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }
        
       
        public Int64 EMPLOYEE_ID { get; set; }
    
        public int POSITION_ID { get; set; }
       
        public int ORG_ID { get; set; }
        public DateTime? EFFECT_DATE { get; set; }
        public DateTime? EXPIRE_DATE { get; set; }
        [Required]
        [MaxLength(30)]
        public string DECISION_NO { get; set; } // Số Quyết định
      
        public int TYPE_ID { get; set; } // Loại Quyết định
    
        public long? SALARY_TYPE_ID { get; set; } // Bảng lương
   
        public long? SALARY_SCALE_ID { get; set; } // Thang lương
        
        public long? SALARY_RANK_ID { get; set; } // Ngạch lương

        public int? SALARY_LEVEL_ID { get; set; } // Bậc lương
        public decimal? SAL_BASIC { get; set; } // Lương cơ bản
        public decimal? SAL_TOTAL { get; set; } // Tổng lương
        public decimal? SAL_PERCENT { get; set; } // Tỷ lệ hưởng lương
 
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
    
    }
    */
}
