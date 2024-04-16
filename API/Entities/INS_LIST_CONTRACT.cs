using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("INS_LIST_CONTRACT")]
public class INS_LIST_CONTRACT : BASE_ENTITY
{
    public string? CONTRACT_INS_NO { get; set; }
    public int? YEAR { get; set; }
    public long? ORG_INSURANCE { get; set; }
    public DateTime? START_DATE { get; set; }
    public DateTime? EXPIRE_DATE { get; set; }
    public decimal? VAL_CO { get; set; }
    public DateTime? BUY_DATE { get; set; }
    public string? NOTE { get; set; }
    public int? PROGRAM_ID { get; set; }
    public bool? IS_DELETED { get; set; }
    public string? MODIFIED_BY { get; set; }
    public DateTime? MODIFIED_DATE { get; set; }
    public string? CREATED_LOG { get; set; }
    public string? MODIFIED_LOG { get; set; }
    public decimal? SAL_INSU { get; set; }
    public string? PROGRAM {get;set;}
    public bool? IS_ACTIVE { get; set; }
}
