using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_ENTITLEMENT_EDIT")]
public partial class AT_ENTITLEMENT_EDIT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public int YEAR { get; set; }

    public int MONTH { get; set; }

    public decimal NUMBER_CHANGE { get; set; }

    public string? NOTE { get; set; }

    public string? CODE { get; set; }

    public string? CODE_REF { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
