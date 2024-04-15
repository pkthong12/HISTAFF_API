namespace CoreDAL.Models
{
    /*
    [Table("SYS_GROUP_PERMISSION")]
    public class SysGroupPermission : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey("SysGroupUser")]
        public int GROUP_ID { get; set; }
        [ForeignKey("SysFunction")]
        public long FUNCTION_ID { get; set; }
        [MaxLength(500)]
        public string PERMISSION_STRING { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public SysGroupUser SysGroupUser { get; set; }
        public SysFunction SysFunction { get; set; }
    }
    */
}
