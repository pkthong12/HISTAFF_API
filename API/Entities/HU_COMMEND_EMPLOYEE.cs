using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_COMMEND_EMPLOYEE")]
    public class HU_COMMEND_EMPLOYEE: BASE_ENTITY
    {
        public long EMPLOYEE_ID { get; set; }
        public long? PROFILE_ID { get; set; }
        public long? COMMEND_ID { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public long? STATUS_ID { get; set; }
    }
}
