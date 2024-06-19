namespace BadmintonTournament;

class Player
{
    // public int Id { get; set; }
    public string Name { get; }
    public string Racket { get; }
    public double Rating { get; set; }
    public bool Main { get; }

    public Player(string name, string racket, double rating, bool main=false)
    {
        // Id = id;
        Name = name;
        Racket = racket;
        Rating = rating;
        Main = main;
    }
}