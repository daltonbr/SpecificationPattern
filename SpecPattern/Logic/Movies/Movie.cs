using Logic.Utils;

namespace Logic.Movies;

public class Movie : Entity
{
    public string Name { get; }
    public DateTime ReleaseDate { get; }
    public MpaaRating MpaaRating { get; }
    public string Genre { get; }
    public double Rating { get; }
    public Director Director { get; }

    protected Movie()
    {
    }

}