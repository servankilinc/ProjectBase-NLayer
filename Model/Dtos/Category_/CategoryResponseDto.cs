using Core.Model;

namespace Model.Dtos.Category_;

public class CategoryResponseDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}
