using Castle.DynamicProxy;
using Newtonsoft.Json;
using System.Reflection;

namespace Core.Utils.CrossCuttingConcerns.Helpers;

public static class Extensions
{
    public static TAttribute? GetAttribute<TAttribute>(this IInvocation invocation) where TAttribute : Attribute
    {
        var methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;

        return methodInfo.GetCustomAttributes<TAttribute>(true).FirstOrDefault();
    }

    public static bool HasAttribute<TAttribute>(this IInvocation invocation) where TAttribute : Attribute
    {
        var methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;

        var methodAttribute = methodInfo.GetCustomAttributes<TAttribute>(true);
        var classAttribute = invocation.TargetType.GetCustomAttributes<TAttribute>(true);

        return methodAttribute.Any() || classAttribute.Any();
    }

    public static bool IsAsync(this MethodInfo methodInfo)
    {
        return typeof(Task).IsAssignableFrom(methodInfo.ReturnType);
    }

    public static bool IsGenericAsync(this MethodInfo methodInfo)
    {
        var isWithResult =
            methodInfo.ReturnType.IsGenericType &&
            methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>) &&
            methodInfo.ReturnType.GetGenericArguments().Any();

        return isWithResult;
    }

    public static bool IsVoid(this MethodInfo methodInfo)
    {
        return methodInfo.ReturnType == typeof(void);
    }

    public static string GetLocation(this IInvocation invocation)
    {
        var methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;

        var className = methodInfo.DeclaringType?.FullName ?? "<UnknownClass>";
        var methodName = methodInfo.Name;
        return $"{className}.{methodName}";
    }

    public static string GetParameters(this IInvocation invocation)
    {
        var methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;

        var parameters = methodInfo.GetParameters();
        var arguments = invocation.Arguments;

        var paramsByArgs = parameters.Select((p, i) =>
        {
            var arg = arguments[i];
            if (arg == null) return "null";

            Type type = arg.GetType();
            if (type.IsSimpleType()) return arg.ToString();

            string serialized = JsonConvert.SerializeObject(arg, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = 7,
            });
            return $"Parameter Detail: {p.Name} = {serialized}";
        }).ToArray();

        return string.Join(", \n", paramsByArgs);
    }

    public static bool IsSimpleType(this Type type)
    {
        if (type.IsPrimitive || type.IsEnum) return true;

        Type[] simpleTypes =
        [
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(Uri),
            typeof(DateOnly),
            typeof(TimeOnly)
        ];

        return simpleTypes.Contains(type);
    }
}
