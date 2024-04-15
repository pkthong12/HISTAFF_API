using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("INS_REGIMES")]
    public class INS_REGIMES:BASE_ENTITY
    {
        public string? CODE { get; set; }
        public string? NAME { get; set; }
        public long? INS_GROUP_ID { get; set; }
        public long? CAL_DATE_TYPE { get; set; }
        public int? TOTAL_DAY { get; set; }
        public int? BENEFITS_LEVELS { get; set;}
        public string? NOTE { get; set;}
        public bool? IS_ACTIVE { get; set; }
    }
}
