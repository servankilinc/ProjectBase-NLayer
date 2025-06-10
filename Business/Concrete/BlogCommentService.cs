using AutoMapper;
using Business.Abstract;
using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Utils.CrossCuttingConcerns;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using Model.Dtos.BlogComment_;
using Model.Entities;

namespace Business.Concrete;

[ExceptionHandler]
public class BlogCommentService : ServiceBase<BlogComment, IBlogCommentRepository>, IBlogCommentService
{
    public BlogCommentService(IBlogCommentRepository blogCommentRepository, IMapper mapper) : base(blogCommentRepository, mapper)
    {
    }

    #region GetBasic
    public async Task<BlogCommentBasicResponseDto?> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == default) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync<BlogCommentBasicResponseDto>(
            where: f => f.Id == Id,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<BlogCommentBasicResponseDto>?> GetAllAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync<BlogCommentBasicResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<BlogCommentBasicResponseDto>> GetListAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _PaginationAsync<BlogCommentBasicResponseDto>(
            paginationRequest: request.PaginationRequest,
            filter: request.Filter,
            sorts: request.Sorts,
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region GetDetail
    public async Task<BlogCommentDetailResponseDto?> GetByDetailAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == Guid.Empty) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync<BlogCommentDetailResponseDto>(
            where: f => f.Id == Id,
            include: i => i
                .Include(x => x.User),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<BlogCommentDetailResponseDto>?> GetAllByDetailAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync<BlogCommentDetailResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.User),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<BlogCommentDetailResponseDto>> GetListByDetailAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _PaginationAsync<BlogCommentDetailResponseDto>(
            paginationRequest: request.PaginationRequest,
            filter: request.Filter,
            sorts: request.Sorts,
            include: i => i
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region Create
    [Validation(typeof(BlogCommentCreateDto))]
    public async Task<BlogComment> CreateAsync(BlogCommentCreateDto request, CancellationToken cancellationToken = default)
    {
        var result = await _AddAsync(request, cancellationToken);

        return result;
    }
    #endregion

    #region Update
    [Validation(typeof(BlogCommentUpdateDto))]
    public async Task<BlogComment> UpdateAsync(BlogCommentUpdateDto request, CancellationToken cancellationToken = default)
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
    public async Task<DatatableResponseClientSide<BlogComment>> DatatableClientSideAsync(DynamicRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableClientSideAsync(
            filter: request.Filter,
            sorts: request.Sorts,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<DatatableResponseServerSide<BlogComment>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default)
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
