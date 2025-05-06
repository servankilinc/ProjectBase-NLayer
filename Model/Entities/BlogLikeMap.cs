using Core.Model;

namespace Model.Entities;

public class BlogLikeMap : IEntity
{
    public Guid BlogId { get; set; } 
    public Guid UserId { get; set; } 

    public Blog? Blog { get; set; }
    public User? User { get; set; }
}
