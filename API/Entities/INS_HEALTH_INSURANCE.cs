using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("INS_HEALTH_INSURANCE")]

public class INS_HEALTH_INSURANCE : BASE_ENTITY
{
    public long? EMPLOYEE_ID { get; set; }
    public long? ORG_ID { get; set; }
    public int? YEAR { get; set; }
    public long? INS_CONTRACT_ID { get; set; }
    public bool? CHECK_BHNT { get; set; }
    public long? FAMILY_ID { get; set; } 
    public long? DT_CHITRA { get; set; }
    public DateTime? JOIN_DATE { get; set; }
    public DateTime? EFFECT_DATE { get; set; }
    public long? MONEY_INS { get; set; }
    public DateTime? REDUCE_DATE { get; set; }
    public decimal? REFUND { get; set; }
    public DateTime? DATE_RECEIVE_MONEY { get; set; }
    public string? EMP_RECEIVE_MONEY { get; set; }
    public string? NOTE { get; set; }
    public long? INS_CONTRACT_DE_ID { get; set; }
}
