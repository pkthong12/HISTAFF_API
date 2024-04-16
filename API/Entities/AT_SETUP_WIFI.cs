using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_SETUP_WIFI")]
public class AT_SETUP_WIFI : BASE_ENTITY
{
    public string? NAME_VN { get; set; }
    public string? NAME_WIFI { get; set; }
    public string? IP { get; set; }
    public bool? IS_ACTIVE { get; set; }
    public string? CREATED_LOG { get; set; }
    public string? UPDATED_LOG { get; set; }
    public string? NOTE { get; set; }
    public long? ORG_ID { get; set; }
}
