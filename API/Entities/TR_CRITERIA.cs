using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("TR_CRITERIA")]

public class TR_CRITERIA : BASE_ENTITY 
{
    public string? CODE { get; set; }
    public string? NAME { get; set; }
    public int? MAX_SCORE { get; set; }
    public string? NOTE { get; set; }
    public bool? IS_ACTIVE { get; set; }
}
