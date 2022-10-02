namespace DI_Lesson.Model.Interfaces;

public interface IContainer : IDisposable, IAsyncDisposable
{
    IScope CreateScope();
}