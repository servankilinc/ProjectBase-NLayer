using AutoMapper;
using Business.Abstract;
using Core.Utils.DynamicQuery;
using DataAccess.UoW;
using Microsoft.EntityFrameworkCore.Query;
using Model.Dtos;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Get
    public async Task<User?> GetAsync(
        Filter? filter = null, 
        IEnumerable<Sort>? sorts = null, 
        Expression<Func<User, bool>>? where = null, 
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, 
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null, 
        bool withDeleted = false, 
        bool enableTracking = true, 
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.UserRepository.GetAsync(filter, sorts, where, orderBy, include, withDeleted, enableTracking, cancellationToken);
    }
    //public async Task<UserResponseDto?> GetAsync(
    //    Filter? filter = null,
    //    IEnumerable<Sort>? sorts = null, 
    //    Expression<Func<User, bool>>? where = null,
    //    Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, 
    //    Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null,
    //    bool withDeleted = false,
    //    bool enableTracking = true,
    //    CancellationToken cancellationToken = default)
    //{
    //    Expression<Func<User, UserResponseDto>> select = s => new UserResponseDto
    //    {
    //        Id = s.Id,
    //        Name = s.Name,
    //        Email = s.Email
    //    };
    //    return await _unitOfWork.UserRepository.GetAsync(select, filter, sorts, where, orderBy, include, withDeleted, enableTracking, cancellationToken);
    //}
    #endregion

    #region Add
    public async Task<User> AddAsync(Dto_UserCreate dto_UserCreate, CancellationToken cancellationToken = default)
    {
        var dataToInsert = _mapper.Map<User>(dto_UserCreate);
        var created_user = _unitOfWork.UserRepository.Add(dataToInsert);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return created_user;
    }
    public async Task<List<User>> AddRangeAsync(IEnumerable<Dto_UserCreate> dto_UserCreates, CancellationToken cancellationToken = default)
    {
        var dataToInsert = _mapper.Map<IEnumerable<User>>(dto_UserCreates);
        var createdData = _unitOfWork.UserRepository.AddRange(dataToInsert);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return createdData;
    }

    //public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    //{ 
    //    var createdData = _unitOfWork.UserRepository.Add(user);
    //    await _unitOfWork.SaveChangesAsync(cancellationToken);
    //    return createdData;
    //}

    //public async Task<List<User>> AddRangeAsync(IEnumerable<User> users, CancellationToken cancellationToken = default)
    //{ 
    //    var createdData = _unitOfWork.UserRepository.AddRange(users);
    //    await _unitOfWork.SaveChangesAsync(cancellationToken);
    //    return createdData;
    //}

    #endregion

    #region Update
    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var updatedData = _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return updatedData;
    }
    public async Task<List<User>> UpdateRangeAsync(IEnumerable<User> users, CancellationToken cancellationToken = default)
    {
        var updatedData = _unitOfWork.UserRepository.UpdateRange(users);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return updatedData;
    }
    //public async Task<User> Update(UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default)
    //{
    //    var user = _mapper.Map<User>(userUpdateDto);
    //    var updatedData = _unitOfWork.UserRepository.Update(user);
    //    await _unitOfWork.SaveChangesAsync(cancellationToken);
    //    return updatedData;
    //}
    //public async Task<User> UpdateRange(IEnumerable<UserUpdateDto> userUpdateDtos, CancellationToken cancellationToken = default)
    //{
    //    var users = _mapper.Map<IEnumerable<User>>(userUpdateDtos);
    //    var updatedData = _unitOfWork.UserRepository.UpdateRange(users);
    //    await _unitOfWork.SaveChangesAsync(cancellationToken);
    //    return updatedData;
    //}
    #endregion

    #region Delete
    public async Task DeleteAsync(Expression<Func<User, bool>> where, CancellationToken cancellationToken = default)
    {
        _unitOfWork.UserRepository.DeleteByFilter(where);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    #endregion
}
