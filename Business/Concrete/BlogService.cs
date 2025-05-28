using AutoMapper;
using Business.Abstract;
using Business.ServiceBase;
using DataAccess.Abstract;
using Model.Entities;

namespace Business.Concrete;

public class BlogService : ServiceBase<Blog, IBlogRepository>, IBlogService
{
    public BlogService(IBlogRepository blogRepository, IMapper mapper) : base(blogRepository, mapper)
    {
    }
}
