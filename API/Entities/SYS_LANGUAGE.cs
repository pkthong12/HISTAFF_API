using API.Main;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("SYS_LANGUAGE")]
public partial class SYS_LANGUAGE: BASE_ENTITY
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    [MinLength(1), MaxLength(255)]
    public string KEY { get; set; } = null!;
    [MinLength(1), MaxLength(255)]
    public string VI { get; set; } = null!;
    [MinLength(1), MaxLength(255)]
    public string EN { get; set; } = null!;
    public string? NOTE { get; set; }
}
