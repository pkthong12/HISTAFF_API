using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_ACTION")]
public class SYS_ACTION: BASE_ENTITY
{
    public string? CODE { get; set; }
    public bool? IS_ACTIVE { get; set; }
}