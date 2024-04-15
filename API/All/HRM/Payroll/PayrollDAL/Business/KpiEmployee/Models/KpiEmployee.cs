using API.Entities;
using Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollDAL.Models
{
    /*
    [Table("PA_KPI_SALARY_DETAIL")] // PHẦN TỬ LƯƠNG
    public class KpiEmployee : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        public int PERIOD_ID { get; set; }
        [ForeignKey("Employee")]
        public long EMPLOYEE_ID { get; set; }
        [ForeignKey("KpiTarget")]
        public long KPI_TARGET_ID { get; set; }
        public decimal? REAL_VALUE { get; set; }
        public decimal? START_VALUE { get; set; }
        public decimal? EQUAL_VALUE { get; set; }
        public decimal? KPI_SALARY { get; set; }
        public bool? IS_PAY_SALARY { get; set; }
        [MaxLength(550)]
        public string NOTE { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public KpiTarget KpiTarget { get; set; }
        public Employee Employee { get; set; }
    }
    */
    [Table("PA_KPI_SALARY_DETAIL_DAY")] // KPI theo ngày
    public class KpiEmployeeDay : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        public int PERIOD_ID { get; set; }

        [ForeignKey("KpiEmployee")]
        public long KPI_EMP_ID { get; set; }

        public decimal? DAY01 { get; set; }
        public decimal? DAY02 { get; set; }
        public decimal? DAY03 { get; set; }
        public decimal? DAY04 { get; set; }
        public decimal? DAY05 { get; set; }
        public decimal? DAY06 { get; set; }
        public decimal? DAY07 { get; set; }
        public decimal? DAY08 { get; set; }
        public decimal? DAY09 { get; set; }
        public decimal? DAY10 { get; set; }
        public decimal? DAY11 { get; set; }
        public decimal? DAY12 { get; set; }
        public decimal? DAY13 { get; set; }
        public decimal? DAY14 { get; set; }
        public decimal? DAY15 { get; set; }
        public decimal? DAY16 { get; set; }
        public decimal? DAY17 { get; set; }
        public decimal? DAY18 { get; set; }
        public decimal? DAY19 { get; set; }
        public decimal? DAY20 { get; set; }
        public decimal? DAY21 { get; set; }
        public decimal? DAY22 { get; set; }
        public decimal? DAY23 { get; set; }
        public decimal? DAY24 { get; set; }
        public decimal? DAY25 { get; set; }
        public decimal? DAY26 { get; set; }
        public decimal? DAY27 { get; set; }
        public decimal? DAY28 { get; set; }
        public decimal? DAY29 { get; set; }
        public decimal? DAY30 { get; set; }
        public decimal? DAY31 { get; set; }

        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public PA_KPI_SALARY_DETAIL KpiEmployee { get; set; }
    }

    /*
    [Table("PA_KPI_SALARY_DETAIL_TMP")] // BANG TAM
    public class KpiEmployeeTmp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        public int PERIOD_ID { get; set; }
        public long EMPLOYEE_ID { get; set; }
        public long KPI_TARGET_ID { get; set; }
        public decimal? REAL_VALUE { get; set; }
        public decimal? START_VALUE { get; set; }
        public decimal? EQUAL_VALUE { get; set; }
    }
    */
}
