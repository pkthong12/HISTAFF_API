namespace PayrollDAL.Models
{
    /*
    [Table("PA_FORMULA")] // công thức lương
    public class Formula : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string COL_NAME { get; set; }
        [ForeignKey("SalaryType")]
        public int SALARY_TYPE_ID { get; set; }
        [Required]
        [MaxLength(1000)]
        public string FORMULA { get; set; }
        [MaxLength(1000)]
        public string FORMULA_NAME { get; set; }
        public int? ORDERS { get; set; }
        public int? IS_DAILY { get; set; }
        public bool? IS_ACTIVE { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public SalaryType SalaryType { get; set; }
    }

    */
}
