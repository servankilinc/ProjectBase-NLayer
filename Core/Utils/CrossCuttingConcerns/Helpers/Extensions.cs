using Castle.DynamicProxy;
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
}
