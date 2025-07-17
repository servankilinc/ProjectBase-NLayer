using Business.Abstract;
using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Blog_;
using Model.Dtos.BlogComment_;
using WebUI.Models.ViewModels.BlogComment;
using WebUI.Utils.ActionFilters;

namespace WebUI.Controllers;

public class BlogCommentController : Controller
{
    private readonly IBlogService _blogService;
    private readonly IBlogCommentService _blogCommentService;
    private readonly IUserService _userService;
    public BlogCommentController(IBlogService blogService, IBlogCommentService blogCommentService, IUserService userService)
    {
        _blogService = blogService;
        _blogCommentService = blogCommentService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = new BlogCommentViewModel
        {
            UserIds = await _userService.GetSelectListAsync(),
            BlogIds = await _blogService.GetSelectListAsync()
        };
        return View(viewModel);
    }
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new BlogCommentCreateViewModel
        {
            UserIds = await _userService.GetSelectListAsync(),
            BlogIds = await _blogService.GetSelectListAsync()
        };
        return View(viewModel);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<BlogCommentCreateDto>))]
    public async Task<IActionResult> Create(BlogCommentCreateDto createModel)
    {
        var result = await _blogCommentService.CreateAsync(createModel);
        return Ok(result);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<BlogCommentUpdateDto>))]
    public async Task<IActionResult> Update(BlogCommentUpdateDto updateModel)
    {
        var result = await _blogCommentService.UpdateAsync(updateModel);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _blogCommentService.DeleteAsync(id);
        return Ok();
    }

    #region Datatable
    [HttpPost]
    public async Task<DatatableResponseServerSide<BlogCommentReportDto>> DatatableServerSide(DynamicDatatableServerSideRequest request)
    {
        var result = await _blogCommentService.DatatableServerSideByReportAsync(request);
        return result;
    }
    #endregion

    #region Forms Partials
    [HttpGet]
    public async Task<IActionResult> CreateForm()
    {
        var viewModel = new BlogCommentCreateViewModel
        {
            UserIds = await _userService.GetSelectListAsync(),
            BlogIds = await _blogService.GetSelectListAsync()
        };
        return PartialView("./Partials/CreateForm", viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateForm(Guid id)
    {
        var data = await _blogCommentService.GetAsync<BlogCommentUpdateDto>(id);

        if (data == null) return NotFound(data);

        var viewModel = new BlogCommentUpdateViewModel
        {
            UpdateModel = data,
            UserIds = await _userService.GetSelectListAsync(),
            BlogIds = await _blogService.GetSelectListAsync()
        };
        return PartialView("./Partials/UpdateForm", viewModel);
    }
    #endregion
}
