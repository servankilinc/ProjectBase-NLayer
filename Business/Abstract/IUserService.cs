using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Model.Dtos.User_;
using Model.Entities;

namespace Business.Abstract;

public interface IUserService : IServiceBase<User>, IServiceBaseAsync<User>
{
    #region GetBasic
    Task<UserBasicResponseDto?> GetAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<UserBasicResponseDto>?> GetAllAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<UserBasicResponseDto>> GetListAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
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
    Task<User> CreateAsync(UserCreateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<User> UpdateAsync(UserUpdateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Delete
    Task DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    #endregion

    #region Datatable Methods
    Task<DatatableResponseClientSide<User>> DatatableClientSideAsync(DynamicRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseServerSide<User>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default);
    #endregion
}