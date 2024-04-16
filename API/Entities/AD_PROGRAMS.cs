
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Entities;

[Table("AD_PROGRAMS")]
public partial class AD_PROGRAMS
{
    [Key]
    public int PROGRAM_ID { get; set; }
    public string? CODE { get; set; }
    public string? NAME { get; set; }
    public string? PROGRAM_TYPE { get; set; }
    public string? DESCRIPTION { get; set; }
    public string? STORE_EXECUTE_IN { get; set; }
    public string? STORE_EXECUTE_OUT { get; set; }
    public string? TEMPLATE_NAME { get; set; }
    public string? TEMPLATE_TYPE_IN  { get; set; }
    public string? TEMPLATE_TYPE_OUT { get; set; }
    public string? TEMPLATE_URL { get; set; }
    public string? NOTE { get; set; }
}
