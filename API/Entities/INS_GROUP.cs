using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("INS_GROUP")]
    public class INS_GROUP : BASE_ENTITY
    {
        public string CODE { get; set; } = null!;
        public string NAME { get; set; } = null!;

        public string? NOTE { get; set; }

        public bool IS_ACTIVE { get; set; }
    }
}
