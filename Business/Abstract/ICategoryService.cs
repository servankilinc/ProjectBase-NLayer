using Business.ServiceBase;
using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Core.Utils.Pagination;
using Model.Dtos.Category_;
using Model.Entities;

namespace Business.Abstract;

public interface ICategoryService : IServiceBase<Category>, IServiceBaseAsync<Category>
{
    #region GetBasic
    Task<CategoryResponseDto?> GetAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<CategoryResponseDto>?> GetAllAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<CategoryResponseDto>> GetListAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region GetDetail
    Task<CategoryBlogsResponseDto?> GetByDetailAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<ICollection<CategoryBlogsResponseDto>?> GetAllByDetailAsync(DynamicRequest? request, CancellationToken cancellationToken = default);
    Task<PaginationResponse<CategoryBlogsResponseDto>> GetListByDetailAsync(DynamicPaginationRequest request, CancellationToken cancellationToken = default);
    #endregion

    #region Create
    Task<CategoryResponseDto> CreateAsync(CategoryCreateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Update
    Task<CategoryResponseDto> UpdateAsync(CategoryUpdateDto request, CancellationToken cancellationToken = default);
    #endregion

    #region Delete
    Task DeleteAsync(Guid Id, CancellationToken cancellationToken = default);
    #endregion

    #region Datatable Methods
    Task<DatatableResponseClientSide<Category>> DatatableClientSideAsync(DynamicRequest request, CancellationToken cancellationToken = default);
    Task<DatatableResponseServerSide<Category>> DatatableServerSideAsync(DynamicDatatableServerSideRequest request, CancellationToken cancellationToken = default);
    #endregion
}
