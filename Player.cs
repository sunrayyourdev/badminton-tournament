namespace BadmintonTournament;

class Player
{
    public string Name { get; }
    public string Racket { get; }
    public double Rating { get; set; }
    public bool Main { get; }
    public int Points { set; get; } = 0;

    public Player(string name, string racket, double rating, bool main = false)
    {
        Name = name;
        Racket = racket;
        Rating = rating;
        Main = main;
    }

    public void Reset()
    {
        Points = 0;
    }
}