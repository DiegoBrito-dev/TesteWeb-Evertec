using Microsoft.EntityFrameworkCore;
using TouristSpots.Application.Common;
using TouristSpots.Application.PontosTuristicos;
using TouristSpots.Domain;
using TouristSpots.Infrastructure.Data;

namespace TouristSpots.Infrastructure.PontosTuristicos;

public sealed class EfPontoTuristicoRepositorio : IPontoTuristicoRepositorio
{
    private readonly PontosTuristicosDbContext dbContext;

    public EfPontoTuristicoRepositorio(PontosTuristicosDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AdicionarAsync(PontoTuristico pontoTuristico, CancellationToken cancellationToken)
    {
        await dbContext.PontosTuristicos.AddAsync(pontoTuristico, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<PontoTuristico?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        dbContext.PontosTuristicos
            .AsNoTracking()
            .FirstOrDefaultAsync(pontoTuristico => pontoTuristico.Id == id, cancellationToken);

    public async Task<ResultadoPaginado<PontoTuristico>> PesquisarAsync(
        string? termoBusca,
        int pagina,
        int tamanhoPagina,
        CancellationToken cancellationToken)
    {
        var consulta = dbContext.PontosTuristicos.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(termoBusca))
        {
            var termo = termoBusca.Trim();
            var termoLike = $"%{termo}%";

            consulta = consulta.Where(pontoTuristico =>
                EF.Functions.Like(pontoTuristico.Nome, termoLike) ||
                EF.Functions.Like(pontoTuristico.Descricao, termoLike) ||
                EF.Functions.Like(pontoTuristico.Localizacao, termoLike));
        }

        var totalItens = await consulta.CountAsync(cancellationToken);
        var itens = await consulta
            .OrderByDescending(pontoTuristico => pontoTuristico.CriadoEm)
            .ThenBy(pontoTuristico => pontoTuristico.Nome)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync(cancellationToken);

        return new ResultadoPaginado<PontoTuristico>(itens, pagina, tamanhoPagina, totalItens);
    }
}
