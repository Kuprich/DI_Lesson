using Autofac;
using BenchmarkDotNet.Attributes;
using DI_Lesson.Extensions;
using DI_Lesson.Model;
using DI_Lesson.Model.Builders;
using DI_Lesson.Model.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DI_Lesson;

[MemoryDiagnoser]
public class ContainerBenchmark
{
    private readonly IScope _reflectionBasedScope, _lambdaBasedScope;
	private readonly ILifetimeScope _lifetimeScope;
	private readonly IServiceScope _serviceScope;

	public ContainerBenchmark()
	{
		var lambdaBasedBuilder = new Model.ContainerBuilder(new LambdaBasedActivationbuilder());
		var reflectionBasedBuilder = new Model.ContainerBuilder(new ReflectionBasedActivationBuilder());

		InitContainer(lambdaBasedBuilder);
		InitContainer(reflectionBasedBuilder);

		_reflectionBasedScope = reflectionBasedBuilder.Build().CreateScope();
		_lambdaBasedScope = reflectionBasedBuilder.Build().CreateScope();

		_lifetimeScope = InitAutofac();
		_serviceScope = InitMsdi();


    }

	private IServiceScope InitMsdi()
	{
		var collection = new ServiceCollection();
		collection.AddTransient<IService, Service>()
			.AddTransient<Controller, Controller>();

		return collection.BuildServiceProvider().CreateScope();
	}

	private ILifetimeScope InitAutofac()
	{
		var containerBuilder = new Autofac.ContainerBuilder();
		containerBuilder.RegisterType<Service>().As<IService>();
		containerBuilder.RegisterType<Controller>().AsSelf();

		return containerBuilder.Build().BeginLifetimeScope();
	}

	private void InitContainer(IContainerBuilder builder)
	{
		builder.RegisterTransient<IService, Service>()
			.RegisterTransient<Controller, Controller>();
	}

	[Benchmark(Baseline = true)]
	public Controller Create() => new Controller(new Service());

    [Benchmark]
    public Controller Reflection() => (Controller)_reflectionBasedScope.Resolve(typeof(Controller));

    [Benchmark]
    public Controller Lambda() => (Controller)_lambdaBasedScope.Resolve(typeof(Controller));

	[Benchmark]
	public Controller Autofac() => _lifetimeScope.Resolve<Controller>();

	[Benchmark]
	public Controller Msdi() => _serviceScope.ServiceProvider.GetRequiredService<Controller>();
}
