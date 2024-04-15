namespace ProfileDAL.Models
{
    /*
    [Table("SYS_POSITION")]// Chức danh
    public class PositionSys : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [ForeignKey("GroupPosition")]
        public  int GROUP_ID { get; set; }
        [Required]
        public long AREA_ID { get; set; }
        [Required]
        [MaxLength(50)]
        public string CODE { get; set; }

        [Required]
        [MaxLength(100)]
        public string NAME { get; set; }

        [MaxLength(1500)]
        public string NOTE { get; set; }

        public string JOB_DESC { get; set; }

        [DefaultValue("1")]
        public bool? IS_ACTIVE { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public GroupPositionSys GroupPosition { get; set; }

    }
    */
}
