using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_COM_WORKING")]
    public class HU_COM_WORKING: BASE_ENTITY
    {
        public long? COM_EMPLOYEE_ID { get; set; }
        public long? COMMUNIST_POSITION_ID { get; set; }
        public long? COMMUNIST_ORG_ID { get; set; }
        public long? TRANFER_TYPE_ID { get; set; }
        public DateTime? EFFECT_DATE { get; set; }
        public string? DECISION_NO { get; set; }
        public DateTime? EXPIRE_DATE { get; set; }
        public DateTime? DATE_OF_PAYMENT { get; set; }
        public long? STATUS_ID { get; set; }
        public string? NOTE { get; set; }
        public string? CREATED_LOG { get; set; }
        public string? UPDATED_LOG { get; set; }
        public string? COMMUNIST_ORG_NAME { get; set; }
        public string? COMMUNIST_TITLE_NAME { get; set; }
        public long? COMMUNIST_ORG_ID_OLD { get; set; }
        public long? COMMUNIST_TITLE_ID_OLD { get; set; }
        public long? COMMUNIST_TITLE_ID_MAX { get; set; }
        public string? SIGN_NAME { get; set; }
        public string? SIGN_POSITION_NAME { get; set; }
        public string? NOTE_PD { get; set; }
        public DateTime? SIGN_DATE { get; set; }
        public long? HU_COM_EMPLOYEEID { get; set; }
        public string? COMMUNIST_TITLE_NAME_MAX { get; set; }
        public string? ATTACHED_FILE { get; set; }
    }
}
