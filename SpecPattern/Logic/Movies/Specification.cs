using System.Linq.Expressions;

namespace Logic.Movies;

public abstract class Specification<T>
{
    public bool IsSatisfiedBy(T entity)
    {
        Func<T,bool> predicate = ToExpression().Compile();
        return predicate(entity);
    }

    public abstract Expression<Func<T, bool>> ToExpression();
}
