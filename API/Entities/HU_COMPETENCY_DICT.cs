using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_COMPETENCY_DICT")]

public class HU_COMPETENCY_DICT : BASE_ENTITY
{
    public long? COMPETENCY_GROUP_ID { get; set; }
    public long? COMPETENCY_ID { get; set; }
    public long? LEVEL_NUMBER { get; set; }
    public string? NOTE { get; set; }
    public bool? IS_ACTIVE { get; set; }
    public long? COMPETENCY_ASPECT_ID { get; set; }
}
