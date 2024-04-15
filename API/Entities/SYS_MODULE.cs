using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_MODULE")]
public partial class SYS_MODULE : BASE_ENTITY
{

    public long? APPLICATION_ID { get; set; }

    public string NAME { get; set; } = null!;

    public string CODE { get; set; } = null!;

    public string? NOTE { get; set; }

    public int? ORDERS { get; set; }

    public decimal? PRICE { get; set; }

    public bool? IS_ACTIVE { get; set; }

    // public virtual SYS_CONFIG? APPLICATION { get; set; }

    //public virtual ICollection<SYS_FUNCTION> SYS_FUNCTIONs { get; set; } = new List<SYS_FUNCTION>();
}
