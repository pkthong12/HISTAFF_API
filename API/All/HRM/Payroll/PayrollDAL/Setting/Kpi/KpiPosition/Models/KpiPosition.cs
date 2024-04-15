namespace PayrollDAL.Models
{
    /*
    [Table("PA_KPI_POSITION")] // PHẦN TỬ LƯƠNG - THEO VỊ TRÍ
    public class KpiPosition : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
       
        [ForeignKey("KpiTarget")]
        public long KPI_TARGET_ID { get; set; }
        [ForeignKey("Position")]
        public int POSITION_ID { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public KpiTarget KpiTarget { get; set; }
        public Position Position { get; set; }
    }
    */
}
