using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("HU_EMPLOYEE_PAPERS")]
public partial class HU_EMPLOYEE_PAPERS: BASE_ENTITY
{

    public long PAPER_ID { get; set; }

    public long? EMP_ID { get; set; }

    public DateTime DATE_INPUT { get; set; }

    public string? URL { get; set; }

    public string? NOTE { get; set; }

    public long STATUS_ID { get; set; }

}
