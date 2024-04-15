namespace ProfileDAL.Models
{
    /*
    [Table("SYS_SHIFT")]// ca lam viec
    public class ShiftSys : IAuditableEntity
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
        [Required]
        public long AREA_ID { get; set; }
        public DateTime HOURS_START { get; set; }
        public DateTime HOURS_STOP { get; set; }
        public DateTime BREAKS_FROM { get; set; }// thoi gian nghi tu
        public DateTime BREAKS_TO { get; set; }// thoi gian nghi den
        public int? TIME_LATE { get; set; }// thời gian đi muộn 
        public int? TIME_EARLY { get; set; }// thời gian về sớm
        public long TIME_TYPE_ID { get; set; }
        public bool? IS_NOON { get; set; }// ca đêm 
        public bool? IS_BREAK { get; set; }// có nghỉ
        [MaxLength(1000)]
        public string NOTE { get; set; }
        [MaxLength(1000)]
        public bool IS_ACTIVE { get; set; }

        //lich trinh lam viec
        public long? MON_ID { get; set; }
        public long? TUE_ID { get; set; }
        public long? WED_ID { get; set; }
        public long? THU_ID { get; set; }
        public long? FRI_ID { get; set; }
        public long? SAT_ID { get; set; }
        public long? SUN_ID { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }
    */
}
