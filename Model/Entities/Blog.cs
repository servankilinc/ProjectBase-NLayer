using Core.Model;
namespace Model.Entities;

public class Blog: IEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string BannerImageSource { get; set; } = null!;
    public Guid AuthorId { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }

    public virtual User? User { get; set; }
    public virtual ICollection<BlogLikeMap>? BlogLikeMaps { get; set; }
    public virtual ICollection<BlogCommentMap>? BlogCommentMaps { get; set; }
}
