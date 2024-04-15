using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("SYS_MUTATION_LOG")]
public partial class SYS_MUTATION_LOG : BASE_ENTITY
{
    public string? SYS_FUNCTION_CODE { get; set; }
    public string? SYS_ACTION_CODE { get; set; }
    public string? BEFORE { get; set; }
    public string? BEFORE1 { get; set; }
    public string? BEFORE2 { get; set; }
    public string? BEFORE3 { get; set; }
    public string? AFTER { get; set; }
    public string? AFTER1 { get; set; }
    public string? AFTER2 { get; set; }
    public string? AFTER3 { get; set; }
    public string? USERNAME { get; set; }
}
