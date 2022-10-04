using DI_Lesson.Model.Descriptors;
using DI_Lesson.Model.Interfaces;
using System.Reflection;

namespace DI_Lesson.Model.Builders;

public interface IActivationBuilder
{
    Func<IScope, object> BuildActivation(ServiceDescriptorBase descriptor)
    {
        var tb = (TypeBasedServiceDescriptor)descriptor;
        var ctor = tb.ImplementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
        var args = ctor.GetParameters();

        return BuildActivationInternal(tb, ctor, args, descriptor);
    }

    Func<IScope, object> BuildActivationInternal(TypeBasedServiceDescriptor tb, ConstructorInfo ctor, ParameterInfo[] args, ServiceDescriptorBase descriptor);
}
