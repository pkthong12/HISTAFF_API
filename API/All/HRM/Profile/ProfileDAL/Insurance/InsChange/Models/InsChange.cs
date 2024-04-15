namespace ProfileDAL.Models
{
    /*
    [Table("INS_CHANGE")]// Biến động bảo hiểm
    public class InsChange : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public long TENANT_ID { get; set; }
        public int EMPLOYEE_ID { get; set; }
        public int CHANGE_TYPE_ID { get; set; }// Loại biến động
        public DateTime CHANGE_MONTH { get; set; }// ngày CẤP
        public double SALARY_OLD { get; set; }// Hệ số kỳ trước
        public double SALARY_NEW { get; set; }// Hệ số kỳ Này
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        [MaxLength(1500)]
        public string NOTE { get; set; }
        public bool? IS_BHXH { get; set; }// BHXH
        public bool? IS_BHYT { get; set; }// BHYT
        public bool? IS_BHTN { get; set; }// BHTN
        public bool? IS_BNN { get; set; }// BNN
    }

    [Table("TMP_INS_CHANGE")]// Import
    public class InsChangeTmp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        [MaxLength(150)]
        public string REF_CODE { get; set; }
        [MaxLength(150)]
        public string CODE { get; set; } // Mã nhân viên
        public int? EMPLOYEE_ID { get; set; }
        [MaxLength(250)]
        public string TYPE_NAME { get; set; } // Mã nhân viên
        public int? CHANGE_TYPE_ID { get; set; }// Loại biến động
        public DateTime CHANGE_MONTH { get; set; }// ngày CẤP
        public decimal SALARY_OLD { get; set; }// Hệ số kỳ trước
        public decimal SALARY_NEW { get; set; }// Hệ số kỳ Này
        public int? IS_BHXH { get; set; }// BHXH
        public int? IS_BHYT { get; set; }// BHYT
        public int? IS_BHTN { get; set; }// BHTN
        public int? IS_BNN { get; set; }// BNN
    }
    */
}
