using AutoMapper;
using Business.Abstract;
using Business.ServiceBase;
using DataAccess.Abstract;
using Model.Entities;

namespace Business.Concrete;

public class CategoryService : ServiceBase<Category, ICategoryRepository>, ICategoryService
{
    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper) : base(categoryRepository, mapper)
    {
    }
}
