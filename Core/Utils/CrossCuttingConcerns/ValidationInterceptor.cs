using Castle.DynamicProxy;
using Core.Utils.CrossCuttingConcerns.Helpers;
using Core.Utils.ExceptionHandle.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Utils.CrossCuttingConcerns;

public class ValidationInterceptor : IInterceptor
{
    private readonly IServiceProvider _serviceProvider;
    public ValidationInterceptor(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public void Intercept(IInvocation invocation)
    {
        if (invocation.HasAttribute<ValidationAttribute>())
        {
            HandleIntercept(invocation);
        }
        else
        {
            invocation.Proceed();
        }
    }

    public void HandleIntercept(IInvocation invocation)
    {
        var methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;

        var attribute = invocation.GetAttribute<ValidationAttribute>();
        if (attribute == null) throw new InvalidOperationException("ValidationAttribute not found.");

        if (!methodInfo.IsAsync())
        {
            InterceptSync(invocation, attribute);
        }
        else
        {
            if (!methodInfo.IsGenericAsync())
            {
                invocation.ReturnValue = InterceptAsync(invocation, attribute);
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

    private void InterceptSync(IInvocation invocation, ValidationAttribute attribute)
    {
        CheckValidation(invocation, attribute.TargetType);
        invocation.Proceed();
    }

    private async Task InterceptAsync(IInvocation invocation, ValidationAttribute attribute)
    {
        CheckValidation(invocation, attribute.TargetType);
        invocation.Proceed();
        var task = (Task)invocation.ReturnValue;
        await task.ConfigureAwait(false);
    }

    private async Task<TResult> InterceptAsyncGeneric<TResult>(IInvocation invocation, ValidationAttribute attribute)
    {
        CheckValidation(invocation, attribute.TargetType);
        invocation.Proceed();
        var task = (Task<TResult>)invocation.ReturnValue;
        return await task.ConfigureAwait(false);
    }

    private void CheckValidation(IInvocation invocation, Type targetType)
    {
        var arg = invocation.Arguments.FirstOrDefault(arg => arg?.GetType() == targetType);
        if (arg == null) throw new InvalidOperationException("Arg object to validation could not be determined.");

        var validatorsType = typeof(IEnumerable<>).MakeGenericType(typeof(IValidator<>).MakeGenericType(targetType));

        var validators = (IEnumerable<IValidator>)_serviceProvider.GetRequiredService(validatorsType);
        if (!validators.Any()) return;

        var context = (IValidationContext)Activator.CreateInstance(typeof(ValidationContext<>).MakeGenericType(targetType), arg)!;
        if (context == null) throw new InvalidOperationException("ValidationContext could not be created.");

        IEnumerable<ValidationFailure> failures = validators
            .Select(validator => validator.Validate(context))
            .Where(result => !result.IsValid)
            .SelectMany(result => result.Errors)
            .ToList();

        
        if (failures.Any()){
            string message = "Validation Error(s):" + string.Join(", \n", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));
            throw new ValidationRuleException(message, failures, invocation.GetLocation(), invocation.GetParameters());
        }
    }
}


[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
public class ValidationAttribute : Attribute
{
    public Type TargetType { get; }
    public ValidationAttribute(Type targetType) => TargetType = targetType;
}