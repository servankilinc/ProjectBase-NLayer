using Castle.DynamicProxy;
using Core.Utils.Caching;
using Core.Utils.CrossCuttingConcerns.Helpers;
using Newtonsoft.Json;
using System.Reflection;

namespace Core.Utils.CrossCuttingConcerns;

public class CacheInterceptor : IInterceptor
{
    private readonly ICacheService _cacheService;
    public CacheInterceptor(ICacheService cacheService) => _cacheService = cacheService;

    public void Intercept(IInvocation invocation)
    {
        if (invocation.HasAttribute<CacheAttribute>())
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

        var attribute = invocation.GetAttribute<CacheAttribute>();
        if (attribute == null) throw new InvalidOperationException("CacheAttribute not found.");

        if (!methodInfo.IsAsync())
        {
            if (methodInfo.IsVoid())
            {
                InterceptSyncVoid(invocation);
            }
            else
            {
                InterceptSync(invocation, methodInfo, attribute);
            }
        }
        else
        {
            if (!methodInfo.IsGenericAsync())
            {
                invocation.ReturnValue = InterceptAsync(invocation);
            }
            else
            {
                var returnType = methodInfo.ReturnType.GetGenericArguments()[0];

                var method = GetType().GetMethod(nameof(InterceptAsyncGeneric), BindingFlags.NonPublic | BindingFlags.Instance)?.MakeGenericMethod(returnType);
                if (method == null) throw new InvalidOperationException("InterceptAsyncGeneric could not be resolved.");

                invocation.ReturnValue = method.Invoke(this, new object[] { invocation, attribute });
            }
        }
    }

    private void InterceptSyncVoid(IInvocation invocation)
    {
        invocation.Proceed();
    }

    private void InterceptSync(IInvocation invocation, MethodInfo methodInfo, CacheAttribute attribute)
    {
        var cacheKey = GenerateCacheKey(attribute.CacheKey, invocation.Arguments);
        var resultCache = _cacheService.GetFromCache(cacheKey);
        if (resultCache.IsSuccess)
        {
            var source = JsonConvert.DeserializeObject(resultCache.Source!, methodInfo.ReturnType);
            if (source != null)
            {
                invocation.ReturnValue = source;
                return;
            }
        }

        invocation.Proceed();

        if (invocation.ReturnValue != null)
        {
            _cacheService.AddToCache(cacheKey, attribute.CacheGroupKeys, invocation.ReturnValue);
        }
    }

    private async Task InterceptAsync(IInvocation invocation)
    {
        invocation.Proceed();
        var task = (Task)invocation.ReturnValue;
        await task.ConfigureAwait(false);
    }

    private async Task<TResult> InterceptAsyncGeneric<TResult>(IInvocation invocation, CacheAttribute attribute)
    {
        var cacheKey = GenerateCacheKey(attribute.CacheKey, invocation.Arguments);
        var resultCache = _cacheService.GetFromCache(cacheKey);
        if (resultCache.IsSuccess)
        {
            var source = JsonConvert.DeserializeObject<TResult>(resultCache.Source!);
            if (source != null) return source;
        }

        invocation.Proceed();
        var task = (Task<TResult>)invocation.ReturnValue;
        var result = await task.ConfigureAwait(false);

        if (result != null)
        {
            _cacheService.AddToCache(cacheKey, attribute.CacheGroupKeys, result);
        }
        return result;
    }

    private string GenerateCacheKey(string cacheKey, object[] args)
    {
        var filteredArgs = args.Where(a => a is not CancellationToken && a is not null).ToArray();
         
        if (filteredArgs.Length > 0)
        {
            var serializedArgs = JsonConvert.SerializeObject(filteredArgs, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.None
            });
            return $"{cacheKey}-{serializedArgs}";
        }
        return $"{cacheKey}";
    }
}


[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
public class CacheAttribute : Attribute
{
    public string CacheKey { get; }
    public string[] CacheGroupKeys { get; }
    public CacheAttribute(string cacheKey, string[] cacheGroupKeys)
    {
        CacheKey = cacheKey;
        CacheGroupKeys = cacheGroupKeys;
    }
}