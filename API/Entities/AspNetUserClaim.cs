namespace API.Entities;

public partial class AspNetUserClaim
{
    public long Id { get; set; }

    public string UserId { get; set; } = null!;

    public string? ClaimType { get; set; }

    public string? ClaimValue { get; set; }

    public virtual SYS_USER User { get; set; } = null!;
}
