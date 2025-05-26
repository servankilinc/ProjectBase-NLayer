using Core.Model;
using Model.Dtos.Blog_;

namespace Model.Dtos.User_;

public class UserBlogsResponseDto : IDto
{
    public UserBasicResponseDto? User { get; set; }
    public List<BlogBasicResponseDto>? BlogList { get; set; }
}
