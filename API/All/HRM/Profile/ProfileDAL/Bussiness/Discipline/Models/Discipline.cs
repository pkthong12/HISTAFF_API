namespace ProfileDAL.Models
{
    /*
    [Table("HU_DISCIPLINE")]
    public class Discipline : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public long TENANT_ID { get; set; }

        [ForeignKey("Employee")]
        public Int64? EMPLOYEE_ID { get; set; }
        [ForeignKey("Organization")]
        public int? ORG_ID { get; set; }
        [ForeignKey("Position")]
        public int? POSITION_ID { get; set; }
        public DateTime EFFECT_DATE { get; set; }
        [Required]
        [MaxLength(30)]
        public string NO { get; set; }
        public DateTime SIGN_DATE { get; set; }
        [ForeignKey("Employee2")]
        public Int64? SIGN_ID { get; set; } // Người ký
        [MaxLength(50)]
        public string SIGNER_NAME { get; set; } // Tên người ký
        [MaxLength(50)]
        public string SIGNER_POSITION { get; set; } // Chức danh người ký
        //[ForeignKey("OtherListFix")]
        public int STATUS_ID { get; set; } // Trạng thái
        //[ForeignKey("OtherListFix")]
        public int DISCIPLINE_OBJ_ID { get; set; } // Đối tượng kỷ luật
        [MaxLength(550)]
        public string DISCIPLINE_TYPE { get; set; } // Hình thức kỷ luật
        [MaxLength(1000)]
        public string REASON { get; set; } // Lý do kỷ luật
        public double MONEY { get; set; } // Mức phạt
        public Boolean IS_SALARY { get; set; } // trừ vào kỳ lương không
        public int YEAR { get; set; } // Năm
        public int PERIOD_ID { get; set; } // kỳ lương
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Employee Employee2 { get; set; }
        //public OtherListFix OtherListFix { get; set; }
        public Organization Organization { get; set; }
    }
    */
}
