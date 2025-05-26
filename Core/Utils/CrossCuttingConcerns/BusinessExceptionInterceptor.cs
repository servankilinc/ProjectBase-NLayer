using Castle.DynamicProxy;
using Core.Utils.CrossCuttingConcerns.Helpers;
using Core.Utils.ExceptionHandle.Exceptions;
using System.Reflection;

namespace Core.Utils.CrossCuttingConcerns;

public class BusinessExceptionInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        if (invocation.HasAttribute<BusinessExceptionAttribute>())
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

        if (!methodInfo.IsAsync())
        {
            InterceptSync(invocation);
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

                invocation.ReturnValue = method.Invoke(this, new object[] { invocation });
            }
        }
    }

    private void InterceptSync(IInvocation invocation)
    {
        try
        {
            invocation.Proceed();
        }
        catch (Exception exception)
        {
            throw HandleException(exception);
        }
    }

    private async Task InterceptAsync(IInvocation invocation)
    {
        try
        {
            invocation.Proceed();
            var task = (Task)invocation.ReturnValue;
            await task.ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            throw HandleException(exception);
        }
    }

    private async Task<TResult> InterceptAsyncGeneric<TResult>(IInvocation invocation)
    {
        try
        {
            invocation.Proceed();
            var task = (Task<TResult>)invocation.ReturnValue;
            return await task.ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            throw HandleException(exception);
        }
    }

    private BusinessException HandleException(Exception exception)
    {
        if (exception.InnerException != null)
            return new BusinessException($"{exception.Message} \n Detail: {exception.InnerException.Message})");
        return new BusinessException(exception.Message);
    }
}


[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
public class BusinessExceptionAttribute : Attribute
{
}