using DI_Lesson.Model.Descriptors;
using DI_Lesson.Model.Interfaces;

namespace DI_Lesson.Model;

public class ContainerBuilder : IContainerBuilder
{
	private readonly List<ServiceDescriptorBase> _descriptors = new();
	public IContainer Build()
	{
		return new Container(_descriptors);
	}

	public void Register(ServiceDescriptorBase descriptor)
	{
		_descriptors.Add(descriptor);
	}
}