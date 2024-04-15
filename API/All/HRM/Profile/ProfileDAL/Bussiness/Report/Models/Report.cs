namespace ProfileDAL.Models
{
    /*
    [Table("HU_REPORT")]
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string NAME { get; set; }
        public string CODE { get; set; }
        [ForeignKey("Parent")]
        public long? PARENT_ID { get; set; }
        [MaxLength(1500)]
        public string NOTE { get; set; }
        public Report Parent { get; set; }
        public List<Report> Childs { get; set; }
    }
    */
}
