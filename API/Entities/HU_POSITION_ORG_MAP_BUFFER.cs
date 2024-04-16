using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_POSITION_ORG_MAP_BUFFER")]
    public class HU_POSITION_ORG_MAP_BUFFER: BASE_ENTITY
    {
        public string USER_ID { get; set; }
        public long? OLD_POSITION_ID { get; set; }
        public long? NEW_POSITION_ID { get; set; }
        public long? ORG_ID { get; set; }
    }
}
