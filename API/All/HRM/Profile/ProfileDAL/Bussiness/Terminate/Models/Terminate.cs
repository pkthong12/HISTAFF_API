namespace ProfileDAL.Models
{
    /*
    [Table("HU_TERMINATE")]// Quyết định cuối cùng
    public class Terminate : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [ForeignKey("Employee")]
        public long EMPLOYEE_ID { get; set; }
        public DateTime? EFFECT_DATE { get; set; } // Ngày hiệu lực
        public DateTime? SEND_DATE { get; set; }
        public DateTime? LAST_DATE { get; set; } // Ngày làm việc cuối cùng
        [MaxLength(1000)]
        public string TER_REASON { get; set; } // Lý do nghỉ việc
        [ForeignKey("Signer")]
        public long? SIGN_ID { get; set; } // Người ký
        [MaxLength(50)]
        public string SIGNER_NAME { get; set; } // Tên người ký
        [MaxLength(50)]
        public string SIGNER_POSITION { get; set; } // Chức danh người ký
        public DateTime? SIGN_DATE { get; set; } // Ngày ký
        [ForeignKey("OtherListFix")]
        public int STATUS_ID { get; set; } // Trạng thái
        [Required]
        [MaxLength(30)]
        public string DECISION_NO { get; set; } // Số Quyết định
        public double? AMOUNT_VIOLATIONS { get; set; } // Số tien vi pham thoi han bao truoc
        public double? TRAINING_COSTS { get; set; } // Số tien chi phí đào tạo
        public double? OTHER_COMPENSATION { get; set; } // boi thuong khac

        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public Employee Employee { get; set; }
        public Employee Signer { get; set; }
    }
    */
}
