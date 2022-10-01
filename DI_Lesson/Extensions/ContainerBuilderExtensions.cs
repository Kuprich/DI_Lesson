using DI_Lesson.Model.Descriptors;
using DI_Lesson.Model.Interfaces;

namespace DI_Lesson.Extensions;

public static class ContainerBuilderExtensions
{
    // Register Singleton
    public static IContainerBuilder RegisterSingleton(this IContainerBuilder builder, Type serviceInterface, Type serviceImplementation) =>
        builder.RegisterType(serviceInterface, serviceImplementation, LifeTime.Singleton);
    public static IContainerBuilder RegisterSingleton<TService, TImplementation>(this IContainerBuilder builder) where TImplementation : TService =>
        builder.RegisterType(typeof(TService), typeof(TImplementation), LifeTime.Singleton);
    public static IContainerBuilder RegisterSingleton(this IContainerBuilder builder, Type serviceInterface, Func<IScope, object> factory) =>
        builder.RegisterFactory(serviceInterface, factory, LifeTime.Singleton);
    public static IContainerBuilder RegisterSingleton<TService>(this IContainerBuilder builder, Func<IScope, object> factory) =>
        builder.RegisterFactory(typeof(TService), factory, LifeTime.Singleton);
    public static IContainerBuilder RegisterSingleton<TService>(this IContainerBuilder builder, object instance) =>
        builder.RegisterInstance(typeof(TService), instance);

    // Register Transient
    public static IContainerBuilder RegisterTransient(this IContainerBuilder builder, Type serviceInterface, Type serviceImplementation) =>
       builder.RegisterType(serviceInterface, serviceImplementation, LifeTime.Transient);
    public static IContainerBuilder RegisterTransient<TService, TImplementation>(this IContainerBuilder builder) where TImplementation : TService =>
        builder.RegisterType(typeof(TService), typeof(TImplementation), LifeTime.Transient);
    public static IContainerBuilder RegisterTransient(this IContainerBuilder builder, Type serviceInterface, Func<IScope, object> factory) =>
        builder.RegisterFactory(serviceInterface, factory, LifeTime.Transient);
    public static IContainerBuilder RegisterTransient<TService>(this IContainerBuilder builder, Func<IScope, object> factory) =>
        builder.RegisterFactory(typeof(TService), factory, LifeTime.Transient);

    // Register Scoped
    public static IContainerBuilder RegisterScoped(this IContainerBuilder builder, Type serviceInterface, Type serviceImplementation) =>
       builder.RegisterType(serviceInterface, serviceImplementation, LifeTime.Scoped);
    public static IContainerBuilder RegisterScoped<TService, TImplementation>(this IContainerBuilder builder) where TImplementation : TService =>
        builder.RegisterType(typeof(TService), typeof(TImplementation), LifeTime.Scoped);
    public static IContainerBuilder RegisterScoped(this IContainerBuilder builder, Type serviceInterface, Func<IScope, object> factory) =>
        builder.RegisterFactory(serviceInterface, factory, LifeTime.Scoped);
    public static IContainerBuilder RegisterScoped<TService>(this IContainerBuilder builder, Func<IScope, object> factory) =>
        builder.RegisterFactory(typeof(TService), factory, LifeTime.Scoped);



    private static IContainerBuilder RegisterType(this IContainerBuilder builder, Type serviceInterface, Type serviceImplementation, LifeTime lifeTime)
    {
        builder.Register(new TypeBasedServiceDescriptor
        {
            ImplementationType = serviceImplementation,
            ServiceType = serviceInterface,
            LifeTime = lifeTime
        });

        return builder;
    }

    private static IContainerBuilder RegisterFactory(this IContainerBuilder builder, Type serviceInterface, Func<IScope, object> factory, LifeTime lifeTime)
    {
        builder.Register(new FactoryBasedServiceDescriptor
        {
            Factory = factory,
            ServiceType = serviceInterface,
            LifeTime = lifeTime
        });

        return builder;
    }

    private static IContainerBuilder RegisterInstance(this IContainerBuilder builder, Type serviceInterface, object instance)
    {
        builder.Register(new InstanceBasedServiceDescriptor(serviceInterface, instance));

        return builder;
    }
}
