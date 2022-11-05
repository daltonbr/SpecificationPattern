using System.Linq.Expressions;

namespace Logic.Movies;

public sealed class MovieForKidsSpecification : Specification<Movie>
{
    public override Expression<Func<Movie, bool>> ToExpression() 
        => movie => movie.MpaaRating <= MpaaRating.PG;
}