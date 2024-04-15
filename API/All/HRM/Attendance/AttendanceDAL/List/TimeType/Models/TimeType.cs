namespace AttendanceDAL.Models
{
    /*
    [Table("AT_TIME_TYPE")]
    public class TimeType 
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
        [ForeignKey("Symbol")]
        public long MORNING_ID { get; set; }
        [ForeignKey("Symbol2")]
        public long AFTERNOON_ID { get; set; }
        public bool? IS_OFF { get; set; }
        public bool? IS_FULLDAY { get; set; }
        public int ORDERS { get; set; }
        [MaxLength(1000)]
        public string NOTE { get; set; }
        [MaxLength(1000)]
        public bool IS_ACTIVE { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public Symbol Symbol { get; set; }
        public Symbol Symbol2 { get; set; }
    }
    */
}
