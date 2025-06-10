using AutoMapper;
using Business.Abstract;
using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Utils.CrossCuttingConcerns;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using Model.Dtos.BlogLike_;
using Model.Entities;

namespace Business.Concrete;

[ExceptionHandler]
public class BlogLikeService : ServiceBase<BlogLike, IBlogLikeRepository>, IBlogLikeService
{
    public BlogLikeService(IBlogLikeRepository blogLikeRepository, IMapper mapper) : base(blogLikeRepository, mapper)
    {
    }

    #region GetBasic
    public async Task<BlogLikeResponseDto?> GetAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default)
    {
        if (BlogId == default) throw new ArgumentNullException(nameof(BlogId));
        if (UserId == default) throw new ArgumentNullException(nameof(UserId));

        var result = await _GetAsync<BlogLikeResponseDto>(
            where: f => f.BlogId == BlogId && f.UserId == UserId,
            include: i => i.Include(x => x.User),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<BlogLikeResponseDto>?> GetAllAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync<BlogLikeResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i.Include(x => x.User),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<BlogLikeResponseDto>> GetListAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _PaginationAsync<BlogLikeResponseDto>(
            paginationRequest: request.PaginationRequest,
            filter: request.Filter,
            sorts: request.Sorts,
            include: i => i.Include(x => x.User),
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region GetDetail
    public async Task<BlogLikeListResponseDto?> GetByDetailAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default)
    {
        if (BlogId == default) throw new ArgumentNullException(nameof(BlogId));
        if (UserId == default) throw new ArgumentNullException(nameof(UserId));

        var result = await _GetAsync<BlogLikeListResponseDto>(
            where: f => f.BlogId == BlogId && f.UserId == UserId,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<BlogLikeListResponseDto>?> GetAllByDetailAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync<BlogLikeListResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<BlogLikeListResponseDto>> GetListByDetailAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _PaginationAsync<BlogLikeListResponseDto>(
            paginationRequest: request.PaginationRequest,
            filter: request.Filter,
            sorts: request.Sorts,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region Create
    [Validation(typeof(BlogLikeCreateDto))]
    public async Task<BlogLike> CreateAsync(BlogLikeCreateDto request, CancellationToken cancellationToken = default)
    {
        var result = await _AddAsync(request, cancellationToken);

        return result;
    }
    #endregion

    #region Update
    [Validation(typeof(BlogLike))]
    public async Task<BlogLike> UpdateAsync(BlogLike request, CancellationToken cancellationToken = default)
    {
        var result = await _UpdateAsync(request, cancellationToken);

        return result;
    }
    #endregion

    #region Delete
    public async Task DeleteAsync(BlogLikeDeleteDto request, CancellationToken cancellationToken = default)
    {
        await _DeleteAsync(where: f => f.BlogId == request.BlogId && f.UserId == request.UserId, cancellationToken);
    }
    #endregion

    #region Datatable Methods
    public async Task<DatatableResponseClientSide<BlogLike>> DatatableClientSideAsync(DynamicRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableClientSideAsync(
            filter: request.Filter,
            sorts: request.Sorts,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<DatatableResponseServerSide<BlogLike>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default)
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
