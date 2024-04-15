using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_ENTITLEMENT")]
public partial class AT_ENTITLEMENT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }
    public long? ORG_ID { get; set; }
    public long? POSITION_ID { get; set; }

    public long? YEAR { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public double? WORKING_TIME_HAVE { get; set; }

    public double? PREV_HAVE { get; set; }

    public double? PREV_USED { get; set; }

    public double? CUR_HAVE { get; set; }

    public double? CUR_USED { get; set; }

    public double? CUR_HAVE1 { get; set; }

    public double? CUR_HAVE2 { get; set; }

    public double? CUR_HAVE3 { get; set; }

    public double? CUR_HAVE4 { get; set; }

    public double? CUR_HAVE5 { get; set; }

    public double? CUR_HAVE6 { get; set; }

    public double? CUR_HAVE7 { get; set; }

    public double? CUR_HAVE8 { get; set; }

    public double? CUR_HAVE9 { get; set; }

    public double? CUR_HAVE10 { get; set; }

    public double? CUR_HAVE11 { get; set; }

    public double? CUR_HAVE12 { get; set; }

    public double? CUR_USED1 { get; set; }

    public double? CUR_USED2 { get; set; }

    public double? CUR_USED3 { get; set; }

    public double? CUR_USED4 { get; set; }

    public double? CUR_USED5 { get; set; }

    public double? CUR_USED6 { get; set; }

    public double? CUR_USED7 { get; set; }

    public double? CUR_USED8 { get; set; }

    public double? CUR_USED9 { get; set; }

    public double? CUR_USED10 { get; set; }

    public double? CUR_USED11 { get; set; }

    public double? CUR_USED12 { get; set; }

    public int? MONTH { get; set; }

    public long? PERIOD_ID { get; set; }
    public double? PREVTOTAL_HAVE { get; set; }
    public double? PREV_USED1 { get; set; }
    public double? PREV_USED2 { get; set; }
    public double? PREV_USED3 { get; set; }
    public double? PREV_USED4 { get; set; }
    public double? PREV_USED5 { get; set; }
    public double? PREV_USED6 { get; set; }

    public double? PREV_USED7 { get; set; }
    public double? PREV_USED8 { get; set; }
    public double? PREV_USED9 { get; set; }
    public double? PREV_USED10 { get; set; }
    public double? PREV_USED11 { get; set; }
    public double? PREV_USED12 { get; set; }

    public DateTime? EXPIREDATE { get; set; }
    public DateTime? JOIN_DATE { get; set; }
    public DateTime? JOIN_DATE_STATE { get; set; }

    public double? SENIORITY { get; set; }
    public double? SENIORITYHAVE { get; set; }
    public double? SENIORITY_MONTH { get; set; }
    public double? QP_YEAR { get; set; }
    public double? QP_MONTH_SUM { get; set; }
    public double? TOTAL_HAVE { get; set; }

    public double? QP_YEARX_USED { get; set; }
    public double? QP_YEARX_HAVE { get; set; }

    public double? QP_STANDARD { get; set; }
}
