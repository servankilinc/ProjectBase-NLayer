using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Abstract;

public interface IBlogLikeMapRepository : IRepository<BlogLikeMap>, IRepositoryAsync<BlogLikeMap>
{
}
