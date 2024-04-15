using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_PROVINCE")]
public partial class HU_PROVINCE : BASE_ENTITY
{
    public string? CODE { get; set; } = null!;

    public string? NAME { get; set; } = null!;
    public string? NOTE { get; set; } = null!;

    public bool IS_ACTIVE { get; set; }

	public long? NATION_ID { get; set; }

}
