using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("SE_REMINDER")]
    public class SE_REMINDER: BASE_ENTITY
    {
        public long SYS_OTHERLIST_ID { get; set; }
        public int VALUE { get; set; }
        public bool IS_ACTIVE { get; set; }
        public string? CODE { get; set; }
    }
}
