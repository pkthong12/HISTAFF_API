using System.ComponentModel.DataAnnotations.Schema;
using Common.Interfaces;

namespace API.Entities;
[Table("ApiAuditLog")]
public partial class ApiAuditLog: IAuditableEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string? ControllerName { get; set; }

    public decimal? Status { get; set; }

    public string? RequestData { get; set; }

    public string? TraceId { get; set; }

    public string? ResponseBody { get; set; }

    public string? Method { get; set; }
    public Double? TimeTotal { get; set; }
    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
