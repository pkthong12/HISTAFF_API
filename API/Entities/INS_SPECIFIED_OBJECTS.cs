using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("INS_SPECIFIED_OBJECTS")]

public class INS_SPECIFIED_OBJECTS : BASE_ENTITY
{
    public DateTime? EFFECTIVE_DATE { get; set; }

    public int? CHANGE_DAY { get; set; }

    public decimal? SI_HI { get; set; }
    
    public decimal? UI { get; set; }

    public decimal? SI_COM { get; set; }

    public decimal? SI_EMP { get; set; }

    public decimal? HI_COM { get; set; }

    public decimal? HI_EMP { get; set; }

    public decimal? UI_COM { get; set; }

    public decimal? UI_EMP { get; set; }

    public decimal? AI_OAI_COM { get; set; }

    public decimal? AI_OAI_EMP { get; set; }

    public int? RETIRE_MALE { get; set; }

    public int? RETIRE_FEMALE { get; set; }
}
