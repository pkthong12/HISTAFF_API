using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_CLASSIFICATION")]
public partial class HU_CLASSIFICATION
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? CODE { get; set; }

    public long? CLASSIFICATION_TYPE { get; set; }

    public long? CLASSIFICATION_LEVEL { get; set; }

    public long? POINT_FROM { get; set; }

    public long? POINT_TO { get; set; }

    public string? NOTE { get; set; }

    public bool IS_ACTIVE { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public string? CREATED_BY { get; set; }

    public DateTime? MODIFIED_DATE { get; set; }

    public string? MODIFIED_BY { get; set; }

}
