using Core.Utils.DynamicQuery;
using Microsoft.EntityFrameworkCore.Query;
using Model.Dtos;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IUserService
{
    #region Get
    Task<User?> GetAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<User, bool>>? where = null,
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);

    //Task<UserResponseDto?> GetAsync(
    //    Filter? filter = null,
    //    IEnumerable<Sort>? sorts = null,
    //    Expression<Func<User, bool>>? where = null,
    //    Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
    //    Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null,
    //    bool withDeleted = false,
    //    bool enableTracking = true,
    //    CancellationToken cancellationToken = default);
    #endregion

    #region Add
    Task<User> AddAsync(Dto_UserCreate dto_UserCreate, CancellationToken cancellationToken = default);
    Task<List<User>> AddRangeAsync(IEnumerable<Dto_UserCreate> dto_UserCreates, CancellationToken cancellationToken = default);
    //Task<User> AddAsync(User user, CancellationToken cancellationToken = default);
    //Task<List<User>> AddRangeAsync(IEnumerable<User> users, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<List<User>> UpdateRangeAsync(IEnumerable<User> users, CancellationToken cancellationToken = default);
    //Task<User> Update(UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default);
    //Task<List<User>> UpdateRangeAsync(IEnumerable<UserUpdateDto> userUpdateDtos, CancellationToken cancellationToken = default);
    #endregion

    #region Delete
    Task DeleteAsync(Expression<Func<User, bool>> where, CancellationToken cancellationToken = default);
    #endregion
}