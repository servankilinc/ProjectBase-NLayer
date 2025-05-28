using AutoMapper;
using Business.Abstract;
using Business.ServiceBase;
using DataAccess.Abstract;
using Model.Entities;

namespace Business.Concrete;

public class BlogLikeService : ServiceBase<BlogLike, IBlogLikeRepository>, IBlogLikeService
{
    public BlogLikeService(IBlogLikeRepository blogLikeRepository, IMapper mapper) : base(blogLikeRepository, mapper)
    {
    }
}
