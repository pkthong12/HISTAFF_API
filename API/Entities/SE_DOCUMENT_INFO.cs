using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SE_DOCUMENT_INFO")]

public class SE_DOCUMENT_INFO : BASE_ENTITY
{
    public long? EMP_ID  {get; set; }
    public long? DOCUMENT_ID  {get; set; }
    public DateTime? SUB_DATE  {get; set; }
    public bool? IS_SUBMIT {get; set; }
    public string? NOTE  {get; set; }
    public string? ATTACHED_FILE {get; set; }
    public string? CREATED_LOG { get; set; }
    public string? UPDATED_LOG { get; set; }
}
