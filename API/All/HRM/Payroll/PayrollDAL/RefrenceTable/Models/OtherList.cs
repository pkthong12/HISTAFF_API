namespace PayrollDAL.Models
{
    /*
    [Table("SYS_OTHER_LIST")]
    public class OtherList : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string CODE { get; set; }

        [Required]
        [MaxLength(100)]
        public string NAME { get; set; }

        [MaxLength(1000)]
        public string NOTE { get; set; }

        [ForeignKey("SysOtherListType")]
        public int? TYPE_ID { get; set; }

        public int? ORDERS { get; set; }

        [DefaultValue("1")]
        public bool? IS_ACTIVE { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

        public OtherListType SysOtherListType { get; set; }
    }
    */
}
