namespace API.DTO
{
    public class SysRefreshTokenDTO
    {
        public long? Id { get; set; }
        public string? User { get; set; }
        public string? Token { get; set; }
        public DateTime? Expires { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Revoked { get; set; }
        public string? CreatedByIp { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByToken { get; set; }
        public string? ReasonRevoked { get; set; }
    }
}
