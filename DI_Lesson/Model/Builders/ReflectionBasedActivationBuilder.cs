using DI_Lesson.Model.Descriptors;
using DI_Lesson.Model.Interfaces;
using System.Reflection;

namespace DI_Lesson.Model.Builders;

public class ReflectionBasedActivationBuilder : IActivationBuilder
{
    public Func<IScope, object> BuildActivation(ServiceDescriptorBase descriptor)
    {
        var tb = (TypeBasedServiceDescriptor)descriptor;

        var ctor = tb.ImplementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
        var args = ctor.GetParameters();

        return scope =>
        {
            var parameters = new object?[args.Length];

            for (int i = 0; i < args.Length; i++)
            {
                parameters[i] = scope.Resolve(args[i].ParameterType);
            }

            return ctor.Invoke(parameters);
        };
    }
}
