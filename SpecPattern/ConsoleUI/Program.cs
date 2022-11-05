using Logic;

namespace ConsoleUI
{
    public class Program
    {
        //private static readonly MainViewModel ViewModel = new();

        public static void Main(string[] args)
        {
            while(true)
            {
                RenderInitialMenu();

                ReadAndProcessOptions(Console.ReadKey().KeyChar);
            }
        }

        private static void ReadAndProcessOptions(char input)
        {
            char inputUpper = Convert.ToChar(input.ToString().ToUpper());
            switch (inputUpper)
            {
                case '1':
                    Console.WriteLine(" Put 1 cent");
                    break;
                case '2':
                    Console.WriteLine(" Put 10 cent");
                    break;
                case 'Q':
                    Console.WriteLine(" Quit");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine($"Invalid Option - {input}");
                    break;
            }
        }

        //private static void InsertMoney(Money money) => ViewModel.InsertMoney(money);

        //private static void ReturnMoney() => ViewModel.ReturnMoney();

        //private static void BuySnack() => ViewModel.BuySnack();

        private static void RenderOutputDetails()
        {
            Console.WriteLine("----------------");
            // Console.WriteLine($"Credits in current Transaction: {ViewModel.GetInTransactionText()}");
            // Console.WriteLine($"Money Inside: {ViewModel.GetMoneyInsideText}");
            Console.WriteLine("----------------");
        }

        private static void RenderInitialMenu()
        {
            Console.Clear();
            Console.WriteLine("Movie List PipBoy Terminal");
            Console.WriteLine("----------------");
            Console.WriteLine("Choose an option:");

            foreach (var menuOption in MenuOptions)
            {
                Console.WriteLine(menuOption);
            }

            RenderOutputDetails();

            Console.Write("\r\nSelect an option: ");
        }
        
        private static readonly List<string> MenuOptions = 
            new()
            {
                "1 - Option 1",
                "Q - Quit",
            };
    }
}
