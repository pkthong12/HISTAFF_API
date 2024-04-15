using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_CONTRACT_TYPE")]
public partial class SYS_CONTRACT_TYPE : BASE_ENTITY
{

    public string? CODE { get; set; } = null!;

    public string? NAME { get; set; } = null!;

    public int? PERIOD { get; set; }

    public int? DAY_NOTICE { get; set; }

    public string? NOTE { get; set; }

    public bool? IS_LEAVE { get; set; }

    public bool? IS_ACTIVE { get; set; }

}
