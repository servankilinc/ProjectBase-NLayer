using Business.ServiceBase;
using Model.Entities;

namespace Business.Abstract;

public interface IBlogCommentService : IServiceBase<BlogComment>, IServiceBaseAsync<BlogComment>
{
}
