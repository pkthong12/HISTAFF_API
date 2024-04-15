using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("AT_ORG_PERIOD")]
    public class AT_ORG_PERIOD:BASE_ENTITY
    {
        public long? ORG_ID { get; set; }
        public long? PERIOD_ID { get; set; }
       
        public int? STATUSCOLEX { get; set; }
        public int? STATUSPAROX { get; set; }
        public int? STATUSPAROX_SUB { get; set; }
        public int? STATUSPAROX_BACKDATE { get; set; }
        public int? STATUSPAROX_TAX_MONTH { get; set; }
        public int? STATUSPAROX_TAX_YEAR { get; set; }
        public int? UPTO_PORTAL { get; set; }
        public int? STATUSEXP { get; set; }
        public int? STATUSPAYBACK { get; set; }
    }
}
