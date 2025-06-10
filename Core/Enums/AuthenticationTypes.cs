using System.ComponentModel;

namespace Core.Enums;

public enum AuthenticationTypes
{
    [Description("None")]
    None = 0,
    [Description("Email")]
    Email = 1,
    [Description("Google")]
    Google = 2,
    [Description("Facebook")]
    Facebook = 3,
}
