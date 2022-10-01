namespace DI_Lesson.Model.Interfaces;

public interface IContainer
{
    IScope CreateScope();
}