using Core.Model;
using Core.Utils.Datatable;
using Core.Utils.DynamicQuery;
using Core.Utils.Pagination;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Repository;

public class RepositoryBase<TEntity, TContext> : IRepository<TEntity>, IRepositoryAsync<TEntity>
    where TEntity : class, IEntity
    where TContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    // DbContext 
    // IdentityDbContext<User, IdentityRole<Guid>, Guid> && IdentityDbContext<User, Role<Guid>, Guid>
{
    protected TContext _context { get; set; }
    public RepositoryBase(TContext context) => _context = context;


    // ********************* Sync Methods *********************
    #region Insert
    public TEntity Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        //_context.SaveChanges();
        return entity;
    }

    public List<TEntity> AddRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
        //_context.SaveChanges();
        return entities.ToList();
    }
    #endregion

    #region Update
    public TEntity Update(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        //_context.SaveChanges();
        return entity;
    }

    public List<TEntity> UpdateRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().UpdateRange(entities);
        //_context.SaveChanges();
        return entities.ToList();
    }
    #endregion

    #region Delete
    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        //_context.SaveChanges();
    }

    public void DeleteByFilter(Expression<Func<TEntity, bool>> where)
    {
        var entitiesToDelete = _context.Set<TEntity>().Where(where).ToList();
        _context.Set<TEntity>().RemoveRange(entitiesToDelete);
        //_context.SaveChanges();
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
        //_context.SaveChanges();
    }
    #endregion

    #region IsExist &  Count
    public bool IsExist(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool withDeleted = false)
    {
        var query = _context.Set<TEntity>().AsQueryable();
     
        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (withDeleted) query = query.IgnoreQueryFilters();
        
        return query.Any();
    }
    public int Count(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool withDeleted = false)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (withDeleted) query = query.IgnoreQueryFilters();

        return query.Count();
    }
    #endregion

    #region Get
    public TEntity? Get(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        if (!enableTracking) query = query.AsNoTracking();

        return query.FirstOrDefault();
    }

    public TResult? Get<TResult>(
        Expression<Func<TEntity, TResult>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true)
    {
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        if (!enableTracking) query = query.AsNoTracking();

        return query.Select(select).FirstOrDefault();
    }
    #endregion

    #region GetAll
    public ICollection<TEntity> GetAll(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        if (!enableTracking) query = query.AsNoTracking();

        return query.ToList();
    }

    public ICollection<TResult> GetAll<TResult>(
        Expression<Func<TEntity, TResult>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true)
    {
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        if (!enableTracking) query = query.AsNoTracking();

        return query.Select(select).ToList();
    }
    #endregion

    #region Datatable Server Side
    public DatatableResponseServerSide<TEntity> GetDatatableServerSide(
        DatatableRequest datatableRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false)
    {
        if (datatableRequest == null) throw new ArgumentNullException(nameof(datatableRequest));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return query.ToDatatableServerSide(datatableRequest);
    }
    public DatatableResponseServerSide<TResult> GetDatatableServerSide<TResult>(
        DatatableRequest datatableRequest,
        Expression<Func<TEntity, TResult>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false)
    {
        if (datatableRequest == null) throw new ArgumentNullException(nameof(datatableRequest));
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return query.Select(select).ToDatatableServerSide(datatableRequest);
    }
    #endregion

    #region Datatable Client Side
    public DatatableResponseClientSide<TEntity> GetDatatableClientSide(
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return query.ToDatatableClientSide();
    }
    public DatatableResponseClientSide<TResult> GetDatatableClientSide<TResult>(
      Expression<Func<TEntity, TResult>> select,
      Filter? filter = null,
      IEnumerable<Sort>? sorts = null,
      Expression<Func<TEntity, bool>>? where = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
      bool withDeleted = false)
    {
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return query.Select(select).ToDatatableClientSide();
    }
    #endregion

    #region Pagination
    public PaginationResponse<TEntity> GetPagination(
        PaginationRequest paginationRequest,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false)
    {
        if (paginationRequest == null) throw new ArgumentNullException(nameof(paginationRequest));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return query.ToPaginate(paginationRequest);
    }
    public PaginationResponse<TResult> GetPagination<TResult>(
        PaginationRequest paginationRequest,
        Expression<Func<TEntity, TResult>> select,
        Filter? filter = null,
        IEnumerable<Sort>? sorts = null,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false)
    {
        if (paginationRequest == null) throw new ArgumentNullException(nameof(paginationRequest));
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return query.Select(select).ToPaginate(paginationRequest);
    }
    #endregion

    // ********************* Async Methods *********************
    #region IsExist & Count
    public async Task<bool> IsExistAsync(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool withDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (withDeleted) query = query.IgnoreQueryFilters();

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Filter? filter = null, Expression<Func<TEntity, bool>>? where = null, bool withDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (withDeleted) query = query.IgnoreQueryFilters();

        return await query.CountAsync(cancellationToken);
    }
    #endregion

    #region Get
    public async Task<TEntity?> GetAsync(Filter? filter = null, IEnumerable<Sort>? sorts = null, Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        if (!enableTracking) query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TResult?> GetAsync<TResult>(Expression<Func<TEntity, TResult>> select, Filter? filter = null, IEnumerable<Sort>? sorts = null, Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        if (!enableTracking) query = query.AsNoTracking();

        return await query.Select(select).FirstOrDefaultAsync(cancellationToken);
    }
    #endregion

    #region GetAll
    public async Task<ICollection<TEntity>> GetAllAsync(Filter? filter = null, IEnumerable<Sort>? sorts = null, Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        if (!enableTracking) query = query.AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<ICollection<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, TResult>> select, Filter? filter = null, IEnumerable<Sort>? sorts = null, Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        if (!enableTracking) query = query.AsNoTracking();

        return await query.Select(select).ToListAsync();
    }
    #endregion

    #region Datatable Server-Side
    public async Task<DatatableResponseServerSide<TEntity>> GetDatatableServerSideAsync(DatatableRequest datatableRequest, Filter? filter = null, IEnumerable<Sort>? sorts = null, Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, CancellationToken cancellationToken = default)
    {
        if (datatableRequest == null) throw new ArgumentNullException(nameof(datatableRequest));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return await query.ToDatatableServerSideAsync(datatableRequest, cancellationToken);
    }
    public async Task<DatatableResponseServerSide<TResult>> GetDatatableServerSideAsync<TResult>(DatatableRequest datatableRequest, Expression<Func<TEntity, TResult>> select, Filter? filter = null, IEnumerable<Sort>? sorts = null, Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, CancellationToken cancellationToken = default)
    {
        if (datatableRequest == null) throw new ArgumentNullException(nameof(datatableRequest));
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return await query.Select(select).ToDatatableServerSideAsync(datatableRequest, cancellationToken);
    }
    #endregion

    #region Datatable Client-Side
    public async Task<DatatableResponseClientSide<TEntity>> GetDatatableClientSideAsync(Filter? filter = null, IEnumerable<Sort>? sorts = null, Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return await query.ToDatatableClientSideAsync(cancellationToken);
    }
    public async Task<DatatableResponseClientSide<TResult>> GetDatatableClientSideAsync<TResult>(Expression<Func<TEntity, TResult>> select, Filter? filter = null, IEnumerable<Sort>? sorts = null, Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, CancellationToken cancellationToken = default)
    {
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return await query.Select(select).ToDatatableClientSideAsync(cancellationToken);
    }
    #endregion

    #region Pagination
    public async Task<PaginationResponse<TEntity>> GetPaginationAsync(PaginationRequest paginationRequest, Filter? filter = null, IEnumerable<Sort>? sorts = null, Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, CancellationToken cancellationToken = default)
    {
        if (paginationRequest == null) throw new ArgumentNullException(nameof(paginationRequest));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return await query.ToPaginateAsync(paginationRequest, cancellationToken);
    }
    public async Task<PaginationResponse<TResult>> GetPaginationAsync<TResult>(PaginationRequest paginationRequest, Expression<Func<TEntity, TResult>> select, Filter? filter = null, IEnumerable<Sort>? sorts = null, Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, CancellationToken cancellationToken = default)
    {
        if (paginationRequest == null) throw new ArgumentNullException(nameof(paginationRequest));
        if (select == null) throw new ArgumentNullException(nameof(select));

        var query = _context.Set<TEntity>().AsQueryable();

        if (where != null) query = query.Where(where);
        if (filter != null) query = query.ToFilter(filter);
        if (orderBy != null) query = orderBy(query);
        if (sorts != null) query = query.ToSort(sorts);
        if (include != null) query = include(query);
        if (withDeleted) query = query.IgnoreQueryFilters();
        query = query.AsNoTracking();

        return await query.Select(select).ToPaginateAsync(paginationRequest, cancellationToken);
    }
    #endregion
}
