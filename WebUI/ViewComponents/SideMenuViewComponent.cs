using Microsoft.AspNetCore.Mvc;
using WebUI.Models.UI;

namespace WebUI.ViewComponents
{
    public class SideMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var menuItems = new List<MenuItem>() 
            {
                new MenuItem
                {
                    Title = "Dashboard",
                    Icon = "fa-brands fa-magento",
                    Path = "/Home/Index",
                    Type = 1,
                },
                new MenuItem
                {
                    Title = "Blog",
                    Icon = "fa-regular fa-folder-open",
                    Type = 0,
                    GroupName = "Pages",
                    SubMenuItems = new List<MenuItem>()
                    {
                        new MenuItem
                        {
                            Title = "Managment",
                            Icon = "fa-regular fa-file-lines",
                            Path = "/Blog/Index",
                            Type = 1,
                        },
                        new MenuItem
                        {
                            Title = "Create",
                            Icon = "fa-solid fa-file-circle-plus",
                            Path= "/Blog/Create",
                            Type = 1,
                        }
                    }
                },
                new MenuItem
                {
                    Title = "User",
                    Icon = "fa-regular fa-folder-open",
                    Type = 0,
                    SubMenuItems = new List<MenuItem>()
                    {
                        new MenuItem
                        {
                            Title = "Managment",
                            Icon = "fa-regular fa-file-lines",
                            Path = "/User/Index",
                            Type = 1,
                        },
                        new MenuItem
                        {
                            Title = "Create",
                            Icon = "fa-solid fa-file-circle-plus",
                            Path= "/User/Create",
                            Type = 1,
                        }
                    }
                },
                new MenuItem
                {
                    Title = "Category",
                    Icon = "fa-regular fa-folder-open",
                    Type = 0,
                    SubMenuItems = new List<MenuItem>()
                    {
                        new MenuItem
                        {
                            Title = "Managment",
                            Icon = "fa-regular fa-file-lines",
                            Path = "/Category/Index",
                            Type = 1,
                        },
                        new MenuItem
                        {
                            Title = "Create",
                            Icon = "fa-solid fa-file-circle-plus",
                            Path= "/Category/Create",
                            Type = 1,
                        }
                    }
                },
                new MenuItem
                {
                    Title = "BlogComment",
                    Icon = "fa-regular fa-folder-open",
                    Type = 0,
                    SubMenuItems = new List<MenuItem>()
                    {
                        new MenuItem
                        {
                            Title = "Managment",
                            Icon = "fa-regular fa-file-lines",
                            Path = "/BlogComment/Index",
                            Type = 1,
                        },
                        new MenuItem
                        {
                            Title = "Create",
                            Icon = "fa-solid fa-file-circle-plus",
                            Path= "/BlogComment/Create",
                            Type = 1,
                        }
                    }
                },
            };
            return View(menuItems);
        }
    }
}
