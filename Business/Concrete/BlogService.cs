﻿using AutoMapper;
using Business.Abstract;
using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Model;
using Core.Utils.CrossCuttingConcerns;
using Core.Utils.Datatable;
using Core.Utils.ExceptionHandle.Exceptions;
using Core.Utils.Pagination;
using DataAccess.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model.Dtos.Blog_;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

[ExceptionHandler]
public class BlogService : ServiceBase<Blog, IBlogRepository>, IBlogService
{
    public BlogService(IBlogRepository blogRepository, IMapper mapper) : base(blogRepository, mapper)
    {
    }

    #region Get Entity
    public async Task<Blog?> GetAsync(Expression<Func<Blog, bool>> where, CancellationToken cancellationToken = default)
    {
        var result = await _GetAsync(
            where: where,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<Blog>?> GetListAsync(Expression<Func<Blog, bool>>? where = null, CancellationToken cancellationToken = default)
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
    public async Task<TResponse?> GetAsync<TResponse>(Expression<Func<Blog, bool>> where, CancellationToken cancellationToken = default) where TResponse : IDto
    {
        var result = await _GetAsync<TResponse>(
            where: where,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<TResponse>?> GetListAsync<TResponse>(Expression<Func<Blog, bool>>? where = null, CancellationToken cancellationToken = default) where TResponse : IDto
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
    public async Task<SelectList> GetSelectListAsync(Expression<Func<Blog, bool>>? where = default, CancellationToken cancellationToken = default)
    {
        var result = new SelectList(await _GetListAsync(
            select: s => new
            {   
                s.Id, 
                s.Title
            },
            where: where,
            tracking: false,
            cancellationToken: cancellationToken
        ), "Id", "Title");

        return result;
    }
    #endregion

    #region Get
    public async Task<Blog?> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == default) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync(
            where: f => f.Id == Id,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<Blog>?> GetAllAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync(
            filter: request?.Filter,
            sorts: request?.Sorts,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<Blog>> GetListAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
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
    public async Task<BlogBasicResponseDto?> GetByBasicAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == default) throw new ArgumentNullException(nameof(Id));

        var result = await _GetAsync<BlogBasicResponseDto>(
            where: f => f.Id == Id,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<ICollection<BlogBasicResponseDto>?> GetAllByBasicAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync<BlogBasicResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category),
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<PaginationResponse<BlogBasicResponseDto>> GetListByBasicAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _PaginationAsync<BlogBasicResponseDto>(
            paginationRequest: request.PaginationRequest,
            filter: request.Filter,
            sorts: request.Sorts,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category),
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
                .Include(x => x.Author)
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
                .Include(x => x.Author)
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
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Include(x => x.BlogComments),
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region Get-BlogLikeListResponseDto
    public async Task<BlogLikeListResponseDto?> GetBlogLikeListResponseDtoAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == default) throw new ArgumentNullException(nameof(id));

        var result = await _GetAsync<BlogLikeListResponseDto>(
            where: f => f.Id == id,
            include: i => i
                .Include(x => x.BlogLikes)
                    .ThenInclude(x => x.User)
,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }
    public async Task<ICollection<BlogLikeListResponseDto>?> GetAllBlogLikeListResponseDtoAsync(DynamicRequest? request, CancellationToken cancellationToken = default)
    {
        var result = await _GetListAsync<BlogLikeListResponseDto>(
            filter: request?.Filter,
            sorts: request?.Sorts,
            include: i => i
                .Include(x => x.BlogLikes)
                    .ThenInclude(x => x.User)
,
            tracking: false,
            cancellationToken: cancellationToken
        );

        return result;
    }
    public async Task<PaginationResponse<BlogLikeListResponseDto>> GetListBlogLikeListResponseDtoAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _PaginationAsync<BlogLikeListResponseDto>(
            paginationRequest: request.PaginationRequest,
            filter: request.Filter,
            sorts: request.Sorts,
            include: i => i
                .Include(x => x.BlogLikes)
                    .ThenInclude(x => x.User)
,
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion

    #region Create
    [Validation(typeof(BlogCreateDto))]
    public async Task<BlogBasicResponseDto> CreateAsync(BlogCreateDto request, CancellationToken cancellationToken = default)
    {
        var result = await _AddAsync<BlogCreateDto, BlogBasicResponseDto>(request, cancellationToken);

        return result;
    }
    #endregion

    #region Update
    [Validation(typeof(BlogUpdateDto))]
    public async Task<BlogBasicResponseDto> UpdateAsync(BlogUpdateDto request, CancellationToken cancellationToken = default)
    {
        throw new BusinessException("Test Amaçlı Bir Hata");
        var result = await _UpdateAsync<BlogUpdateDto, BlogBasicResponseDto>(updateModel: request, where: f => f.Id == request.Id, cancellationToken);

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

    public async Task<DatatableResponseClientSide<BlogReportDto>> DatatableClientSideByReportAsync(DynamicRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableClientSideAsync<BlogReportDto>(
            filter: request.Filter,
            sorts: request.Sorts,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category),
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<DatatableResponseServerSide<Blog>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableServerSideAsync(
            datatableRequest: request.GetDatatableRequest(),
            filter: request.Filter,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task<DatatableResponseServerSide<BlogReportDto>> DatatableServerSideByReportAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _DatatableServerSideAsync<BlogReportDto>(
            datatableRequest: request.GetDatatableRequest(),
            filter: request.Filter,
            include: i => i
                .Include(x => x.Author)
                .Include(x => x.Category),
            cancellationToken: cancellationToken
        );

        return result;
    }
    #endregion
}
