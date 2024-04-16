using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_COMPETENCY_ASPECT")]

public class HU_COMPETENCY_ASPECT : BASE_ENTITY 
{
    public long? COMPETENCY_ID { get; set; }
    public string? CODE { get; set; }
    public string? NAME { get; set; }
    public string? NOTE { get; set; }
    public bool? IS_ACTIVE { get; set; }
}
