using System.ComponentModel;

namespace Core.Enums;

public enum RoleTypes
{
    [Description("User")]
    User = 1,
    [Description("Manager")]
    Manager = 2,
    [Description("Admin")]
    Admin = 3,
    [Description("Owner")]
    Owner = 4,
}