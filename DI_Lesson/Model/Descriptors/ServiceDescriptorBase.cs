namespace DI_Lesson.Model.Descriptors;

public enum LifeTime
{
    Singleton,
    Scoped,
    Transient
}

public abstract class ServiceDescriptorBase
{
    public Type ServiceType { get; init; } = null!;
    public LifeTime LifeTime { get; init; }
}
