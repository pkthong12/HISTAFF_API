using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_COMPETENCY_GROUP")]

public class HU_COMPETENCY_GROUP : BASE_ENTITY
{
    public string? CODE { get; set; }
    public string? NAME { get; set; }
    public bool? IS_ACTIVE { get; set; }
}
