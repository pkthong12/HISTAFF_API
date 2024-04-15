namespace ProfileDAL.Models
{
    /*
    [Table("HU_ALLOWANCE_EMP")]
    public class AllowanceEmp : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [ForeignKey("Employee")]
        public long EMPLOYEE_ID { get; set; }// Nhân viên
        [ForeignKey("Allowance")]
        public Int64? ALLOWANCE_ID { get; set; }// phụ cấp
        public decimal? MONNEY { get; set; }
        public DateTime? DATE_START { get; set; }
        public DateTime? DATE_END { get; set; }

        public bool IS_ACTIVE { get; set; }
        [MaxLength(1500)]
        public string NOTE { get; set; }
        
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public Allowance Allowance { get; set; }
        public Employee Employee { get; set; }
    }
    */
}
