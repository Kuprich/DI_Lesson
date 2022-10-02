using DI_Lesson.Model.Descriptors;
using DI_Lesson.Model.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;

namespace DI_Lesson.Model;

public class Container : IContainer
{
    private class Scope : IScope
    {
        private readonly Container _container;
        private readonly ConcurrentDictionary<Type, object> _scopedInstances = new();

        public Scope(Container container)
        {
            _container = container;
        }
        public object Resolve(Type service)
        {
            var descriptor = _container.FindDescriptor(service);
            if (descriptor == null)
                throw new NullReferenceException($"{nameof(descriptor)} can not be null");

            if (descriptor.LifeTime == LifeTime.Transient)
                return _container.CreateInstance(service, this);

            if (descriptor.LifeTime == LifeTime.Scoped || _container._rootScope == this)
                return _scopedInstances.GetOrAdd(service, service => _container.CreateInstance(service, this));

            return _container._rootScope.Resolve(service);
           
        }
    }



    private ImmutableDictionary<Type, ServiceDescriptorBase> _descriptors;
    private ConcurrentDictionary<Type, Func<IScope, object>> _builtActivators = new();
    private readonly IScope _rootScope;

    private ServiceDescriptorBase? FindDescriptor(Type service)
    {
        _descriptors.TryGetValue(service, out var result);
        return result;
    }

    public Container(IEnumerable<ServiceDescriptorBase> descriptors)
    {
        _descriptors = descriptors.ToImmutableDictionary(x => x.ServiceType);
        _rootScope = new Scope(this);
    }
    public IScope CreateScope()
    {
        return new Scope(this);
    }

    private Func<IScope, object> BuildActivation(Type service)
    {
        if (!_descriptors.TryGetValue(service, out var descriptor))
            throw new InvalidOperationException($"serivice {service} is not registered");

        if (descriptor is InstanceBasedServiceDescriptor ib)
            return _ => ib.Instance;
        if (descriptor is FactoryBasedServiceDescriptor fb)
            return fb.Factory;

        var tb = (TypeBasedServiceDescriptor)descriptor;

        var ctor = tb.ImplementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
        var args = ctor.GetParameters();

        return scope =>
        {
            var parameters = new object?[args.Length];

            for (int i = 0; i < args.Length; i++)
            {
                parameters[i] = CreateInstance(args[i].ParameterType, scope);
            }

            return ctor.Invoke(parameters);
        };
    }

    private object CreateInstance(Type service, IScope scope)
    {
        // что бы каждый раз не лазить по метаданным объекта, данные будут находиться в словаре _builtActivators
        return _builtActivators.GetOrAdd(service, BuildActivation)(scope);
    }
}
