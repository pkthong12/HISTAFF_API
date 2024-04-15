using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_LISTFUND")]

public partial class PA_LISTFUND : BASE_ENTITY
{
    public string? LISTFUND_CODE { get; set; }
    public string? LISTFUND_NAME { get; set; }
    public long? COMPANY_ID { get; set; }
    public string? NOTE { get; set; }
    public string? CREATED_LOG { get; set; }
    public string? UPDATED_LOG { get; set; }
    public bool? IS_ACTIVE { get;set; } 
}

