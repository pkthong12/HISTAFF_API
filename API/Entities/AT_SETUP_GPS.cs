using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_SETUP_GPS")]
public class AT_SETUP_GPS : BASE_ENTITY
{
    public string? NAME{get;set;}
    public string? ADDRESS{get;set;}
    public string? LAT_VD{get;set;}
    public string? LONG_KD{get;set;}
    public long? RADIUS{get;set;}
    public bool? IS_ACTIVE{get;set;}
    public string? CREATED_LOG{get;set;}
    public string? UPDATED_LOG{get;set;}
    public long? ORG_ID { get; set; }
}
