namespace DependencyInjection.Randomer;

public class Randomer : IRandomer
{
    private readonly int _random;

    public Randomer()
    {
        Random rnd = new Random();
        _random = rnd.Next(1, 1000);
    }

    public int GetRandomNumber()
    {
        return _random;
    }
}
