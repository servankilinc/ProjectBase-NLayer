using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Abstract;

public interface IRefreshTokenRepository : IRepository<RefreshToken>, IRepositoryAsync<RefreshToken>
{
}
