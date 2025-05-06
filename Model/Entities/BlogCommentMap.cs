using Core.Model;

namespace Model.Entities;

public class BlogCommentMap : IEntity
{
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }
    public string Comment { get; set; } = null!;

    public Blog? Blog { get; set; }
    public User? User { get; set; }
}
