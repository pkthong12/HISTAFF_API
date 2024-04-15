namespace PayrollDAL.Models
{
    /*
    [Table("PA_SALARY_PAYCHECK")] // Thiết lập phiếu lương
    public class Paycheck : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [ForeignKey("SalaryType")]
        public int SALARY_TYPE_ID { get; set; }
        [ForeignKey("SalaryElement")]
        public long ELEMENT_ID { get; set; }
        [MaxLength(450)]
        public string NAME { get; set; } // tên hiển thị trên phiếu lương
        public int? ORDERS { get; set; }
        public bool IS_VISIBLE { get; set; } // hiển thị trong bảng lương
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public SalaryElement SalaryElement { get; set; }
        public SalaryType SalaryType { get; set; }
    }
    */
}
