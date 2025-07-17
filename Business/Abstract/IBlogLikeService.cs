using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Model;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model.Dtos.Blog_;
using Model.Dtos.BlogLike_;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IBlogLikeService : IServiceBase<BlogLike>, IServiceBaseAsync<BlogLike>
{   
    #region Get Entity
    Task<BlogLike?> GetAsync(Expression<Func<BlogLike, bool>> where, CancellationToken cancellationToken = default);
    Task<ICollection<BlogLike>?> GetListAsync(Expression<Func<BlogLike, bool>>? where = default, CancellationToken cancellationToken = default);
    #endregion

    #region Get Generic
    Task<TResponse?> GetAsync<TResponse>(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default) where TResponse : IDto;
    Task<ICollection<TResponse>?> GetListAsync<TResponse>(Expression<Func<BlogLike, bool>>? where = default, CancellationToken cancellationToken = default) where TResponse : IDto;
    #endregion

    #region SelectList
    Task<SelectList> GetSelectListAsync(Expression<Func<BlogLike, bool>>? where = default, CancellationToken cancellationToken = default);
    #endregion

    #region GetBasic
    Task<BlogLikeResponseDto?> GetByBasicAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default);
    Task<ICollection<BlogLikeResponseDto>?> GetAllByBasicAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<BlogLikeResponseDto>> GetListByBasicAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region GetDetail
    Task<BlogLikeListResponseDto?> GetByDetailAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default);
    Task<ICollection<BlogLikeListResponseDto>?> GetAllByDetailAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<BlogLikeListResponseDto>> GetListByDetailAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region Create
    Task<BlogLikeResponseDto> CreateAsync(BlogLikeCreateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<BlogLikeResponseDto> UpdateAsync(BlogLike request, CancellationToken cancellationToken = default);
    #endregion

    #region Delete
    Task DeleteAsync(BlogLikeDeleteDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Datatable Methods
    Task<DatatableResponseClientSide<BlogLike>> DatatableClientSideAsync(DynamicRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseServerSide<BlogLike>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default);
    #endregion
}
