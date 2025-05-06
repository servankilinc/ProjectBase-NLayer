using Core.Model;
using Microsoft.AspNetCore.Identity;

namespace Model.Entities;

public class User: IdentityUser<Guid>, IEntity, ISoftDeletableEntity, IAuditableEntity
{
    // Id Email gibi alanları Identity kütüphanesi sağlıyor yinede girilebilir
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Addres { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }


    // Auditable Properties
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? CreateDateUtc { get; set; }
    public DateTime? UpdateDateUtc { get; set; }

    // Soft Deletable Properties
    public string? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDateUtc { get; set; }

    // Relationships
    public virtual ICollection<Blog>? Blogs { get; set; }
    public virtual ICollection<BlogLikeMap>? BlogLikeMaps { get; set; }
    public virtual ICollection<BlogCommentMap>? BlogCommentMaps { get; set; }
}
