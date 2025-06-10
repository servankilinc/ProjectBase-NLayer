using Castle.DynamicProxy;
using Core.Model;
using Core.Utils.CrossCuttingConcerns.Helpers;
using Core.Utils.ExceptionHandle.Exceptions;
using System.Reflection;

namespace Core.Utils.CrossCuttingConcerns;

public class ExceptionHandlerInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        if (invocation.HasAttribute<ExceptionHandlerAttribute>())
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
            throw HandleException(exception, invocation);
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
            throw HandleException(exception, invocation);
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
            throw HandleException(exception, invocation);
        }
    }

    private Exception HandleException(Exception exception, IInvocation invocation)
    {
        if (exception is IAppException appException)
        {
            appException.LocationName ??= invocation.GetLocation();
            appException.Parameters ??= invocation.GetParameters();

            return exception;
        }

        var message = exception.InnerException != null ? $"Message: {exception.Message} \n InnerException Message: {exception.InnerException.Message})" : $"Message: {exception.Message} \n ";

        return new GeneralException(message, exception, invocation.GetLocation(), invocation.GetParameters());
    }
}


[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
public class ExceptionHandlerAttribute : Attribute
{
}