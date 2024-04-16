using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("TR_SETTING_CRI_DETAIL")]

public class TR_SETTING_CRI_DETAIL : BASE_ENTITY 
{
    public long? CRITERIA_ID { get; set; }
    public double? RATIO { get; set; }
    public double? POINT_MAX { get; set; }
    public long? COURSE_ID { get; set; }
}
