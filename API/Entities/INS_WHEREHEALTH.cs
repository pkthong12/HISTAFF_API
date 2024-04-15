using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("INS_WHEREHEALTH")]
    public class INS_WHEREHEALTH:BASE_ENTITY
    {
        public string? CODE { get; set; }
        public string? NAME_VN { get; set; }
        public string? ADDRESS { get; set; }
        public string? ACTFLG { get; set; }
        public long PROVINCE_ID { get; set; }
        public long DISTRICT_ID { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public string? NOTE { get; set; }
    }
}
