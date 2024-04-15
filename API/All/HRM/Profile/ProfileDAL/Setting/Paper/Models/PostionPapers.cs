namespace ProfileDAL.Models
{
    /*
    [Table("HU_POS_PAPER")]// Chức danh
    public class PostionPapers : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey("position")]
        public int POS_ID { get; set; }
        [ForeignKey("paper")]
        public int PAPER_ID { get; set; }
        
        [DefaultValue("1")]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public OtherList paper { get; set; }
        public Position position { get; set; }
    }
    */
}
