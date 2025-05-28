using Business.ServiceBase;
using Model.Entities;

namespace Business.Abstract;

public interface ICategoryService : IServiceBase<Category>, IServiceBaseAsync<Category>
{
}
