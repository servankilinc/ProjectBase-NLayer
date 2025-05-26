using Core.Utils.CrossCuttingConcerns;
using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Concrete;

[DataAccessException]
public class BlogRepository : RepositoryBase<Blog, AppDbContext>, IBlogRepository
{
    public BlogRepository(AppDbContext context) : base(context)
    {
    }
}
