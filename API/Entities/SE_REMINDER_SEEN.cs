using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{

    [Table("SE_REMINDER_SEEN")]
    public class SE_REMINDER_SEEN : BASE_ENTITY
    {
        public long? REF_ID { get; set; }
        public string? NAME { get; set; }
        public string? CODE { get; set; }
        public string? AVATAR { get; set; }
    }
}
