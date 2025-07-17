namespace WebUI.Models.ViewModels.Category
{
    public class CategoryViewModel
    {
        public CategoryFilterModel FilterModel { get; set; } = new CategoryFilterModel();
    }

    public class CategoryFilterModel
    {
        // *** bu alanlar entity içindeki filterable fieldlardan getirilir ***
        public string? Name { get; set; }

        // Soft Deletable ise
        public bool IsDeleted { get; set; }
    }
}
