using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Abstract;

public interface ICategoryRepository: IRepository<Category>, IRepositoryAsync<Category>
{
}
