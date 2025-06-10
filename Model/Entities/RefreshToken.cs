using Core.Model;

namespace Model.Entities;

public class RefreshToken : IEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string IpAddress { get; set; } = null!;
    public string Token { get; set; } = null!;
    public DateTime ExpirationUtc { get; set; }
    public DateTime CreateDateUtc { get; set; }
    public int TTL { get; set; }

    public virtual User? User { get; set; }
}