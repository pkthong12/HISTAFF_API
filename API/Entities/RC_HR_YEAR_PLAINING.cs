using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("RC_HR_YEAR_PLANING")]
    public class RC_HR_YEAR_PLANING : BASE_ENTITY
    {
        public int? YEAR { get; set; }
        public DateTime? EFFECT_DATE { get; set; }
        public DateTime? EXPIRE_DATE { get; set; }
        public string? VERSION { get; set; }
        public string? NOTE { get; set; }
        public string? CREATED_LOG { get; set; }
        public string? UPDATED_LOG { get; set; }
        public long? ORG_ID { get; set; }
    }
}
