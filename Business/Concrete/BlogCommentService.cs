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
using Model.Dtos.BlogComment_;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

[ExceptionHandler]
public class BlogCommentService : ServiceBase<BlogComment, IBlogCommentRepository>, IBlogCommentService
{
    public BlogCommentService(IBlogCommentRepository blogCommentRepository, IMapper mapper) : base(blogCommentRepository, mapper)
    {
    }

    #region Get Entity
    public async Task<BlogComment?> GetAsync(Expression<Func<BlogComment, bool>> where, CancellationToken cancellationToken = default)
    {
        var result = await _GetAsync(
            where: where,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<BlogComment>?> GetListAsync(Expression<Func<BlogComment, bool>>? where = null, CancellationToken cancellationToken = default)
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
    public async Task<TResponse?> GetAsync<TResponse>(Expression<Func<BlogComment, bool>> where, CancellationToken cancellationToken = default) where TResponse : IDto
    {
        var result = await _GetAsync<TResponse>(
            where: where,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<TResponse>?> GetListAsync<TResponse>(Expression<Func<BlogComment, bool>>? where = null, CancellationToken cancellationToken = default) where TResponse : IDto
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
    public async Task<SelectList> GetSelectListAsync(Expression<Func<BlogComment, bool>>? where = default, CancellationToken cancellationToken = default)
    {
        var result = new SelectList(await _GetListAsync(
            select: s => new
            {
                s.Id,
                s.Comment
            },
            where: where,
            tracking: false,
            cancellationToken: cancellationToken
        ), "Id", "Comment");

        return result;
    }
    #endregion

    #region Get
    public async Task<BlogComment?> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == default) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync(
            where: f => f.Id == Id,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<BlogComment>?> GetAllAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<BlogComment>> GetListAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _PaginationAsync(
            paginationRequest: request.PaginationRequest,
            filter: request.Filter,
            sorts: request.Sorts,
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region GetBasic
    public async Task<BlogCommentBasicResponseDto?> GetByBasicAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == default) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync<BlogCommentBasicResponseDto>(
            where: f => f.Id == Id,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<BlogCommentBasicResponseDto>?> GetAllByBasicAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync<BlogCommentBasicResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<BlogCommentBasicResponseDto>> GetListByBasicAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
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
    public async Task<BlogCommentBasicResponseDto> CreateAsync(BlogCommentCreateDto request, CancellationToken cancellationToken = default)
    {
        var result = await _AddAsync<BlogCommentCreateDto, BlogCommentBasicResponseDto>(request, cancellationToken);

        return result;
    }
    #endregion

    #region Update
    [Validation(typeof(BlogCommentUpdateDto))]
    public async Task<BlogCommentBasicResponseDto> UpdateAsync(BlogCommentUpdateDto request, CancellationToken cancellationToken = default)
    {
        var result = await _UpdateAsync<BlogCommentUpdateDto, BlogCommentBasicResponseDto>(updateModel: request, where: f => f.Id == request.Id, cancellationToken);

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
            datatableRequest: request.GetDatatableRequest(),
            filter: request.Filter,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<DatatableResponseClientSide<BlogCommentReportDto>> DatatableClientSideByReportAsync(DynamicRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableClientSideAsync<BlogCommentReportDto>(
            filter: request.Filter,
            sorts: request.Sorts,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<DatatableResponseServerSide<BlogCommentReportDto>> DatatableServerSideByReportAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableServerSideAsync<BlogCommentReportDto>(
            datatableRequest: request.GetDatatableRequest(),
            filter: request.Filter,
            include: i => i
                .Include(x => x.Blog)
                .Include(x => x.User),
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion
}
