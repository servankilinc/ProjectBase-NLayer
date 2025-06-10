using AutoMapper;
using Business.Abstract;
using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Utils.CrossCuttingConcerns;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using Model.Dtos.Blog_;
using Model.Entities;

namespace Business.Concrete;

[ExceptionHandler]
public class BlogService : ServiceBase<Blog, IBlogRepository>, IBlogService
{
    public BlogService(IBlogRepository blogRepository, IMapper mapper) : base(blogRepository, mapper)
    {
    }

    #region GetBasic
    public async Task<BlogBasicResponseDto?> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == default) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync<BlogBasicResponseDto>(
            where: f => f.Id == Id,
            include: i => i.Include(x => x.Category),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<BlogBasicResponseDto>?> GetAllAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync<BlogBasicResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i.Include(x => x.Category),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<BlogBasicResponseDto>> GetListAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _PaginationAsync<BlogBasicResponseDto>(
            paginationRequest: request.PaginationRequest,
            filter: request.Filter,
            include: i => i.Include(x => x.Category),
            sorts: request.Sorts,
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region GetDetail
    public async Task<BlogDetailResponseDto?> GetByDetailAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == Guid.Empty) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync<BlogDetailResponseDto>(
            where: f => f.Id == Id,
            include: i => i
                .Include(x => x.Category)
                .Include(x => x.BlogComments),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<BlogDetailResponseDto>?> GetAllByDetailAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync<BlogDetailResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.Category)
                .Include(x => x.BlogComments),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<BlogDetailResponseDto>> GetListByDetailAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _PaginationAsync<BlogDetailResponseDto>(
            paginationRequest: request.PaginationRequest,
            filter: request.Filter,
            sorts: request.Sorts,
            include: i => i
                .Include(x => x.Category)
                .Include(x => x.BlogComments),
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region Create
    [Validation(typeof(BlogCreateDto))]
    public async Task<Blog> CreateAsync(BlogCreateDto request, CancellationToken cancellationToken = default)
    {
        var result = await _AddAsync(request, cancellationToken);

        return result;
    }
    #endregion

    #region Update
    [Validation(typeof(BlogUpdateDto))]
    public async Task<Blog> UpdateAsync(BlogUpdateDto request, CancellationToken cancellationToken = default)
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
    public async Task<DatatableResponseClientSide<Blog>> DatatableClientSideAsync(DynamicRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableClientSideAsync(
            filter: request.Filter,
            sorts: request.Sorts,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<DatatableResponseServerSide<Blog>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default)
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
