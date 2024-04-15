using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("SE_LDAP")]
    public class SE_LDAP : BASE_ENTITY
    {
        public string LDAP_NAME { get; set; } = null!;

        public string DOMAIN_NAME { get; set; } = null!;

        public string BASE_DN { get; set; } = null!;

        public long? PORT { get; set; }

        public string? CREATED_LOG { get; set; }

        public string? UPDATED_LOG { get; set; }

    }

}
