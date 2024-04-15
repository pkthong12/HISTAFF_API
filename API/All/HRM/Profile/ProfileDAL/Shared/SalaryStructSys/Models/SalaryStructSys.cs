namespace ProfileDAL.Models
{
    /*
    [Table("SYS_SALARY_STRUCTURE")] // Thông tin kết cấu bảng lương mẫu
    public class SalaryStructSys : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public long AREA_ID { get; set; }
        [ForeignKey("SalaryTypeSys")]
        public int SALARY_TYPE_ID { get; set; }
        [ForeignKey("SalaryElementSys")]
        public long ELEMENT_ID { get; set; }
        public bool IS_VISIBLE { get; set; } // hiển thị trong bảng lương
        public bool IS_CALCULATE { get; set; } // Thiết lập công thức
        public bool IS_IMPORT { get; set; } // Dữ liệu import
        public int? ORDERS { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public SalaryElementSys SalaryElementSys { get; set; }
        public SalaryTypeSys SalaryTypeSys { get; set; }
    }
    */
}
