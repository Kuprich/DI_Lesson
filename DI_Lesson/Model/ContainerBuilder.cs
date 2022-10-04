using DI_Lesson.Model.Builders;
using DI_Lesson.Model.Descriptors;
using DI_Lesson.Model.Interfaces;

namespace DI_Lesson.Model;

public class ContainerBuilder : IContainerBuilder
{
	private readonly List<ServiceDescriptorBase> _descriptors = new();
	private readonly IActivationBuilder _builder;

	public ContainerBuilder(IActivationBuilder builder)
	{
		_builder = builder;
	}
	
	public IContainer Build()
	{
		return new Container(_descriptors, _builder);
	}

	public void Register(ServiceDescriptorBase descriptor)
	{
		_descriptors.Add(descriptor);
	}
}