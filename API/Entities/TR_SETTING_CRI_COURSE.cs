using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("TR_SETTING_CRI_COURSE")]

public class TR_SETTING_CRI_COURSE : BASE_ENTITY 
{
    public long? TR_COURSE_ID { get; set; }
    public long? TR_CRITERIA_GROUP_ID { get; set; }
    public DateTime? EFFECT_FROM { get; set; }
    public DateTime? EFFECT_TO { get; set; }
    public string? REMARK { get; set; }
    public double? SCALE_POINT { get; set; }

}
