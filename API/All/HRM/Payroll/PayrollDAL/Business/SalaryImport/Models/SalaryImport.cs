namespace PayrollDAL.Models
{
    /*
    [Table("PA_SAL_IMPORT")] // Import lương khác
    public class SalaryImport : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [ForeignKey("Employee")]
        public long EMPLOYEE_ID { get; set; }       
        [ForeignKey("Element")]
        public long ELEMENT_ID { get; set; } // Phần tử lương
        public int PERIOD_ID { get; set; } // Kỳ ứng lương
        public decimal MONEY { get; set; } // Số tiền ứng
        [MaxLength(450)]
        public string MONEY1 { get; set; } // Số tiền ứng
        public int TYPE_SAL { get; set; } // Bảng lương
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public SalaryElement Element { get; set; }
        public Employee Employee { get; set; }
    }
    */
    /*
    [Table("PA_SAL_IMPORT_TMP")] // Import lương khác
    public class SalaryImportTmp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        [MaxLength(50)]
        public string REF_CODE { get; set; } // Số tiền ứng
        public long EMPLOYEE_ID { get; set; }
        public string EMPLOYEE_CODE { get; set; }
        public long ELEMENT_ID { get; set; } // Phần tử lương
        public int PERIOD_ID { get; set; } // Kỳ ứng lương
        [MaxLength(250)]
        public string VALUES { get; set; } // Số tiền ứng
        public int TYPE_SAL { get; set; } // Bảng lương
    }
    */
}
