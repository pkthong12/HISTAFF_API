using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_ASSET_MNG")]
public class HU_ASSET_MNG : BASE_ENTITY
{
    public long? EMPLOYEE_ID { get; set; }
    public long? ORG_ID { get; set; }
    public long? ASSET_ID { get; set; }
    public long? SERIAL_NUM { get; set; }
    public decimal? VALUE_ASSET { get; set; }
    public DateTime? DATE_ISSUE { get; set; }
    public DateTime? REVOCATION_DATE { get; set; }
    public long? STATUS_ASSET_ID { get; set; }
    public string? NOTE { get; set; }
    public string? CREATED_LOG { get; set; }
    public string? UPDATED_LOG { get; set; }

}
