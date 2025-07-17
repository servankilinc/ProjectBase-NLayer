using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Model;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model.Dtos.Blog_;
using Model.Dtos.User_;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IUserService : IServiceBase<User>, IServiceBaseAsync<User>
{
    #region Get Entity
    Task<User?> GetAsync(Expression<Func<User, bool>> where, CancellationToken cancellationToken = default);
    Task<ICollection<User>?> GetListAsync(Expression<Func<User, bool>>? where = default, CancellationToken cancellationToken = default);
    #endregion

    #region Get Generic
    Task<TResponse?> GetAsync<TResponse>(Guid Id, CancellationToken cancellationToken = default) where TResponse : IDto;
    Task<ICollection<TResponse>?> GetListAsync<TResponse>(Expression<Func<User, bool>>? where = default, CancellationToken cancellationToken = default) where TResponse : IDto;
    #endregion

    #region SelectList
    Task<SelectList> GetSelectListAsync(Expression<Func<User, bool>>? where = default, CancellationToken cancellationToken = default);
    #endregion

    #region GetBasic
    Task<UserBasicResponseDto?> GetByBasicAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<UserBasicResponseDto>?> GetAllByBasicAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<UserBasicResponseDto>> GetListByBasicAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region GetDetail
    Task<UserDetailResponseDto?> GetByDetailAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<UserDetailResponseDto>?> GetAllByDetailAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<UserDetailResponseDto>> GetListByDetailAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion;

    #region Get-UserBlogsResponseDto
    Task<UserBlogsResponseDto?> GetUserBlogsResponseDtoAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<UserBlogsResponseDto>?> GetAllUserBlogsResponseDtoAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<UserBlogsResponseDto>> GetListUserBlogsResponseDtoAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region Create
    Task<UserBasicResponseDto> CreateAsync(UserCreateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<UserBasicResponseDto> UpdateAsync(UserUpdateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Delete
    Task DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    #endregion

    #region Datatable Methods
    Task<DatatableResponseClientSide<User>> DatatableClientSideAsync(DynamicRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseClientSide<UserReportDto>> DatatableClientSideByReportAsync(DynamicRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseServerSide<User>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseServerSide<UserReportDto>> DatatableServerSideByReportAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default);
    #endregion
}