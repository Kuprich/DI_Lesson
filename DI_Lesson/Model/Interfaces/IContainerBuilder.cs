using DI_Lesson.Model.Descriptors;

namespace DI_Lesson.Model.Interfaces;

public interface IContainerBuilder
{
    void Register(ServiceDescriptorBase descriptor);
    IContainer Build();
}
