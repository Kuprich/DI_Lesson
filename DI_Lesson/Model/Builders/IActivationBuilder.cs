using DI_Lesson.Model.Descriptors;
using DI_Lesson.Model.Interfaces;

namespace DI_Lesson.Model.Builders;

public interface IActivationBuilder
{
    Func<IScope, object> BuildActivation(ServiceDescriptorBase descriptor);
}
