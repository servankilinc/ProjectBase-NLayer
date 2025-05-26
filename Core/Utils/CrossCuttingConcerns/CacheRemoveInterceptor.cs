using Castle.DynamicProxy;
using Core.Utils.Caching;
using Core.Utils.CrossCuttingConcerns.Helpers;
using System.Reflection;

namespace Core.Utils.CrossCuttingConcerns;

public class CacheRemoveInterceptor : IInterceptor
{
    private readonly ICacheService _cacheService;
    public CacheRemoveInterceptor(ICacheService cacheService) => _cacheService = cacheService;

    public void Intercept(IInvocation invocation)
    {
        if (invocation.HasAttribute<CacheRemoveAttribute>())
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

        var attribute = invocation.GetAttribute<CacheRemoveAttribute>();
        if (attribute == null) throw new InvalidOperationException("CacheRemoveAttribute not found.");

        if (!methodInfo.IsAsync())
        {
            InterceptSync(invocation, attribute.CacheKey);
        }
        else
        {
            if (!methodInfo.IsGenericAsync())
            {
                invocation.ReturnValue = InterceptAsync(invocation, attribute.CacheKey);
            }
            else
            {
                var returnType = methodInfo.ReturnType.GetGenericArguments()[0];

                var method = GetType().GetMethod(nameof(InterceptAsyncGeneric), BindingFlags.NonPublic | BindingFlags.Instance)?.MakeGenericMethod(returnType);
                if (method == null) throw new InvalidOperationException("InterceptAsyncGeneric could not be resolved.");

                invocation.ReturnValue = method.Invoke(this, new object[] { invocation, attribute.CacheKey });
            }
        }
    }

    private void InterceptSync(IInvocation invocation, string cacheKey)
    {
        _cacheService.RemoveFromCache(cacheKey);
        invocation.Proceed();
    }

    private async Task InterceptAsync(IInvocation invocation, string cacheKey)
    {
        _cacheService.RemoveFromCache(cacheKey);
        invocation.Proceed();
        var task = (Task)invocation.ReturnValue;
        await task.ConfigureAwait(false);
    }

    private async Task<TResult> InterceptAsyncGeneric<TResult>(IInvocation invocation, string cacheKey)
    {
        _cacheService.RemoveFromCache(cacheKey);
        invocation.Proceed();
        var task = (Task<TResult>)invocation.ReturnValue;
        return await task.ConfigureAwait(false);
    }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
public class CacheRemoveAttribute : Attribute
{
    public string CacheKey { get; }
    public CacheRemoveAttribute(string cacheKey) => CacheKey = cacheKey;
}