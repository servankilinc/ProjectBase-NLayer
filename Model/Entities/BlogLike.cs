using Core.Model;

namespace Model.Entities;

public class BlogLike : IEntity, ILoggableEntity
{
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }

    #region Relations
    public Blog? Blog { get; set; }
    public User? User { get; set; }
    #endregion
}
