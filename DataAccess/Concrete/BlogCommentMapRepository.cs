using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Concrete;

public class BlogCommentMapRepository : RepositoryBase<BlogCommentMap, AppDbContext>, IBlogCommentMapRepository
{
    public BlogCommentMapRepository(AppDbContext context) : base(context)
    {
    }
}