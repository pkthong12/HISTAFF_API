namespace ProfileDAL.Models
{
    /*
    [Table("HU_COMMEND")]
    public class Commend : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }
        public Int64 TENANT_ID { get; set; }
        public DateTime EFFECT_DATE { get; set; }
        [Required]
        [MaxLength(30)]
        public string NO { get; set; }
        public DateTime SIGN_DATE { get; set; }
        [ForeignKey("Employee")]
        public Int64? SIGN_ID { get; set; } // Người ký
        [MaxLength(50)]
        public string SIGNER_NAME { get; set; } // Tên người ký
        [MaxLength(50)]
        public string SIGNER_POSITION { get; set; } // Chức danh người ký
        [ForeignKey("Organization")]
        public int? ORG_ID { get; set; } // Phòng ban
        //[ForeignKey("COMMENDOBJID")]
        public long? COMMEND_OBJ_ID { get; set; } // Đối tượng khen thưởng
        //[ForeignKey("SOURCECOST")]
        [MaxLength(250)]
        public long? SOURCE_COST_ID { get; set; } // Nguồn chi
        [MaxLength(550)]
        public string COMMEND_TYPE { get; set; } // Hình thức khen thưởng
        [MaxLength(1000)]
        public string REASON { get; set; } // Lý do khen thưởng
        //[ForeignKey("STATUSID")]
        public long? STATUS_ID { get; set; } // Trạng thái
        public double? MONEY { get; set; } // Mức thưởng
        public Boolean IS_TAX { get; set; } // Trạng thái
        public int? PERIOD_ID { get; set; } // kỳ lương
        public int? YEAR { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public Employee Employee { get; set; }
        public Organization Organization { get; set; }
        //public OtherListFix STATUSID { get; set; }
        //public OtherListFix COMMENDOBJID { get; set; }


    }
    */
}
