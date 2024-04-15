namespace AttendanceDAL.Models
{
    /*
    [Table("AT_TIMESHEET_MONTHLY")]
    public class TimeSheetMonthly : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [ForeignKey("employee")]
        public long EMPLOYEE_ID { get; set; }
        public long PERIOD_ID { get; set; }
        public int FULLDAY { get; set; }
        public int ORG_ID { get; set; }
        public decimal WORKING_CD { get; set; }
        public decimal WORKING_P { get; set; }
        public decimal WORKING_KL { get; set; }
        public decimal WORKING_NB { get; set; }
        public decimal WORKING_L { get; set; } // Công nghỉ lễ 
        public decimal WORKING_X { get; set; }// cong ngay thuong, di lam thuc te
        public decimal WORKING_CT { get; set; }// cong di cong tac
        public decimal WORKING_VR { get; set; }
        public decimal WORKING_TS { get; set; }
        public decimal WORKING_H { get; set; }
        public decimal WORKING_XL { get; set; }
        public decimal WORKING_OFF { get; set; }
        public decimal WORKING__ { get; set; }
        public decimal WORKING_LPAY { get; set; } // Công nghỉ có hưởng lương
        public decimal WORKING_PAY { get; set; } // Tổng Công tính lương
        public decimal WORKING_N { get; set; } // Tổng Công nghỉ
        public decimal OT_NL { get; set; } 
        public decimal OT_DNL { get; set; } 
        public decimal OT_NN { get; set; } 
        public decimal OT_DNN { get; set; } 
        public decimal OT_NT { get; set; } 
        public decimal OT_DNT { get; set; }
        public decimal? WORKING_O { get; set; } // CÔNG ONSITE

        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

    }
    [Table("AT_TIMESHEET_MONTHLY_DTL")]
    public class TimeSheetMonthlyDtl : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [ForeignKey("Employee")]
        public long EMPLOYEE_ID { get; set; }
        [ForeignKey("SalaryPeriod")]
        public long PERIOD_ID { get; set; }
        public int FULLDAY { get; set; }
        public DateTime? WORKING_DAY { get; set; }
        public int ORG_ID { get; set; }
        public decimal WORKING_CD { get; set; }
        public decimal WORKING_P { get; set; }
        public decimal WORKING_KL { get; set; }
        public decimal WORKING_NB { get; set; }
        public decimal WORKING_L { get; set; } // Công nghỉ lễ 
        public decimal WORKING_X { get; set; }// cong ngay thuong, di lam thuc te
        public decimal WORKING_CT { get; set; }// cong di cong tac
        public decimal WORKING_VR { get; set; }
        public decimal WORKING_TS { get; set; }
        public decimal WORKING_H { get; set; }
        public decimal WORKING_XL { get; set; }
        public decimal WORKING_OFF { get; set; }
        public decimal WORKING__ { get; set; }
        public decimal WORKING_LPAY { get; set; } // Công nghỉ có hưởng lương
        public decimal WORKING_PAY { get; set; } // Tổng Công tính lương
        public decimal WORKING_N { get; set; } // Tổng Công nghỉ
        public decimal? WORKING_O { get; set; } // CÔNG ONSITE
        public decimal OT_NL { get; set; }
        public decimal OT_DNL { get; set; }
        public decimal OT_NN { get; set; }
        public decimal OT_DNN { get; set; }
        public decimal OT_NT { get; set; }
        public decimal OT_DNT { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public Employee Employee { get; set; }
        public SalaryPeriod SalaryPeriod { get; set; }

    }
    */
}
