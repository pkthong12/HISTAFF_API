namespace AttendanceDAL.Models
{
    /*
    [Table("AT_TIME_LATE_EARLY")]// di muon ve som
    public class TimeLateEarly : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        public DateTime? DATE_START { get; set; }// Ngày bat dau
        public DateTime? DATE_END { get; set; }// Ngày ket thuc
        public int? TIME_LATE { get; set; }// so phut di muon
        public int? TIME_EARLY { get; set; }// so phut ve tre
        [ForeignKey("employee")]
        public long? EMPLOYEE_ID { get; set; }
        public int STATUS_ID { get; set; }
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
        public Employee employee { get; set; }
    }
    */
}
