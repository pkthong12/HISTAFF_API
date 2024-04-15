namespace ProfileDAL.Models
{
    /*
    [Table("AD_BOOKING")] // Đặt phòng họp
    public class Booking : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        public int EMP_ID { get; set; }
        [ForeignKey("room")]
        public int ROOM_ID { get; set; }
        public DateTime BOOKING_DAY { get; set; }
        [Required]
        [MaxLength(10)]
        public DateTime HOUR_FROM { get; set; }
        [Required]
        [MaxLength(10)]
        public DateTime HOUR_TO { get; set; }
        [MaxLength(1500)]
        public string NOTE { get; set; }
        public int STATUS_ID { get; set; }
        public int? APPROVED_ID { get; set; }
        public DateTime? APPROVED_DATE { get; set; }
        [MaxLength(1500)]
        public string APPROVED_NOTE { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public Room room { get; set; }
    }
    */
}
