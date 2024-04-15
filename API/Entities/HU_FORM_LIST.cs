using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_FORM_LIST")]
public partial class HU_FORM_LIST
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? NAME { get; set; }

    public int? ID_MAP { get; set; }

    public long? PARENT_ID { get; set; }

    public long? TYPE_ID { get; set; }

    public long? ID_ORIGIN { get; set; }

    public string? TEXT { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public int? TENANT_ID { get; set; }
}
