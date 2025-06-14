using Business.Abstract;
using Core.BaseRequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Category_;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService) => _categoryService = categoryService;

    #region GetBasic
    [HttpGet("Get")]
    public async Task<IActionResult> Get(Guid Id)
    {
        var result = await _categoryService.GetAsync(Id);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAll(DynamicRequest? request)
    {
        var result = await _categoryService.GetAllAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetList")]
    public async Task<IActionResult> GetList(DynamicPaginationRequest request)
    {
        var result = await _categoryService.GetListAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }
    #endregion

    #region GetDetail
    [HttpGet("GetByDetail")]
    public async Task<IActionResult> GetByDetail(Guid Id)
    {
        var result = await _categoryService.GetByDetailAsync(Id);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetAllByDetail")]
    public async Task<IActionResult> GetAllByDetail(DynamicRequest? request)
    {
        var result = await _categoryService.GetAllByDetailAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("GetListByDetail")]
    public async Task<IActionResult> GetListByDetail(DynamicPaginationRequest request)
    {
        var result = await _categoryService.GetListByDetailAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }
    #endregion

    #region Create
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CategoryCreateDto request)
    {
        var result = await _categoryService.CreateAsync(request);

        return Ok(result);
    }
    #endregion

    #region Update
    [HttpPatch("Update")]
    public async Task<IActionResult> Update(CategoryUpdateDto request)
    {
        var result = await _categoryService.UpdateAsync(request);

        return Ok(result);
    }
    #endregion

    #region Delete
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(Guid Id)
    {
        if (Id == default) return BadRequest("Id parameter cannot be empty.");

        await _categoryService.DeleteAsync(Id);

        return Ok();
    }
    #endregion

    #region Datatable Methods
    [HttpPost("DatatableClientSide")]
    public async Task<IActionResult> DatatableClientSide(DynamicRequest request)
    {
        var result = await _categoryService.DatatableClientSideAsync(request);

        return Ok(result);
    }

    [HttpPost("DatatableServerSide")]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableServerSideRequest request)
    {
        var result = await _categoryService.DatatableServerSideAsync(request);

        return Ok(result);
    }
    #endregion
}
