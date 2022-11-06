using System.Linq.Expressions;

namespace Logic.Movies;

public abstract class Specification<T>
{
    public static readonly Specification<T> All = new IdentitySpecification<T>();
    public bool IsSatisfiedBy(T entity)
    {
        Func<T,bool> predicate = ToExpression().Compile();
        return predicate(entity);
    }

    public abstract Expression<Func<T, bool>> ToExpression();

    public Specification<T> And(Specification<T> specification)
    {
        // optimization
        if (this == All)
        {
            return specification;
        }

        if (specification == All)
        {
            return this;
        }

        return new AndSpecification<T>(this, specification);
    }

    public Specification<T> Or(Specification<T> specification)
    {
        // optimization
        if (this == All || specification == All)
        {
            return All;
        }
        
        return new OrSpecification<T>(this, specification);
    }

    public Specification<T> Not()
    {
        return new NotSpecification<T>(this);
    }
}

public sealed class MovieDirectedBySpecification : Specification<Movie>
{
    private readonly string _director;

    public MovieDirectedBySpecification(string director)
    {
        _director = director;
    }
    public override Expression<Func<Movie, bool>> ToExpression() 
        => movie => movie.Director.Name == _director;
}
