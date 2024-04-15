using API.Main;

namespace API.DTO
{
    public class SeLdapDTO:BaseDTO
    {
        public string? LdapName { get; set; }

        public string? DomainName { get; set; }

        public string? BaseDn { get; set; }

        public long? Port { get; set; }

        public string? CreatedLog { get; set; }

        public string? UpdatedLog { get; set; }
    }
}
