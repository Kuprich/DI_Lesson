namespace DI_Lesson.Model.Interfaces;

public interface IScope
{
    object Resolve(Type service);
}