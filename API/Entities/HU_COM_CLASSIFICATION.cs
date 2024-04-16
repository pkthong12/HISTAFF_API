using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_COM_CLASSIFICATION")]
public class HU_COM_CLASSIFICATION : BASE_ENTITY
{
    public long? COMMUNIST_ID { get; set; }
    public long? COMMUNIST_ORG_ID { get; set; }
    public long? COMMUNIST_TITLE_ID { get; set; }
    public long? EMPLOYEE_ID { get; set; }
    public long? DECISION_NUMBER { get; set; }
    public DateTime? DECISION_DATE { get; set; }
    public int? YEAR { get; set; }
    public long? CLASSIFICATION_ID { get; set; }
    public string? REWARD_NAME { get; set; }
    public string? NOTE { get; set; }
    public string? CREATED_LOG { get; set; }
    public string? UPDATED_LOG { get; set; }

}
