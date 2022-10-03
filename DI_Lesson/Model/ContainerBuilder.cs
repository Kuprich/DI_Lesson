using DI_Lesson.Model.Builders;
using DI_Lesson.Model.Descriptors;
using DI_Lesson.Model.Interfaces;

namespace DI_Lesson.Model;

public class ContainerBuilder : IContainerBuilder
{
	private readonly List<ServiceDescriptorBase> _descriptors = new();
	public IContainer Build()
	{
		return new Container(_descriptors, new ReflectionBasedActivationBuilder());
	}

	public void Register(ServiceDescriptorBase descriptor)
	{
		_descriptors.Add(descriptor);
	}
}