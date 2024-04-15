using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_PHASE_ADVANCE_SYMBOL")]
public class PA_PHASE_ADVANCE_SYMBOL : BASE_ENTITY
{
    public long? PHASE_ADVANCE_ID { get; set; }
    public long? SYMBOL_ID { get; set; }
    public string? CREATED_LOG { get; set; }
    public string? UPDATED_LOG { get; set; }

}
