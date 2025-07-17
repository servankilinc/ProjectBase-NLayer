using WebUI.Models;

namespace WebUI.Models.UI;

public class MenuItem
{
    public string Title { get; set; } = "Menu Item";
    public int Type { get; set; } // 0 = group, 1 = route
    public string? Path { get; set; }
    public string Icon { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public string? Description { get; set; }

    public List<MenuItem>? SubMenuItems { get; set; }
}
