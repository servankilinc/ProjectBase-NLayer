namespace WebUI.Models.ViewModels.User
{
    public class UserViewModel
    {
        public UserFilterModel FilterModel { get; set; } = new UserFilterModel();
    }

    public class UserFilterModel
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        // Soft Deletable ise
        public bool IsDeleted { get; set; }
    }
}
