using Model.Dtos.User_;

namespace WebUI.Models.ViewModels.User
{
    public class UserCreateViewModel
    {
        // create dto varsa o kullanılır yoksa entity kullan formu oluştururken listeleri dahil etmezsin
        public UserCreateDto CreateModel { get; set; } = new UserCreateDto();
    }
}
