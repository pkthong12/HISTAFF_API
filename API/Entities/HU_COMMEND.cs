using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_COMMEND")]
public partial class HU_COMMEND:BASE_ENTITY
{
    public DateTime? EFFECT_DATE { get; set; }
    public string? NO { get; set; }
    public DateTime? SIGN_DATE { get; set; }
    public long? SIGN_PROFILE_ID { get; set; }
    public long? SIGN_ID { get; set; }
    public string? SIGNER_NAME { get; set; }
    public string? SIGNER_POSITION { get; set; }
    public long? ORG_ID { get; set; }
    public long? COMMEND_OBJ_ID { get; set; }
    public long? SOURCE_COST_ID { get; set; }
    public string? COMMEND_TYPE { get; set; }
    public string? REASON { get; set; }
    public long? STATUS_ID { get; set; }
    public double? MONEY { get; set; }
    public bool? IS_TAX { get; set; }
    public long? PERIOD_ID { get; set; }
    public int? YEAR { get; set; }

    //public virtual ICollection<HU_COMMEND_EMP> HU_COMMEND_EMPs { get; set; } = new List<HU_COMMEND_EMP>();

    /*public virtual HU_ORGANIZATION? ORG { get; set; }

    public virtual AT_SALARY_PERIOD? PERIOD { get; set; }

    public virtual HU_EMPLOYEE? SIGN { get; set; }*/
    public string? CONTENT { get; set; }
    public long? EMPLOYEE_ID { get; set; }
    public string? EMPLOYEE_CODE { get; set; }
    public string? EMPLOYEE_NAME { get; set; }
    public string? NUM_REWARD_PAY { get; set; }
    public string? ORG_NAME { get; set; }
    public string? POSITION_NAME { get; set; }
    public int? SALARY_INCREASE_TIME { get; set; }
    public long? ORG_LEVEL_ID { get; set; }
    public long? AWARD_TITLE_ID { get; set; }
    public string? NOTE { get; set; }
    public DateTime? SIGN_PAYMENT_DATE { get; set; }
    public string? PAYMENT_NO { get; set; }
    public long? STATUS_PAYMENT_ID { get; set; }
    public long? SIGN_PAYMENT_PROFILE_ID { get; set; }
    public long? SIGN_PAYMENT_ID { get; set; }
    public string? POSITION_PAYMENT_NAME { get; set; }
    public long? FUND_SOURCE_ID { get; set; }
    public long? REWARD_ID { get; set; }
    public long? REWARD_LEVEL_ID { get; set; }
    public int? MONTH_TAX { get; set; }
    public string? ATTACHMENT { get; set; }
    public string? SIGN_PAYMENT_NAME { get; set; }
    public string? PAYMENT_NOTE { get; set; }
    public string? PAYMENT_CONTENT { get; set; }
    public string? PAYMENT_ATTACHMENT { get; set; }
    public string? LIST_REWARD_LEVEL_ID { get; set; }
}
