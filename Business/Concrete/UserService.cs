using AutoMapper;
using Business.Abstract;
using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Model;
using Core.Utils.CrossCuttingConcerns;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using DataAccess.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model.Dtos.Blog_;
using Model.Dtos.User_;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

[ExceptionHandler]
public class UserService : ServiceBase<User, IUserRepository>, IUserService
{
    public UserService(IUserRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }

    #region Get Entity
    public async Task<User?> GetAsync(Expression<Func<User, bool>> where, CancellationToken cancellationToken = default)
    {
        var result = await _GetAsync(
            where: where,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<User>?> GetListAsync(Expression<Func<User, bool>>? where = null, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync(
             where: where,
             tracking: false,
             cancellationToken: cancellationToken
         );

        return result;
    }
    #endregion

    #region Get Generic
    public async Task<TResponse?> GetAsync<TResponse>(Guid Id, CancellationToken cancellationToken = default) where TResponse : IDto
    {
        if (Id == default) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync<TResponse>(
            where: f => f.Id == Id,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<TResponse>?> GetListAsync<TResponse>(Expression<Func<User, bool>>? where = null, CancellationToken cancellationToken = default) where TResponse : IDto
    {
        var result = await _GetListAsync<TResponse>(
            where: where,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region SelectList
    public async Task<SelectList> GetSelectListAsync(Expression<Func<User, bool>>? where = default, CancellationToken cancellationToken = default)
    {
        var result = new SelectList(await _GetListAsync(
            where: where,
            tracking: false,
            cancellationToken: cancellationToken
        ), "Id", "Name");

        return result;
    }
    #endregion

    #region GetBasic
    public async Task<UserBasicResponseDto?> GetByBasicAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == default) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync<UserBasicResponseDto>(
            where: f => f.Id == Id,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<UserBasicResponseDto>?> GetAllByBasicAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync<UserBasicResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<UserBasicResponseDto>> GetListByBasicAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _PaginationAsync<UserBasicResponseDto>(
            paginationRequest: request.PaginationRequest,
            filter: request.Filter,
            sorts: request.Sorts,
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region GetDetail
    public async Task<UserDetailResponseDto?> GetByDetailAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == Guid.Empty) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync<UserDetailResponseDto>(
            where: f => f.Id == Id,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<UserDetailResponseDto>?> GetAllByDetailAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync<UserDetailResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<UserDetailResponseDto>> GetListByDetailAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _PaginationAsync<UserDetailResponseDto>(
            paginationRequest: request.PaginationRequest,
            filter: request.Filter,
            sorts: request.Sorts,
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region Get-UserBlogsResponseDto
    public async Task<UserBlogsResponseDto?> GetUserBlogsResponseDtoAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == Guid.Empty) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync<UserBlogsResponseDto>(
            where: f => f.Id == Id,
            include: i =>
                i.Include(x => x.Blogs),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<UserBlogsResponseDto>?> GetAllUserBlogsResponseDtoAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync<UserBlogsResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i =>
                i.Include(x => x.Blogs),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<UserBlogsResponseDto>> GetListUserBlogsResponseDtoAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _PaginationAsync<UserBlogsResponseDto>(
            paginationRequest: request.PaginationRequest,
            filter: request.Filter,
            sorts: request.Sorts,
            include: i =>
                i.Include(x => x.Blogs),
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region Create
    [Validation(typeof(UserCreateDto))]
    public async Task<UserBasicResponseDto> CreateAsync(UserCreateDto request, CancellationToken cancellationToken = default)
    {
        var result = await _AddAsync<UserCreateDto, UserBasicResponseDto>(request, cancellationToken);

        return result;
    }
    #endregion

    #region Update
    [Validation(typeof(UserUpdateDto))]
    public async Task<UserBasicResponseDto> UpdateAsync(UserUpdateDto request, CancellationToken cancellationToken = default)
    {
        var result = await _UpdateAsync<UserUpdateDto, UserBasicResponseDto>(updateModel: request, where: f => f.Id == request.Id, cancellationToken);

        return result;
    }
    #endregion

    #region Delete
    public async Task DeleteAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == Guid.Empty) throw new ArgumentNullException(nameof(Id));

        await _DeleteAsync(where: f => f.Id == Id, cancellationToken);
    }
    #endregion

    #region Datatable Methods
    public async Task<DatatableResponseClientSide<User>> DatatableClientSideAsync(DynamicRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableClientSideAsync(
            filter: request.Filter,
            sorts: request.Sorts,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<DatatableResponseClientSide<UserReportDto>> DatatableClientSideByReportAsync(DynamicRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableClientSideAsync<UserReportDto>(
            filter: request.Filter,
            sorts: request.Sorts,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<DatatableResponseServerSide<User>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableServerSideAsync(
            datatableRequest: request.GetDatatableRequest(),
            filter: request.Filter,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<DatatableResponseServerSide<UserReportDto>> DatatableServerSideByReportAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableServerSideAsync<UserReportDto>(
            datatableRequest: request.GetDatatableRequest(),
            filter: request.Filter,
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion
}
