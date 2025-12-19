using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Core.Utils.DynamicQuery;

public static class QueryableFilterExtension
{
    private static readonly string[] _logics = { "and", "or" };
    private static readonly IDictionary<string, string> _operators = new Dictionary<string, string>
    {
        { "base", " " },
        { "eq", "=" },
        { "neq", "!=" },
        { "lt", "<" },
        { "lte", "<=" },
        { "gt", ">" },
        { "gte", ">=" },
        { "isnull", "== null" },
        { "isnotnull", "!= null" },
        { "startswith", "StartsWith" },
        { "endswith", "EndsWith" },
        { "contains", "Contains" },
        { "doesnotcontain", "Contains" }
    };

    public static IQueryable<T> ToFilter<T>(this IQueryable<T> queryable, Filter filter)
    {
        List<Filter> filterList = new();
        GetFilters(filterList, filter);

        foreach (Filter item in filterList) // Validation before processes
        {
            if (item.Operator == "base" && _logics.Contains(item.Logic)) continue;
            if (string.IsNullOrEmpty(item.Field))
                throw new ArgumentException("Empty Field For Filter Process");
            if (string.IsNullOrEmpty(item.Operator) || !_operators.ContainsKey(item.Operator))
                throw new ArgumentException("Invalid Opreator Type For Filter Process");
            if (string.IsNullOrEmpty(item.Value) && (item.Operator == "isnull" || item.Operator == "isnotnull")) // those operators do not need value
                throw new ArgumentException("Invalid Value For Filter Process");
            if (string.IsNullOrEmpty(item.Logic) == false && _logics.Contains(item.Logic) == false)
                throw new ArgumentException("Invalid Logic Type For Filter Process");
        }

        string?[] values = filterList.Where(f => f.Value != null).Select(f => f.Value).ToArray();
        string where = Transform(filter, filterList);

        if (!string.IsNullOrWhiteSpace(where))
            queryable = queryable.Where(where, values);
        return queryable;
    }

    private static void GetFilters(IList<Filter> filterList, Filter filter)
    {
        if (filter.Operator != "base" && string.IsNullOrEmpty(filter.Value)) return;

        filterList.Add(filter);
        if (filter.Filters is not null && filter.Filters.Any())
            foreach (Filter item in filter.Filters)
                GetFilters(filterList, item);
    }

    public static string Transform(Filter filter, IList<Filter> filters)
    {
        var tempList = filters.Where(f => f.Value != null).ToList();
        int index = tempList.IndexOf(filter);
        string comparison = _operators[filter.Operator!];
        StringBuilder where = new();

        switch (filter.Operator)
        {
            case "base":
                where.Append($" ");
                break;
            case "eq":
                where.Append($"np({filter.Field}) == @{index}");
                break;
            case "neq":
                where.Append($"np({filter.Field}) != @{index}");
                break;
            case "lt":
                where.Append($"np({filter.Field}) < @{index}");
                break;
            case "lte":
                where.Append($"np({filter.Field}) <= @{index}");
                break;
            case "gt":
                where.Append($"np({filter.Field}) > @{index}");
                break;
            case "gte":
                where.Append($"np({filter.Field}) >= @{index}");
                break;
            case "isnull":
                where.Append($"np({filter.Field}) == null");
                break;
            case "isnotnull":
                where.Append($"np({filter.Field}) != null");
                break;
            case "startswith":
                where.Append($"np({filter.Field}).StartsWith(@{index})");
                break;
            case "endswith":
                where.Append($"np({filter.Field}).EndsWith(@{index})");
                break;
            case "contains":
                where.Append($"np({filter.Field}).Contains(@{index})");
                break;
            case "doesnotcontain":
                where.Append($"!np({filter.Field}).Contains(@{index})");
                break;
            default:
                throw new ArgumentException($"Invalid Operator Type For Filter Process ({filter.Operator})");
        }



        if (filter.Logic is not null && filter.Filters is not null && filter.Filters.Any())
        {
            string baseLogic = filter.Operator == "base" ? "" : filter.Logic;
            if (filter.Operator == "base" && !filter.Filters.Any(f => f.Value != null)) return "";

            return $"({where} {baseLogic} {string.Join(separator: $" {filter.Logic} ", value: filter.Filters.Where(f => f.Operator == "base" || !string.IsNullOrEmpty(f.Value)).Select(f => Transform(f, filters)).ToArray())})";
        }

        return where.ToString();
    }
}