using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Business.Utils.TokenService;
using Core.Utils.CrossCuttingConcerns;

namespace Business;

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TokenService>().As<ITokenService>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(ExceptionHandlerInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<AuthService>().As<IAuthService>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(ValidationInterceptor), typeof(ExceptionHandlerInterceptor))
            .InstancePerLifetimeScope();

        // ***** Entity Services *****
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
