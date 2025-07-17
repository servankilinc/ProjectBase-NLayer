using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.Models.ViewModels.Blog
{
    public class BlogViewModel
    {
        public SelectList? CategoryIds { get; set; }
        public SelectList? AuthorIds { get; set; }
        public BlogFilterModel FilterModel { get; set; } = new BlogFilterModel();
    }

    public class BlogFilterModel
    {
        // *** bu alanlar entity içindeki filterable fieldlardan getirilir ***
        public Guid AuthorId { get; set; }
        public string? Title { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime Date { get; set; }

        // Soft Deletable ise
        public bool IsDeleted { get; set; }
    }
}
