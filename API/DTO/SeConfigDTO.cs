using API.Main;

namespace API.DTO
{
    public class SeConfigDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
        public int? Module { get; set; }
        public string? Note { get; set; }
        public bool? IsAuthSsl { get; set; }
        public bool? IsAuthSendingMail { get; set; }
        public string? Account { get; set; }
        public string? Password { get; set; }
    }
}
