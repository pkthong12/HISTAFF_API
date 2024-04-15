namespace AttendanceDAL.Models
{
    /*
    [Table("AT_SALARY_PERIOD")]// ky luong
    public class SalaryPeriod : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string NAME { get; set; }
        public int YEAR { get; set; }
        public int? MONTH { get; set; }
        public DateTime DATE_START { get; set; }
        public DateTime DATE_END { get; set; }
        public double STANDARD_WORKING { get; set; }
        public decimal? STANDARD_TIME { get; set; }
        public string NOTE { get; set; }
        [MaxLength(1000)]
        public bool IS_ACTIVE { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

    }
    [Table("AT_SALARY_PERIOD_DTL")]
    public class SalaryPeriodDtl : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [ForeignKey("Period")]
        public long PERIOD_ID { get; set; }
        public int? ORG_ID { get; set; }
        public long? EMP_ID { get; set; }
        public decimal WORKING_STANDARD { get; set; }
        public decimal? STANDARD_TIME { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public SalaryPeriod Period { get; set; }

    }
    */
}
