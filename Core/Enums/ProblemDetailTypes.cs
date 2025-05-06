using System.ComponentModel;

namespace Core.Enums;

public enum ProblemDetailTypes
{
    [Description("Unknown Error")]
    General = 1,
    [Description("Validation Error")]
    Validation = 2,
    [Description("Business Logic")]
    Business = 3,
    [Description("Data Access")]
    DataAccess = 4,
}
