namespace DI_Lesson.Model.Descriptors;

// Example:
// Register<IService>(Service);
public class InstanceBasedServiceDescriptor : ServiceDescriptorBase
{
    public object Instance { get; init; } = null!;

    public InstanceBasedServiceDescriptor(Type serviceType, object instance)
    {
        LifeTime = LifeTime.Singleton;
        ServiceType = serviceType;
        Instance = instance;
    }
}