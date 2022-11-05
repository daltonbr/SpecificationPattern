using Logic.Movies;

namespace ConsoleUI;

public class MovieListViewModel
{
    private readonly MovieRepository _repository;
    
    //public Command SearchCommand { get; }
    //public Command<Guid> BuyTicketCommand { get; }
    public IReadOnlyList<Movie> Movies { get; private set; }
    
    public bool ForKidsOnly { get; set; }

    public MovieListViewModel()
    {
        _repository = new MovieRepository();

        //SearchCommand = new Command(Search);
        //BuyTicketCommand = new Command<Guid>(BuyTicket);
    }
    
    private void BuyTicket(Guid movieGuid)
    {
        // MessageBox.Show("You've bought a ticket");
    }

    private void Search(bool forKidsOnly)
    {
        Movies = _repository.GetList(forKidsOnly);
        //Notify(nameof(Movies);
    }
    
}