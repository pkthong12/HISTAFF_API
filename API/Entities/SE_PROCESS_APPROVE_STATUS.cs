using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SE_PROCESS_APPROVE_STATUS")]

public class SE_PROCESS_APPROVE_STATUS : BASE_ENTITY
{

    public string? ID_REGGROUP { get; set; }

    public string? PROCESS_TYPE { get; set; }

    public int? APP_LEVEL { get; set; }

    public int? APP_STATUS { get; set; }

    public long? PROCESS_APPROVE_ID { get; set; }

    public string? APP_NOTE { get; set; }

    public DateTime? APP_DATE { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public long? PROCESS_ID { get; set; }

    public long? EMPLOYEE_APPROVED { get; set; }

}
