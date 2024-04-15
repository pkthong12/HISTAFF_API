namespace CoreDAL.Models
{
    /*
    [Table("SYS_FUNCTION")]
    public class SysFunction : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("SysGroupFunction")]
        public int? GROUP_ID { get; set; }
        [Required]
        [ForeignKey("SysModule")]
        public int MODULE_ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string NAME { get; set; }
        [Required]
        [MaxLength(50)]
        public string CODE { get; set; }
        [MaxLength(50)]
        public string STATES { get; set; }
        [DefaultValue("1")]
        public bool? IS_ACTIVE { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

        public SysGroupFunction SysGroupFunction { get; set; }
        public SysModule SysModule { get; set; }

    }
    */
}
