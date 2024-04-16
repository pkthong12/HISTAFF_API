namespace API.Entities;
using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

[Table("INS_LIST_PROGRAM")]
public class INS_LIST_PROGRAM : BASE_ENTITY
{
    public string? MODIFIED_BY { get; set; }
    public DateTime? MODIFIED_DATE { get; set; }
    public string? CREATED_LOG { get; set; }
    public string? MODIFIED_LOG { get; set; }
    public bool? IS_DELETED { get; set; }
    public string? CODE { get; set; }
    public string? NAME { get; set; }
    public string? NOTE { get; set; }
    public bool? IS_ACTIVE { get; set; }
}

