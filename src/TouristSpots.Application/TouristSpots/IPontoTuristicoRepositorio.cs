using TouristSpots.Application.Common;
using TouristSpots.Domain;

namespace TouristSpots.Application.PontosTuristicos;

public interface IPontoTuristicoRepositorio
{
    Task AdicionarAsync(PontoTuristico pontoTuristico, CancellationToken cancellationToken);
    Task<PontoTuristico?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ResultadoPaginado<PontoTuristico>> PesquisarAsync(string? termoBusca, int pagina, int tamanhoPagina, CancellationToken cancellationToken);
}
