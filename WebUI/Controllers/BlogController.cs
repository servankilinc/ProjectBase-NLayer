using Business.Abstract;
using Core.BaseRequestModels;
using Core.Utils.Datatable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Blog_;
using WebUI.Models.ViewModels.Blog;
using WebUI.Utils.ActionFilters;

namespace WebUI.Controllers;


[Authorize]
public class BlogController : Controller
{
    private readonly IBlogService _blogService;
    private readonly ICategoryService _categoryService;
    private readonly IUserService _userService;
    public BlogController(IBlogService blogService, ICategoryService categoryService, IUserService userService)
    {
        _blogService = blogService;
        _categoryService = categoryService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = new BlogViewModel
        {
            AuthorIds = await _userService.GetSelectListAsync(),
            CategoryIds = await _categoryService.GetSelectListAsync()
        };
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new BlogCreateViewModel
        {
            AuthorIds = await _userService.GetSelectListAsync(),
            CategoryIds = await _categoryService.GetSelectListAsync()
        };
        return View(viewModel);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<BlogCreateDto>))]
    public async Task<IActionResult> Create(BlogCreateDto createModel)
    {
        var result = await _blogService.CreateAsync(createModel);
        return Ok(result);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<BlogUpdateDto>))]
    public async Task<IActionResult> Update(BlogUpdateDto updateModel)
    {
        var result = await _blogService.UpdateAsync(updateModel);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _blogService.DeleteAsync(id);
        return Ok();
    }

    #region Datatable
    [HttpPost]
    public async Task<DatatableResponseServerSide<BlogReportDto>> DatatableServerSide(DynamicDatatableServerSideRequest request)
    {
        var result = await _blogService.DatatableServerSideByReportAsync(request);
        return result;
    }
    #endregion

    #region Form Partials
    [HttpGet]
    public async Task<IActionResult> CreateForm()
    {
        var viewModel = new BlogCreateViewModel
        {
            AuthorIds = await _userService.GetSelectListAsync(),
            CategoryIds = await _categoryService.GetSelectListAsync()
        };
        return PartialView("./Partials/CreateForm", viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateForm(Guid id)
    {
        var data = await _blogService.GetAsync<BlogUpdateDto>(id);

        if (data == null) return NotFound(data);

        var viewModel = new BlogUpdateViewModel
        {
            UpdateModel = data,
            AuthorIds = await _userService.GetSelectListAsync(),
            CategoryIds = await _categoryService.GetSelectListAsync()
        };
        return PartialView("./Partials/UpdateForm", viewModel);
    }
    #endregion
}