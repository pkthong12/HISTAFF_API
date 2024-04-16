using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_TERMINAL")]
public partial class AT_TERMINAL : BASE_ENTITY
{
    public string? TERMINAL_CODE { get; set; } 

    public string? TERMINAL_NAME { get; set; } 

    public string? ADDRESS_PLACE { get; set; } 

    public string? TERMINAL_PORT { get; set; } 

    public string? TERMINAL_IP { get; set; } 

    public string? PASS { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? NOTE { get; set; } 

}
