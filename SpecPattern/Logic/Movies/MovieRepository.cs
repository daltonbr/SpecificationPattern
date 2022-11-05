using System.Linq.Expressions;

namespace Logic.Movies;

public class MovieRepository
{
    public Movie? GetOne(Guid guid)
    {
        //TODO: to be implemented
        return null;
    }

    public IReadOnlyList<Movie> GetList(Specification<Movie> specification)
    {
        //TODO: to be implemented;
        return new List<Movie>()
            .AsQueryable()
            .Where(specification.ToExpression())
            .ToList();
    }
    
    // public IQueryable<Movie> Find()
    // {
    //     ISession session = SessionFactory.OpenSession();
    //     return session.Query<Movie>();
    //     // dbContext.Movies // For EntityFramework
    // }
}