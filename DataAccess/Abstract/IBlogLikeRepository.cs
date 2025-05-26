using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Abstract;

public interface IBlogLikeRepository : IRepository<BlogLike>, IRepositoryAsync<BlogLike>
{
}
