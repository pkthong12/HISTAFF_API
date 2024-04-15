namespace PayrollDAL.Models
{
    /*
    [Table("PA_KPI_TARGET")] // PHẦN TỬ LƯƠNG
    public class KpiTarget : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string CODE { get; set; }
        [Required]
        [MaxLength(255)]
        public string NAME { get; set; }
        [ForeignKey("KpiGroup")]
        public long KPI_GROUP_ID { get; set; }
        [MaxLength(255)] // đơn vị
        public string UNIT { get; set; }
        public int? COL_ID { get; set; }
        [MaxLength(100)]
        public string COL_NAME { get; set; }
        public int? MAX_VALUE { get; set; }
        public bool? IS_REAL_VALUE { get; set; }
        public bool? IS_PAY_SALARY { get; set; }
        public bool? IS_IMPORT_KPI { get; set; }
        public bool IS_ACTIVE { get; set; }
        public bool? IS_SYSTEM { get; set; }
        public int? TYPE_ID { get; set; } // Theo ngày 0 , theo tháng 1
        public int ORDERS { get; set; }
        [MaxLength(550)]
        public string NOTE { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public KpiGroup KpiGroup { get; set; }
    }
    */
}
