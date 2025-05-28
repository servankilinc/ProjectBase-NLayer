using Business.ServiceBase;
using Model.Entities;

namespace Business.Abstract;

public interface IUserService : IServiceBase<User>, IServiceBaseAsync<User>
{
}