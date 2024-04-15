using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PT_BLOG_INTERNAL")]
public partial class PT_BLOG_INTERNAL
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? TITLE { get; set; }

    public string? IMG_URL { get; set; }

    public string? DESCRIPTION { get; set; }

    public string? CONTENT { get; set; }

    public long? THEME_ID { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
