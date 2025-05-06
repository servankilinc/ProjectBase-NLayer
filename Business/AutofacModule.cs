using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Core.Utils.CrossCuttingConcerns;

namespace Business;

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserService>().As<IUserService>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(ValidationInterceptor), typeof(BusinessExceptionInterceptor), typeof(CacheRemoveInterceptor), typeof(CacheRemoveGroupInterceptor), typeof(CacheInterceptor))
            .InstancePerLifetimeScope();
    }
}
