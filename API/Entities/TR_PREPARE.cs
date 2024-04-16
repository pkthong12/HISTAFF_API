using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("TR_PREPARE")]
public class TR_PREPARE : BASE_ENTITY
{
    public long? TR_PROGRAM_ID { get; set; }
    public long? TR_LIST_PREPARE_ID { get; set; }
    public long? EMPLOYEE_ID { get; set; }
}
