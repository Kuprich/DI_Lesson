using DI_Lesson.Model.Descriptors;
using DI_Lesson.Model.Interfaces;
using System.Reflection;

namespace DI_Lesson.Model;

public class Container : IContainer
{
    private class Scope : IScope
    {
        private readonly Container _container;

        public Scope(Container container)
        {
            _container = container;
        }
        public object Resolve(Type service)
        {
            return _container.CreateInstance(service, this);
        }
    }

    private Dictionary<Type, ServiceDescriptorBase> _descriptors;
    public Container(IEnumerable<ServiceDescriptorBase> descriptors)
    {
        _descriptors = descriptors.ToDictionary(x => x.ServiceType);
    }
    public IScope CreateScope()
    {
        return new Scope(this);
    }
    private object CreateInstance(Type service, IScope scope)
    {
        if (!_descriptors.TryGetValue(service, out var descriptor))
            throw new InvalidOperationException($"serivice {service} is not registered");
        
        if (descriptor is InstanceBasedServiceDescriptor ib)
            return ib.Instance;
        if (descriptor is FactoryBasedServiceDescriptor fb)
            return fb.Factory(scope);

        var tb = (TypeBasedServiceDescriptor)descriptor;

        //сделаем вид, что у нас только один конструктор
        var ctor = tb.ImplementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
        var args = ctor.GetParameters();
        var parameters = new object?[args.Length];

        for (int i = 0; i < args.Length; i++)
        {
            parameters[i] = CreateInstance(args[i].ParameterType, scope);
        }

        return ctor.Invoke(parameters);

    }
}
