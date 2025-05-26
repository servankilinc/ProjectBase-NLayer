using Autofac;
using Autofac.Extras.DynamicProxy;
using Core.Utils.CrossCuttingConcerns;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.UoW;

namespace DataAccess;

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    { 
        // Unit of Work Service
        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>()
            .InstancePerLifetimeScope();

        // Repositoy Services
        builder.RegisterType<UserRepository>().As<IUserRepository>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(DataAccessExceptionInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<BlogRepository>().As<IBlogRepository>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(DataAccessExceptionInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<CategoryRepository>().As<ICategoryRepository>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(DataAccessExceptionInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<BlogLikeRepository>().As<IBlogLikeRepository>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(DataAccessExceptionInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<BlogCommentRepository>().As<IBlogCommentRepository>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(DataAccessExceptionInterceptor))
            .InstancePerLifetimeScope();
    }
}