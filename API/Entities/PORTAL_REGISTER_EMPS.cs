using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("PORTAL_REGISTER_EMPS")]
    public class PORTAL_REGISTER_EMPS:BASE_ENTITY
    {
        public long REGISTER_OFF_ID { get; set; }
        public long EMPLOYEE_ID{ get; set; }
    }
}
