using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Abstract;

public interface IBlogCommentMapRepository : IRepository<BlogCommentMap>, IRepositoryAsync<BlogCommentMap>
{
}