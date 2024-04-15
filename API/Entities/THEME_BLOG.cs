using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("THEME_BLOG")]
public partial class THEME_BLOG
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? NAME { get; set; }

    public string? IMG_URL { get; set; }

    public string? COLOR { get; set; }

    public bool IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
