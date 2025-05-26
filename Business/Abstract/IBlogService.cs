using Core.Utils.DynamicQuery;
using Microsoft.EntityFrameworkCore.Query;
using Model.Dtos;
using Model.Dtos.Blog_;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Abstract;

public interface IBlogService
{
    #region Get 
    Task<Blog?> GetAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<Blog, bool>>? where = null,
        Func<IQueryable<Blog>, IOrderedQueryable<Blog>>? orderBy = null,
        Func<IQueryable<Blog>, IIncludableQueryable<Blog, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);
    Task<BlogBasicResponseDto?> GetResponseAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<Blog, bool>>? where = null,
        Func<IQueryable<Blog>, IOrderedQueryable<Blog>>? orderBy = null,
        Func<IQueryable<Blog>, IIncludableQueryable<Blog, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);
    #endregion

    #region Add
    Task<BlogBasicResponseDto> AddAsync(BlogCreateDto blogCreateDto, CancellationToken cancellationToken = default);
    Task<List<BlogBasicResponseDto>> AddRangeAsync(IEnumerable<BlogCreateDto> blogCreateDtos, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<BlogBasicResponseDto> UpdateAsync(Blog blog, CancellationToken cancellationToken = default);
    Task<List<BlogBasicResponseDto>> UpdateRangeAsync(IEnumerable<Blog> blogs, CancellationToken cancellationToken = default);
     #endregion

    #region Delete
    Task DeleteAsync(Expression<Func<Blog, bool>> where, CancellationToken cancellationToken = default);
    #endregion
}
