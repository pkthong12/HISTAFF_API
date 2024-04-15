using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("TMP_INS_CHANGE")]
public partial class TMP_INS_CHANGE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? REF_CODE { get; set; }

    public string? CODE { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public string? TYPE_NAME { get; set; }

    public long? CHANGE_TYPE_ID { get; set; }

    public DateTime CHANGE_MONTH { get; set; }

    public decimal SALARY_OLD { get; set; }

    public decimal SALARY_NEW { get; set; }

    public int? IS_BHXH { get; set; }

    public int? IS_BHYT { get; set; }

    public int? IS_BHTN { get; set; }

    public int? IS_BNN { get; set; }
}
