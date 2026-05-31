using TouristSpots.Application.Common;
using TouristSpots.Application.PontosTuristicos;
using TouristSpots.Domain;

namespace TouristSpots.Tests;

public sealed class PontoTuristicoServicoTests
{
    [Fact]
    public async Task PesquisarAsync_QuandoTamanhoPaginaEGrande_DeveLimitarTamanhoPagina()
    {
        var repositorio = new RepositorioPontoTuristicoFake();
        var servico = new PontoTuristicoServico(repositorio);

        var resultado = await servico.PesquisarAsync(null, pagina: -5, tamanhoPagina: 500, CancellationToken.None);

        Assert.Equal(1, resultado.Pagina);
        Assert.Equal(50, resultado.TamanhoPagina);
    }

    private sealed class RepositorioPontoTuristicoFake : IPontoTuristicoRepositorio
    {
        public Task AdicionarAsync(PontoTuristico pontoTuristico, CancellationToken cancellationToken) =>
            Task.CompletedTask;

        public Task<PontoTuristico?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken) =>
            Task.FromResult<PontoTuristico?>(null);

        public Task<ResultadoPaginado<PontoTuristico>> PesquisarAsync(
            string? termoBusca,
            int pagina,
            int tamanhoPagina,
            CancellationToken cancellationToken)
        {
            ResultadoPaginado<PontoTuristico> resultado = new([], pagina, tamanhoPagina, 0);

            return Task.FromResult(resultado);
        }
    }
}
