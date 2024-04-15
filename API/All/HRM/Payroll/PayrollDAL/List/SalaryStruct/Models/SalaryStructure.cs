namespace PayrollDAL.Models
{
    /*
    [Table("PA_SALARY_STRUCTURE")] // Thông tin kết cấu bảng lương
    public class SalaryStructure : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
       
        [ForeignKey("SalaryType")]
        public int SALARY_TYPE_ID { get; set; }
        [ForeignKey("SalaryElement")]
        public long ELEMENT_ID { get; set; }
        public bool? IS_VISIBLE { get; set; } // hiển thị trong bảng lương
        public bool? IS_CALCULATE { get; set; } // Thiết lập công thức
        public bool? IS_IMPORT { get; set; } // Dữ liệu import
        public bool? IS_SUM { get; set; } // Dữ liệu tính tổng
        public bool? IS_CHANGE { get; set; } // Công thức tính theo biến động tron tháng
        public int? ORDERS { get; set; }
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
