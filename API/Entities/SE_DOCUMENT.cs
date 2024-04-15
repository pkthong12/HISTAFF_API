using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SE_DOCUMENT")]
public class SE_DOCUMENT : BASE_ENTITY
{
    public string? CODE { get; set; }
    public string? NAME { get; set; }
    public long? DOCUMENT_TYPE { get; set; }
    public bool? IS_OBLIGATORY { get; set; }
    public bool? IS_PERMISSVE_UPLOAD { get; set; }
    public bool? IS_ACTIVE { get; set; }
    public string? NOTE { get; set; }
    public string? CREATED_LOG { get; set; }
    public string? UPDATE_LOG { get; set; }

}
