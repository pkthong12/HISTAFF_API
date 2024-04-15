using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("AD_ROOM")]
public partial class AD_ROOM
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public string ADDRESS { get; set; } = null!;

    public string? NOTE { get; set; }

    public int? ORDERS { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    //public virtual ICollection<AD_BOOKING> AD_BOOKINGs { get; set; } = new List<AD_BOOKING>();
}
