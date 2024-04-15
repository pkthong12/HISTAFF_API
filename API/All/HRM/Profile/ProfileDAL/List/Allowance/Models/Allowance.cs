namespace ProfileDAL.Models
{
    /*
    [Table("HU_ALLOWANCE")] // Phụ Cấp
    public class Allowance : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string CODE { get; set; }
        [MaxLength(50)]
        public string COL_NAME { get; set; } // ten cot o bang payroll sheet
        [Required]
        [MaxLength(100)]
        public string NAME { get; set; }

        //[ForeignKey("TYPE")]
        public int? TYPE_ID { get; set; }// Loại hưởng
        [DefaultValue("0")]
        public bool? IS_INSURANCE { get; set; } // đóng bảo hiểm
        public bool? IS_ACTIVE { get; set; }
        [MaxLength(1500)]
        public string NOTE { get; set; }
        public bool? IS_FULLDAY { get; set; }
        

        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        //public OtherListFix TYPE { get; set; }
    }
    */
}
