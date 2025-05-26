using Core.Utils.CrossCuttingConcerns;
using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Concrete;

[DataAccessException]
public class BlogLikeRepository : RepositoryBase<BlogLike, AppDbContext>, IBlogLikeRepository
{
    public BlogLikeRepository(AppDbContext context) : base(context)
    {
    }
}
