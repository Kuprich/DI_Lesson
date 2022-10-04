using BenchmarkDotNet.Attributes;
using DI_Lesson.Extensions;
using DI_Lesson.Model;
using DI_Lesson.Model.Builders;
using DI_Lesson.Model.Interfaces;

namespace DI_Lesson;

public class ContainerBenchmark
{
    private readonly IScope _reflectionBased, _lambdaBased;

	public ContainerBenchmark()
	{
		var lambdaBasedBuilder = new ContainerBuilder(new LambdaBasedActivationbuilder());
		var reflectionBasedBuilder = new ContainerBuilder(new ReflectionBasedActivationBuilder());

		InitContainer(lambdaBasedBuilder);
		InitContainer(reflectionBasedBuilder);

		_reflectionBased = reflectionBasedBuilder.Build().CreateScope();
		_lambdaBased = reflectionBasedBuilder.Build().CreateScope();

    }

	private void InitContainer(IContainerBuilder builder)
	{
		builder.RegisterTransient<IService, Service>()
			.RegisterScoped<Controller, Controller>();
	}

	[Benchmark(Baseline = true)]
	public Controller Create() => new Controller(new Service());

    [Benchmark]
    public Controller Reflection() => (Controller)_reflectionBased.Resolve(typeof(Controller));

    [Benchmark]
    public Controller Lambda() => (Controller)_lambdaBased.Resolve(typeof(Controller));
}
