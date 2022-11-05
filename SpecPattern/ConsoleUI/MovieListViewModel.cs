using System.Linq.Expressions;
using Logic.Movies;

namespace ConsoleUI;

public class MovieListViewModel
{
    private readonly MovieRepository _repository;
    
    //public Command SearchCommand { get; }
    //public Command<Guid> BuyTicketCommand { get; }
    public IReadOnlyList<Movie> Movies { get; private set; }
    
    public bool ForKidsOnly { get; set; }
    public bool OnCD { get; set; }

    public MovieListViewModel()
    {
        _repository = new MovieRepository();

        //SearchCommand = new Command(Search);
        //BuyTicketCommand = new Command<Guid>(BuyTicket);
    }
    
    private void BuyChildTicket(Guid movieGuid)
    {
        Movie movie = _repository.GetOne(movieGuid);
        if (movie == null)
        {
            return;
        }

        var specification = new MovieForKidsSpecification();

        if (!specification.IsSatisfiedBy(movie))
        {
            //The movie isn't suitable for children ERROR
            return;
        }
        
        // Proceed buying ticket
    }

    private void BuyCD(Guid movieGuid)
    {
        Movie movie = _repository.GetOne(movieGuid);
        if (movie == null)
        {
            return;
        }

        var specification = new AvailableOnCdSpecification();
        
        if (!specification.IsSatisfiedBy(movie))
        {
            // The movie doesn't have a CD version ERROR
            return;
        }
        
        // Proceed buying CD
    }

    private void Search()
    {
        var specification = new MovieForKidsSpecification();
        Movies = _repository.GetList(specification);

        //Notify(nameof(Movies);
    }
    
}
