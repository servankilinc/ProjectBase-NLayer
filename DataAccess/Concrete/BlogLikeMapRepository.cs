using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Concrete;

public class BlogLikeMapRepository : RepositoryBase<BlogLikeMap, AppDbContext>, IBlogLikeMapRepository
{
    public BlogLikeMapRepository(AppDbContext context) : base(context)
    {
    }
}
