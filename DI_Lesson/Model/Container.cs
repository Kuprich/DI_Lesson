using DI_Lesson.Model.Interfaces;

namespace DI_Lesson.Model;

public class Container : IContainer
{
	public IScope CreateScope()
	{
		throw new NotImplementedException();
	}
}
