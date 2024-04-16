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
    public decimal? SALARY_BHTN_OLD { get; set; }
    public decimal? SALARY_BHTN_NEW { get; set; }
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
