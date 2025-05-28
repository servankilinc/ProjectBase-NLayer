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
            .InterceptedBy(typeof(ValidationInterceptor), typeof(ExceptionHandlerInterceptor), typeof(CacheRemoveInterceptor), typeof(CacheRemoveGroupInterceptor), typeof(CacheInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<BlogService>().As<IBlogService>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(ValidationInterceptor), typeof(ExceptionHandlerInterceptor), typeof(CacheRemoveInterceptor), typeof(CacheRemoveGroupInterceptor), typeof(CacheInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<BlogLikeService>().As<IBlogLikeService>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(ValidationInterceptor), typeof(ExceptionHandlerInterceptor), typeof(CacheRemoveInterceptor), typeof(CacheRemoveGroupInterceptor), typeof(CacheInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<BlogCommentService>().As<IBlogCommentService>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(ValidationInterceptor), typeof(ExceptionHandlerInterceptor), typeof(CacheRemoveInterceptor), typeof(CacheRemoveGroupInterceptor), typeof(CacheInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<CategoryService>().As<ICategoryService>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(ValidationInterceptor), typeof(ExceptionHandlerInterceptor), typeof(CacheRemoveInterceptor), typeof(CacheRemoveGroupInterceptor), typeof(CacheInterceptor))
            .InstancePerLifetimeScope();
    }
}
