using DI_Lesson.Extensions;
using DI_Lesson.Model;
using DI_Lesson.Model.Interfaces;

var reg = new Registration();
var container = reg.ConfigureServices();
container.Resolve<Controller>().Do();
class Container : IContainer
{
	public IScope CreateScope()
	{
		throw new NotImplementedException();
	}
}

class Registration
{
	public Container ConfigureServices()
	{
		var builder = new ContainerBuilder();

		builder.RegisterSingleton<IService, Service>();

		//builder.Register<IService, Service>();
		//builder.Register<Controller, Controller>();
		return builder.Build();
	}
}

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

interface IService
{

}

class Service : IService
{

}