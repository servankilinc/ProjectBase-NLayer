using Castle.DynamicProxy;
using Core.Utils.Caching;
using Core.Utils.CrossCuttingConcerns.Helpers;
using System.Reflection;

namespace Core.Utils.CrossCuttingConcerns;

public class CacheRemoveGroupInterceptor : IInterceptor
{
    private readonly ICacheService _cacheService;
    public CacheRemoveGroupInterceptor(ICacheService cacheService) => _cacheService = cacheService;

    public void Intercept(IInvocation invocation)
    {
        if (invocation.HasAttribute<CacheRemoveGroupAttribute>())
        {
            HandleIntercept(invocation);
        }
        else
        {
            invocation.Proceed();
        }
    }

    private void HandleIntercept(IInvocation invocation)
    {
        var methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;

        var attribute = invocation.GetAttribute<CacheRemoveGroupAttribute>();
        if (attribute == null) throw new InvalidOperationException("CacheRemoveGroupAttribute not found.");

        if (!methodInfo.IsAsync())
        {
            InterceptSync(invocation, attribute.CacheGroupKeys);
        }
        else
        {
            if (!methodInfo.IsGenericAsync())
            {
                invocation.ReturnValue = InterceptAsync(invocation, attribute.CacheGroupKeys);
            }
            else
            {
                var returnType = methodInfo.ReturnType.GetGenericArguments()[0];

                var method = GetType().GetMethod(nameof(InterceptAsyncGeneric), BindingFlags.NonPublic | BindingFlags.Instance)?.MakeGenericMethod(returnType);
                if (method == null) throw new InvalidOperationException("InterceptAsyncGeneric could not be resolved.");

                invocation.ReturnValue = method.Invoke(this, new object[] { invocation, attribute.CacheGroupKeys });
            }
        }
    }

    private void InterceptSync(IInvocation invocation, string[] cacheGroupKeys)
    {
        _cacheService.RemoveCacheGroupKeys(cacheGroupKeys);
        invocation.Proceed();
    }

    private async Task InterceptAsync(IInvocation invocation, string[] cacheGroupKeys)
    {
        _cacheService.RemoveCacheGroupKeys(cacheGroupKeys);
        invocation.Proceed();
        var task = (Task)invocation.ReturnValue;
        await task.ConfigureAwait(false);
    }

    private async Task<TResult> InterceptAsyncGeneric<TResult>(IInvocation invocation, string[] cacheGroupKeys)
    {
        _cacheService.RemoveCacheGroupKeys(cacheGroupKeys);
        invocation.Proceed();
        var task = (Task<TResult>)invocation.ReturnValue;
        return await task.ConfigureAwait(false);
    }
}


[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
public class CacheRemoveGroupAttribute : Attribute
{
    public string[] CacheGroupKeys { get; }
    public CacheRemoveGroupAttribute(string[] cacheGroupKeys) => CacheGroupKeys = cacheGroupKeys;
}