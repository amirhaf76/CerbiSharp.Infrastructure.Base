using Autofac;
using CerbiSharp.Infrastructure.Base.AutoFac.FlagInterface;

namespace CerbiSharp.Infrastructure.Base.AutoFac
{
    public abstract class BaseDIModule<T> : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(T).Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .Where(x => !x.IsInterface && !x.IsAbstract && typeof(IScope).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assembly)
                .Where(x => !x.IsInterface && !x.IsAbstract && typeof(ITransient).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(assembly)
                .Where(x => !x.IsInterface && !x.IsAbstract && typeof(ISingleton).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
