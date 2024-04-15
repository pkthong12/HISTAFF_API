using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace API.Entities;
[Table("SE_CONFIG")]

public class SE_CONFIG : BASE_ENTITY
{
    public string? CODE { get; set; }
    public string? NAME { get; set; }
    public string? VALUE { get; set; }
    public int? MODULE { get; set; }
    public string? NOTE { get; set; }
    public bool? IS_AUTH_SSL { get; set; }
    public bool? IS_AUTH_SENDING_MAIL { get; set; }
    public string? ACCOUNT { get; set; }
    public string? PASSWORD { get; set; }

}
