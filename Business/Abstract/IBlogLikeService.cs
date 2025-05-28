using Business.ServiceBase;
using Model.Entities;

namespace Business.Abstract;

public interface IBlogLikeService : IServiceBase<BlogLike>, IServiceBaseAsync<BlogLike>
{
}
