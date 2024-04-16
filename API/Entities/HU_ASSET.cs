using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_ASSET")]

public class HU_ASSET : BASE_ENTITY
{
    public string? CODE { get; set; }
    public string? NAME { get; set; }
    public long? GROUP_ASSET_ID { get; set; }
    public string? NOTE { get; set; }
    public bool? IS_ACTIVE { get; set; }
    public string? CREATED_LOG { get; set; }
    public string? UPDATED_LOG { get; set; }

}
