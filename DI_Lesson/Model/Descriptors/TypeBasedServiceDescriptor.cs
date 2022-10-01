namespace DI_Lesson.Model.Descriptors;

// Register<IService, Service>();
public class TypeBasedServiceDescriptor : ServiceDescriptorBase
{
    public Type ImplementationType { get; init; } = null!;
}
