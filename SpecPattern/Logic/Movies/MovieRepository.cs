namespace Logic.Movies;

public class MovieRepository
{
    public Movie GetOne(Guid guid)
    {
        //TODO: to be implemented
        return null;
    }

    public IReadOnlyList<Movie> GetList(bool forKidsOnly)
    {
        //TODO: to be implemented;
        return new List<Movie>()
            .Where(movie => movie.MpaaRating <= MpaaRating.PG || !forKidsOnly)
            .ToList();
    }
}