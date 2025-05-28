using AutoMapper;
using Business.Abstract;
using Business.ServiceBase;
using DataAccess.Abstract;
using Model.Entities;

namespace Business.Concrete;

public class UserService : ServiceBase<User, IUserRepository>, IUserService
{
    public UserService(IUserRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}
