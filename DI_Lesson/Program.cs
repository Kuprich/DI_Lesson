using DI_Lesson.Extensions;
using DI_Lesson.Model;
using DI_Lesson.Model.Interfaces;

IContainerBuilder builder = new ContainerBuilder();
IContainer container = builder
	.RegisterScoped<IService, Service>()
	.RegisterScoped<Controller, Controller>()
	.Build();


IScope scope = container.CreateScope();
var controller1 = scope.Resolve(typeof(Controller));
var controller2 = scope.Resolve(typeof(Controller));

if (controller1 != controller2)
{
	throw new InvalidOperationException();
}

Console.ReadKey();



class Controller
{
	private readonly IService _service;

	public Controller(IService service)
	{
		_service = service;
	}
	public void Do()
	{

	}
}

interface IService { }
class Service : IService { }

interface IHelper { }
class Helper : IHelper { }