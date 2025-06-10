using AutoMapper;
using Business.Abstract;
using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Utils.CrossCuttingConcerns;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using Model.Dtos.User_;
using Model.Entities;

namespace Business.Concrete;

[ExceptionHandler]
public class UserService : ServiceBase<User, IUserRepository>, IUserService
{
    public UserService(IUserRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }

    #region GetBasic
    public async Task<UserBasicResponseDto?> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == default) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync<UserBasicResponseDto>(
            where: f => f.Id == Id,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<UserBasicResponseDto>?> GetAllAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync<UserBasicResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<UserBasicResponseDto>> GetListAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
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
    public async Task<User> CreateAsync(UserCreateDto request, CancellationToken cancellationToken = default)
    {
        var result = await _AddAsync(request, cancellationToken);

        return result;
    }
    #endregion

    #region Update
    [Validation(typeof(UserUpdateDto))]
    public async Task<User> UpdateAsync(UserUpdateDto request, CancellationToken cancellationToken = default)
    {
        var result = await _UpdateAsync(updateModel: request, where: f => f.Id == request.Id, cancellationToken);

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

    public async Task<DatatableResponseServerSide<User>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableServerSideAsync(
            datatableRequest: request.DatatableRequest,
            filter: request.Filter,
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion
}
