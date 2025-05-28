using Core.Model;
using Model.Dtos.User_;

namespace Model.Dtos.Blog_;

public class BlogLikesResponseDto : IDto
{
    public Guid BlogId { get; set; }
    public int LikeCount { get; set; }
    public List<UserBasicResponseDto>? UserList { get; set; }
}
