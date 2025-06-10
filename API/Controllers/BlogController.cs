using Business.Abstract;
using Core.BaseRequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Blog_;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;
    public BlogController(IBlogService blogService) => _blogService = blogService;

    #region GetBasic
    [HttpGet]
    public async Task<IActionResult> Get(Guid Id)
    {
        var result = await _blogService.GetAsync(Id);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetAll(DynamicRequest? request)
    {
        var result = await _blogService.GetAllAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetList(DynamicPaginationRequest request)
    {
        var result = await _blogService.GetListAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }
    #endregion

    #region GetDetail
    [HttpGet]
    public async Task<IActionResult> GetByDetail(Guid Id)
    {
        var result = await _blogService.GetByDetailAsync(Id);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetAllByDetail(DynamicRequest? request)
    {
        var result = await _blogService.GetAllByDetailAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetListByDetail(DynamicPaginationRequest request)
    {
        var result = await _blogService.GetListByDetailAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }
    #endregion

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create(BlogCreateDto request)
    {
        var result = await _blogService.CreateAsync(request);

        return Ok(result);
    }
    #endregion

    #region Update
    [HttpPatch]
    public async Task<IActionResult> Update(BlogUpdateDto request)
    {
        var result = await _blogService.UpdateAsync(request);

        return Ok(result);
    }
    #endregion

    #region Delete
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid Id)
    {
        await _blogService.DeleteAsync(Id);

        return Ok();
    }
    #endregion

    #region Datatable Methods
    [HttpPost]
    public async Task<IActionResult> DatatableClientSide(DynamicRequest request)
    {
        var result = await _blogService.DatatableClientSideAsync(request);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableServerSideRequest request)
    {
        var result = await _blogService.DatatableServerSideAsync(request);

        return Ok(result);
    }
    #endregion
}
