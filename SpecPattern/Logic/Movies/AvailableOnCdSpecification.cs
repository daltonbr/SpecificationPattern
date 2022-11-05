using System.Linq.Expressions;

namespace Logic.Movies;

public sealed class AvailableOnCdSpecification : Specification<Movie>
{
    private const int MonthsBeforeCDisReleased = 6;
    public override Expression<Func<Movie, bool>> ToExpression()
        => movie => movie.ReleaseDate <= DateTime.Now.AddMonths(- MonthsBeforeCDisReleased);
}