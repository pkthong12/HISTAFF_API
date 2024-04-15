using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_ELEMENT_GROUP")]
public partial class PA_ELEMENT_GROUP
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public int ORDERS { get; set; }

    public bool IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    //public virtual ICollection<PA_ELEMENT> PA_ELEMENTs { get; set; } = new List<PA_ELEMENT>();
}
