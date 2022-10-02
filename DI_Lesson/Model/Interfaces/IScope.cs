namespace DI_Lesson.Model.Interfaces;

public interface IScope : IDisposable, IAsyncDisposable
{
    object Resolve(Type service);
}