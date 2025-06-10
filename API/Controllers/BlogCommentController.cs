using Business.Abstract;
using Core.BaseRequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.BlogComment_;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class BlogCommentController : ControllerBase
{
    private readonly IBlogCommentService _blogCommentService;
    public BlogCommentController(IBlogCommentService blogCommentService) => _blogCommentService = blogCommentService;

    #region GetBasic
    [HttpGet]
    public async Task<IActionResult> Get(Guid Id)
    {
        var result = await _blogCommentService.GetAsync(Id);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetAll(DynamicRequest? request)
    {
        var result = await _blogCommentService.GetAllAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetList(DynamicPaginationRequest request)
    {
        var result = await _blogCommentService.GetListAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }
    #endregion

    #region GetDetail
    [HttpGet]
    public async Task<IActionResult> GetByDetail(Guid Id)
    {
        var result = await _blogCommentService.GetByDetailAsync(Id);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetAllByDetail(DynamicRequest? request)
    {
        var result = await _blogCommentService.GetAllByDetailAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetListByDetail(DynamicPaginationRequest request)
    {
        var result = await _blogCommentService.GetListByDetailAsync(request);

        if (result == null) return NotFound();

        return Ok(result);
    }
    #endregion

    #region Create
    [HttpPost]
    public async Task<IActionResult> Create(BlogCommentCreateDto request)
    {
        var result = await _blogCommentService.CreateAsync(request);

        return Ok(result);
    }
    #endregion

    #region Update
    [HttpPatch]
    public async Task<IActionResult> Update(BlogCommentUpdateDto request)
    {
        var result = await _blogCommentService.UpdateAsync(request);

        return Ok(result);
    }
    #endregion

    #region Delete
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid Id)
    {
        await _blogCommentService.DeleteAsync(Id);

        return Ok();
    }
    #endregion

    #region Datatable Methods
    [HttpPost]
    public async Task<IActionResult> DatatableClientSide(DynamicRequest request)
    {
        var result = await _blogCommentService.DatatableClientSideAsync(request);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> DatatableServerSide(DynamicDatatableServerSideRequest request)
    {
        var result = await _blogCommentService.DatatableServerSideAsync(request);

        return Ok(result);
    }
    #endregion
}
