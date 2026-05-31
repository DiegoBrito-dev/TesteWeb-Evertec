namespace TouristSpots.Domain;

public sealed class ExcecaoDominio : Exception
{
    public ExcecaoDominio(string mensagem)
        : base(mensagem)
    {
    }
}
