namespace BadmintonTournament;

class Program
{
    static void Main(string[] args)
    {
        // Checks if player file exists
        var path = "player.txt";
        if (!File.Exists(path))
        {
            Console.WriteLine($"{path} does not exist.\nExiting program...");
            Environment.Exit(0);
        }


        // 1. Create User Character
        Console.WriteLine("Create your badminton player:");

        string name;
        string racket;
        double rating;
        bool confirm;

        do
        {
            name = TextInput("Name");
            racket = TextInput("Racket");
            rating = IntegerInput("Rating", 1, 5);

            Console.WriteLine($"Name: {name}, Racket: {racket}, Rating: {rating}");
            confirm = ConfirmInput("Keep this player?");
        } while (!confirm);
        
        var main_player = new Player(name, racket, rating);


        // 2. Set Up Tournament Bracket
        int number_of_players = 16;

        Player[] player_list = new Player[number_of_players];
        player_list[0] = main_player;

        using StreamReader reader = File.OpenText(path);
        for (int i = 1; i < player_list.Length; i++)
        {
            try
            {
                string[] attributes = reader.ReadLine().Split(", ");
                var p = new Player(attributes[0], attributes[1], double.Parse(attributes[2]));
                player_list[i] = p;
            }
            catch (FormatException err)
            {
                Console.WriteLine($"{err}: Attribute cannot be converted to double\nExiting program...");
                Environment.Exit(0);
            }
        }

        // Shuffle Indexes
        Shuffle(player_list);
        
        static void Shuffle<T>(T[] array)
        {
            var random = new Random();
            int n = array.Length;

            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (array[j], array[i]) = (array[i], array[j]);
            }
        }
        
        //  Display All Players
        Console.WriteLine("\nBracket: ");
        foreach (var p in player_list)
        {
            Console.WriteLine($"{p.Name}, {p.Racket}, {p.Rating}");
        }


        // 3. Simulate matches
        var main_bracket = player_list.ToList();
        var losers = new List<Player>();
        var bracket_stages = new string[]{"Finals", "Semi-finals", "Quarter-finals", "First Round"};

        // Counts backwards from list length to get appropriate round number
        for (int n = (int)Math.Sqrt(player_list.Length)-1; n >= 0; n--)
        {
            Console.WriteLine($"\n{bracket_stages[n]}:");
            for (int i = 0; i < main_bracket.Count; i++)
            {
                var p1 = main_bracket[i];
                var p2 = main_bracket[i + 1];

                DetermineWinner(out Player winner, out Player loser, p1, p2);
                main_bracket.Remove(loser);
                losers.Add(loser);

                Console.WriteLine($"{p1.Name} ({p1.Rating}) vs {p2.Name} ({p2.Rating}) | Score: {p1.Points}-{p2.Points}, Winner: {winner.Name}");
                p1.Reset();
                p2.Reset();
            }
        }

        static void DetermineWinner(out Player winner, out Player loser, Player p1, Player p2)
        {
            var p1_odds = Math.Round(p1.Rating / (p1.Rating + p2.Rating) * 1000);
            while ((int)Math.Pow(p1.Points - p2.Points, 2) < 4 || (p1.Points < 21 && p2.Points < 21))
            {
                var random_number = new Random().Next(1000);
                if (random_number <= p1_odds)
                {
                    p1.Points += 1;
                }
                else
                {
                    p2.Points += 1;
                }
            }
            if (p1.Points > p2.Points)
            {
                winner = p1;
                loser = p2;
            }
            else
            {
                winner = p2;
                loser = p1;
            }
        }


        // 4. Display result
        var final_winner = main_bracket[0];
        Console.WriteLine($"\nWinner of Tournament: {final_winner.Name}! ({final_winner.Racket}, {final_winner.Rating})");
    }


    // Input Methods

    static string TextInput(string text)
    {
        string? new_text;
        while (true)
        {
            Console.Write($"{text}: ");
            new_text = Console.ReadLine();
            if (!string.IsNullOrEmpty(new_text) && new_text.All(ch => char.IsLetter(ch) || char.IsWhiteSpace(ch)))
            {
                return new_text;
            }
            Console.WriteLine("Invalid input");
        }
    }

    // static double DoubleInput(string text, double min, double max)
    // {
    //     string? user_input;
    //     while (true)
    //     {
    //         Console.Write($"{text} [{min}-{max}]: ");
    //         user_input = Console.ReadLine();
    //         if (int.TryParse(user_input, out int new_double) && new_double >= min && new_double <= max)
    //         {
    //             return new_double;
    //         }
    //         Console.WriteLine("Invalid input");
    //     }
    // }

    static int IntegerInput(string text, int min, int max)
    {
        string? user_input;
        while (true)
        {
            Console.Write($"{text} [{min}-{max}]: ");
            user_input = Console.ReadLine();
            if (int.TryParse(user_input, out int new_integer) && new_integer >= min && new_integer <= max)
            {
                return new_integer;
            }
            Console.WriteLine("Invalid input");
        }
    }

    static bool ConfirmInput(string text)
    {
        while (true)
        {
            Console.Write($"{text} (y/n): ");
            var confirm_input = Console.ReadLine().ToLower();
            switch (confirm_input)
            {
                case "y":
                    return true;
                case "n":
                    return false;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }
}