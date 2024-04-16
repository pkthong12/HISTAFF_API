using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("INS_CHANGE")]
public partial class INS_CHANGE : BASE_ENTITY
{
    public double? SALARY_OLD { get; set; }
    public double? SALARY_NEW { get; set; }

    //public decimal? SALARY_OLD { get; set; }
    //public decimal? SALARY_NEW { get; set; }

    public long? EMPLOYEE_ID { get; set; }
    public long? CHANGE_TYPE_ID { get; set; }
    public DateTime? CHANGE_MONTH { get; set; }
    public string? NOTE { get; set; }
    public bool? IS_BHXH { get; set; }
    public bool? IS_BHYT { get; set; }
    public bool? IS_BHTN { get; set; }
    public bool? IS_BNN { get; set; }
    public decimal? SALARY_BHXH_BHYT_OLD { get; set; }
    public decimal? SALARY_BHXH_BHYT_NEW { get; set; }
    public long? SALARY_BHXH_OLD { get; set; }//mức đóng bhxh cũ
    public long? SALARY_BHYT_OLD { get; set; }//mức đóng bhyt cũ
    public long? SALARY_BHBNN_OLD { get; set; }//mức đóng bhbnn cũ
    public decimal? SALARY_BHTN_OLD { get; set; } //mức đóng bhtn cũ

    public long? SALARY_BHXH_NEW { get; set; }//mức đóng bhxh mới
    public long? SALARY_BHYT_NEW { get; set; }//mức đóng bhyt mới
    public long? SALARY_BHBNN_NEW { get; set; }//mức đóng bhbnn mới
    public decimal? SALARY_BHTN_NEW { get; set; }//mức đóng bhtn mới

    public DateTime? EFFECTIVE_DATE { get; set; }
    public DateTime? EXPIRE_DATE { get; set; }
    public DateTime? DECLARATION_PERIOD { get; set; }
    public DateTime? BHYT_REIMBURSEMENT_DATE { get; set; }

    public DateTime? ARREARS_FROM_MONTH { get; set; }
    public DateTime? ARREARS_TO_MONTH { get; set; }
    public decimal? AR_BHXH_SALARY_DIFFERENCE { get; set; }
    public decimal? AR_BHYT_SALARY_DIFFERENCE { get; set; }
    public decimal? AR_BHTN_SALARY_DIFFERENCE { get; set; }
    public decimal? AR_BHTNLD_BNN_SALARY_DIFFERENCE { get; set; }

    public DateTime? WITHDRAWAL_FROM_MONTH { get; set; }
    public DateTime? WITHDRAWAL_TO_MONTH { get; set; }
    public decimal? WD_BHXH_SALARY_DIFFERENCE { get; set; }
    public decimal? WD_BHYT_SALARY_DIFFERENCE { get; set; }
    public decimal? WD_BHTN_SALARY_DIFFERENCE { get; set; }
    public decimal? WD_BHTNLD_BNN_SALARY_DIFFERENCE { get; set; }
    public long? UNIT_INSURANCE_TYPE_ID { get; set; }

}
