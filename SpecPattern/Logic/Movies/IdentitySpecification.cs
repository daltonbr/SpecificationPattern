using System.Linq.Expressions;

namespace Logic.Movies;

internal sealed class IdentitySpecification<T> : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression() => x => true;
}