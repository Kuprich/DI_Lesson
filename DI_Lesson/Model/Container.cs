using DI_Lesson.Model.Builders;
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
        private readonly ConcurrentStack<object> _disposables = new();

        public Scope(Container container)
        {
            _container = container;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                if (disposable is IDisposable d)
                    d.Dispose();
                else if (disposable is IAsyncDisposable ad)
                    ad.DisposeAsync().GetAwaiter().GetResult();
            }
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var disposable in _disposables)
            {
                if (disposable is IAsyncDisposable ad)
                    await ad.DisposeAsync();
                else if (disposable is IDisposable d)
                    d.Dispose();
            }
        }

        public object Resolve(Type service)
        {
            var descriptor = _container.FindDescriptor(service);
            if (descriptor == null)
                throw new NullReferenceException($"{nameof(descriptor)} can not be null");

            if (descriptor.LifeTime == LifeTime.Transient)
                return CreateInstanceInternal(service);

            if (descriptor.LifeTime == LifeTime.Scoped || _container._rootScope == this)
                return _scopedInstances.GetOrAdd(service, service => CreateInstanceInternal(service));

            return _container._rootScope.Resolve(service);
        }
        object CreateInstanceInternal(Type service)
        {
            var result = _container.CreateInstance(service, this);
            if (result is IDisposable || result is IAsyncDisposable)
                _disposables.Push(result);
            return result;
        }
    }



    private ImmutableDictionary<Type, ServiceDescriptorBase> _descriptors;
    private ConcurrentDictionary<Type, Func<IScope, object>> _builtActivators = new();
    private readonly IScope _rootScope;
    private IActivationBuilder _builder;

    public Container(IEnumerable<ServiceDescriptorBase> descriptors, IActivationBuilder builder)
    {
        _descriptors = descriptors.ToImmutableDictionary(x => x.ServiceType);
        _rootScope = new Scope(this);
        _builder = builder;
    }

    private ServiceDescriptorBase? FindDescriptor(Type service)
    {
        _descriptors.TryGetValue(service, out var result);
        return result;
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

        return _builder.BuildActivation(descriptor);

    }

    private object CreateInstance(Type service, IScope scope)
    {
        // что бы каждый раз не лазить по метаданным объекта, данные будут находиться в словаре _builtActivators
        return _builtActivators.GetOrAdd(service, BuildActivation)(scope);
    }

    public void Dispose()
    {
        _rootScope.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _rootScope.DisposeAsync();
    }
}
