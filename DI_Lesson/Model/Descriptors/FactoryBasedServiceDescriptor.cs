using DI_Lesson.Model.Interfaces;

namespace DI_Lesson.Model.Descriptors;

// Example: 
// Register<IHelper>(s => s = new Helper());
public class FactoryBasedServiceDescriptor : ServiceDescriptorBase
{
    public Func<IScope, object> Factory { get; init; } = null!;
}
