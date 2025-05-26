using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Abstract;

public interface IBlogCommentRepository : IRepository<BlogComment>, IRepositoryAsync<BlogComment>
{
}