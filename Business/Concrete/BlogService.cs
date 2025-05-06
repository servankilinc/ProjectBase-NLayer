using AutoMapper;
using Business.Abstract;
using Core.Utils.DynamicQuery;
using DataAccess.UoW;
using Microsoft.EntityFrameworkCore.Query;
using Model.Dtos;
using Model.Entities;
using System.Linq.Expressions;

namespace Business.Concrete;

public class BlogService : IBlogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public BlogService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }



    #region Get
    public async Task<Blog?> GetAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<Blog, bool>>? where = null,
        Func<IQueryable<Blog>, IOrderedQueryable<Blog>>? orderBy = null,
        Func<IQueryable<Blog>, IIncludableQueryable<Blog, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    { 
        return await _unitOfWork.BlogRepository.GetAsync(filter, sorts, where, orderBy, include, withDeleted, enableTracking, cancellationToken);
    }
    public async Task<BlogBasicResponseDto?> GetResponseAsync(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<Blog, bool>>? where = null,
        Func<IQueryable<Blog>, IOrderedQueryable<Blog>>? orderBy = null,
        Func<IQueryable<Blog>, IIncludableQueryable<Blog, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        Expression<Func<Blog, BlogBasicResponseDto>> select = s => new BlogBasicResponseDto
        {
            BlogId = s.Id,
            Title = s.Title,
            AuthorName = s.User.Name,
            BannerImage = s.BannerImageSource,
            LikeCount = s.LikeCount,
            CommentCount = s.CommentCount,
        };
        return await _unitOfWork.BlogRepository.GetAsync(select, filter, sorts, where, orderBy, include, withDeleted, enableTracking, cancellationToken);
    }
    #endregion

    #region Add
    public async Task<BlogBasicResponseDto> AddAsync(BlogCreateDto blogCreateDto, CancellationToken cancellationToken = default)
    {
        var dataToInsert = _mapper.Map<Blog>(blogCreateDto);
        var createdData = _unitOfWork.BlogRepository.Add(dataToInsert);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<BlogBasicResponseDto>(createdData);
    }
    public async Task<List<BlogBasicResponseDto>> AddRangeAsync(IEnumerable<BlogCreateDto> blogCreateDtos, CancellationToken cancellationToken = default)
    {
        var dataToInsert = _mapper.Map<IEnumerable<Blog>>(blogCreateDtos);
        var createdData = _unitOfWork.BlogRepository.AddRange(dataToInsert);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<List<BlogBasicResponseDto>>(createdData);
    }
    #endregion

    #region Update
    public async Task<BlogBasicResponseDto> UpdateAsync(Blog blog, CancellationToken cancellationToken = default)
    {
        var updatedData = _unitOfWork.BlogRepository.Update(blog);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<BlogBasicResponseDto>(updatedData);
    }
    public async Task<List<BlogBasicResponseDto>> UpdateRangeAsync(IEnumerable<Blog> blogs, CancellationToken cancellationToken = default)
    {
        var updatedData = _unitOfWork.BlogRepository.UpdateRange(blogs);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<List<BlogBasicResponseDto>>(updatedData);
    }
    #endregion

    #region Delete
    public async Task DeleteAsync(Expression<Func<Blog, bool>> where, CancellationToken cancellationToken = default)
    {
        _unitOfWork.BlogRepository.DeleteByFilter(where);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    #endregion
}
