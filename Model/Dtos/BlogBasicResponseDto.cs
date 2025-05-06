using Core.Model;

namespace Model.Dtos;

public class BlogBasicResponseDto : IDto
{
    public Guid BlogId { get; set; }
    public string Title { get; set; } = null!;
    public string BannerImage { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
}
