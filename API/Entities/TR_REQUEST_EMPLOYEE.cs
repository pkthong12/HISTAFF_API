using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("TR_REQUEST_EMPLOYEE")]

public class TR_REQUEST_EMPLOYEE 
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }
    public long? TR_REQUEST_ID { get; set; }
    public long? EMPLOYEE_ID { get; set; }
}
