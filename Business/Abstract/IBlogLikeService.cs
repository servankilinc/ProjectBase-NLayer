using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Model.Dtos.BlogLike_;
using Model.Entities;

namespace Business.Abstract;

public interface IBlogLikeService : IServiceBase<BlogLike>, IServiceBaseAsync<BlogLike>
{
    #region GetBasic
    Task<BlogLikeListResponseDto?> GetByDetailAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default);
    Task<ICollection<BlogLikeListResponseDto>?> GetAllByDetailAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<BlogLikeListResponseDto>> GetListByDetailAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region GetDetail
    Task<BlogLikeResponseDto?> GetAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default);
    Task<ICollection<BlogLikeResponseDto>?> GetAllAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<BlogLikeResponseDto>> GetListAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region Create
    Task<BlogLike> CreateAsync(BlogLikeCreateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<BlogLike> UpdateAsync(BlogLike request, CancellationToken cancellationToken = default);
    #endregion

    #region Delete
    Task DeleteAsync(BlogLikeDeleteDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Datatable Methods
    Task<DatatableResponseClientSide<BlogLike>> DatatableClientSideAsync(DynamicRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseServerSide<BlogLike>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default);
    #endregion
}
