using Core.Model;
using Model.Dtos.Blog_;

namespace Model.Dtos.Category_;

public class CategoryBlogsResponseDto : IDto
{
    public CategoryResponseDto? Category { get; set; }
    public List<BlogBasicResponseDto>? BlogList { get; set; }
}