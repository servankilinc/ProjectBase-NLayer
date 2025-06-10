using System.ComponentModel;

namespace Core.Enums;

public enum RoleTypes
{
    [Description("User")]
    User = 0,
    [Description("Manager")]
    Manager = 1,
    [Description("Admin")]
    Admin = 2,
    [Description("Owner")]
    Owner = 3,
}