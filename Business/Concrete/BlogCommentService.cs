using AutoMapper;
using Business.Abstract;
using Business.ServiceBase;
using DataAccess.Abstract;
using Model.Entities;

namespace Business.Concrete;

public class BlogCommentService : ServiceBase<BlogComment, IBlogCommentRepository>, IBlogCommentService
{
    public BlogCommentService(IBlogCommentRepository blogCommentRepository, IMapper mapper) : base(blogCommentRepository, mapper)
    {
    }
}
