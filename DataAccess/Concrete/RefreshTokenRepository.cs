using Core.Utils.CrossCuttingConcerns;
using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repository;
using Model.Entities;

namespace DataAccess.Concrete;

[DataAccessException]
public class RefreshTokenRepository : RepositoryBase<RefreshToken, AppDbContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AppDbContext context) : base(context)
    {
    }
}