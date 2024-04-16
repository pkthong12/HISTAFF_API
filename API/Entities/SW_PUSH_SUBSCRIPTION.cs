using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("SW_PUSH_SUBSCRIPTION")]
    public class SW_PUSH_SUBSCRIPTION : BASE_ENTITY
    {
        public required string USER_ID { get; set; }
        public required string ENDPOINT { get; set; }
        public DateTime? EXPIRATION_TIME { get; set; }
        public required string SUBSCRIPTION { get; set; }

    }
}