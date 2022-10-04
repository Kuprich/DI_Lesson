using DI_Lesson.Model.Descriptors;
using DI_Lesson.Model.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace DI_Lesson.Model.Builders;

public class LambdaBasedActivationbuilder : IActivationBuilder
{

    private static readonly MethodInfo? ResolveMethod = typeof(IScope).GetMethod("Resolve");

    public Func<IScope, object> BuildActivationInternal(
        TypeBasedServiceDescriptor tb,
        ConstructorInfo ctor,
        ParameterInfo[] args,
        ServiceDescriptorBase descriptor)
    {

        if (ResolveMethod == null)
            throw new Exception("Method Resolve not found!");

        var scopeParameter = Expression.Parameter(typeof(IScope), "scope");

        var parameters = args.Select(x =>
            Expression.Convert(
                 Expression.Call(scopeParameter, ResolveMethod, Expression.Constant(x.ParameterType)), 
                 x.ParameterType));

        var @new = Expression.New(ctor, parameters);

        var lambda = Expression.Lambda<Func<IScope, object>>(@new, scopeParameter);
        return lambda.Compile();

    }
}
