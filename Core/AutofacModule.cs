using Autofac;
using Core.Utils.CrossCuttingConcerns;

namespace Core;

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // ******** Interceptors **********
        builder.RegisterType<ValidationInterceptor>();
        builder.RegisterType<CacheInterceptor>();
        builder.RegisterType<CacheRemoveInterceptor>();
        builder.RegisterType<CacheRemoveGroupInterceptor>();
        builder.RegisterType<BusinessExceptionInterceptor>(); // Use Business Exception Spesific Errors for user feedbacks
        builder.RegisterType<DataAccessExceptionInterceptor>();
    }
}
