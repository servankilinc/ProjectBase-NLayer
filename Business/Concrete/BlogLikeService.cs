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
using Model.Dtos.BlogLike_;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

[ExceptionHandler]
public class BlogLikeService : ServiceBase<BlogLike, IBlogLikeRepository>, IBlogLikeService
{
    public BlogLikeService(IBlogLikeRepository blogLikeRepository, IMapper mapper) : base(blogLikeRepository, mapper)
    {
    }

    #region Get Entity
    public async Task<BlogLike?> GetAsync(Expression<Func<BlogLike, bool>> where, CancellationToken cancellationToken = default)
    {
        var result = await _GetAsync(
            where: where,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<BlogLike>?> GetListAsync(Expression<Func<BlogLike, bool>>? where = null, CancellationToken cancellationToken = default)
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
    public async Task<TResponse?> GetAsync<TResponse>(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default) where TResponse : IDto
    {
        if (BlogId == default) throw new ArgumentNullException(nameof(BlogId));
        if (UserId == default) throw new ArgumentNullException(nameof(UserId));

        var result = await _GetAsync<TResponse>(
            where: f => f.BlogId == BlogId && f.UserId == UserId,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<TResponse>?> GetListAsync<TResponse>(Expression<Func<BlogLike, bool>>? where = null, CancellationToken cancellationToken = default) where TResponse : IDto
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
    public async Task<SelectList> GetSelectListAsync(Expression<Func<BlogLike, bool>>? where = default, CancellationToken cancellationToken = default)
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
    public async Task<BlogLikeResponseDto?> GetByBasicAsync(Guid BlogId, Guid UserId, CancellationToken cancellationToken = default)
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

    public async Task<ICollection<BlogLikeResponseDto>?> GetAllByBasicAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
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

    public async Task<PaginationResponse<BlogLikeResponseDto>> GetListByBasicAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
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
    public async Task<BlogLikeResponseDto> CreateAsync(BlogLikeCreateDto request, CancellationToken cancellationToken = default)
    {
        var result = await _AddAsync<BlogLikeCreateDto, BlogLikeResponseDto>(request, cancellationToken);

        return result;
    }
    #endregion

    #region Update
    [Validation(typeof(BlogLike))]
    public async Task<BlogLikeResponseDto> UpdateAsync(BlogLike request, CancellationToken cancellationToken = default)
    {
        var result = await _UpdateAsync<BlogLikeResponseDto>(request, where: f => f.UserId == request.UserId && f.BlogId == request.BlogId, cancellationToken);

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
            datatableRequest: request.GetDatatableRequest(),
            filter: request.Filter,
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion
}
