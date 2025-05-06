using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Abstract;

public interface IBlogRepository : IRepository<Blog>, IRepositoryAsync<Blog>
{
}
