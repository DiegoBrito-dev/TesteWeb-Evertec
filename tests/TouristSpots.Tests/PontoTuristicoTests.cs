using TouristSpots.Domain;

namespace TouristSpots.Tests;

public sealed class PontoTuristicoTests
{
    [Fact]
    public void Construtor_QuandoDescricaoTemMaisDeCemCaracteres_DeveLancarExcecaoDominio()
    {
        var descricao = new string('a', PontoTuristico.TamanhoMaximoDescricao + 1);

        var acao = () => new PontoTuristico(
            "Museu",
            descricao,
            "Centro historico",
            "Sao Paulo",
            "SP");

        Assert.Throws<ExcecaoDominio>(acao);
    }

    [Fact]
    public void Construtor_QuandoEstadoEstaEmMinusculo_DeveNormalizarSigla()
    {
        var pontoTuristico = new PontoTuristico(
            "Parque",
            "Area verde central",
            "Avenida principal",
            "Curitiba",
            "pr");

        Assert.Equal("PR", pontoTuristico.Estado);
    }
}
