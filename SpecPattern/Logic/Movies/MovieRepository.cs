using System.Linq.Expressions;

namespace Logic.Movies;

public class MovieRepository
{
    public Movie? GetOne(Guid guid)
    {
        //TODO: to be implemented
        return null;
    }

    public IReadOnlyList<Movie> GetList(
        Specification<Movie> specification,
        double minimumRating,
        int page = 0,
        int pageSize = 4)
    {
        //TODO: to be implemented;
        return new List<Movie>()
            .AsQueryable()
            .Where(specification.ToExpression())
            .Where(x => x.Rating >= minimumRating)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
    }
    
    // public IQueryable<Movie> Find()
    // {
    //     ISession session = SessionFactory.OpenSession();
    //     return session.Query<Movie>();
    //     // dbContext.Movies // For EntityFramework
    // }
}