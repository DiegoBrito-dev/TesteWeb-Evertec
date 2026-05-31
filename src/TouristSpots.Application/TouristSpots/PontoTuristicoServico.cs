using TouristSpots.Application.Common;
using TouristSpots.Domain;

namespace TouristSpots.Application.PontosTuristicos;

public sealed class PontoTuristicoServico
{
    private readonly IPontoTuristicoRepositorio repositorio;

    public PontoTuristicoServico(IPontoTuristicoRepositorio repositorio)
    {
        this.repositorio = repositorio;
    }

    public async Task<PontoTuristicoDetalheResponse> CadastrarAsync(CadastrarPontoTuristicoRequest request, CancellationToken cancellationToken)
    {
        var pontoTuristico = new PontoTuristico(
            request.Nome,
            request.Descricao,
            request.Localizacao,
            request.Cidade,
            request.Estado);

        await repositorio.AdicionarAsync(pontoTuristico, cancellationToken);

        return ParaDetalhe(pontoTuristico);
    }

    public async Task<PontoTuristicoDetalheResponse?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var pontoTuristico = await repositorio.ObterPorIdAsync(id, cancellationToken);

        return pontoTuristico is null ? null : ParaDetalhe(pontoTuristico);
    }

    public async Task<ResultadoPaginado<PontoTuristicoResumoResponse>> PesquisarAsync(
        string? termoBusca,
        int pagina,
        int tamanhoPagina,
        CancellationToken cancellationToken)
    {
        pagina = Math.Max(pagina, 1);
        tamanhoPagina = Math.Clamp(tamanhoPagina, 1, 50);

        var resultado = await repositorio.PesquisarAsync(termoBusca, pagina, tamanhoPagina, cancellationToken);

        return new ResultadoPaginado<PontoTuristicoResumoResponse>(
            resultado.Itens.Select(ParaResumo).ToArray(),
            resultado.Pagina,
            resultado.TamanhoPagina,
            resultado.TotalItens);
    }

    private static PontoTuristicoDetalheResponse ParaDetalhe(PontoTuristico pontoTuristico) =>
        new(
            pontoTuristico.Id,
            pontoTuristico.Nome,
            pontoTuristico.Descricao,
            pontoTuristico.Localizacao,
            pontoTuristico.Cidade,
            pontoTuristico.Estado,
            pontoTuristico.CriadoEm);

    private static PontoTuristicoResumoResponse ParaResumo(PontoTuristico pontoTuristico) =>
        new(
            pontoTuristico.Id,
            pontoTuristico.Nome,
            pontoTuristico.Localizacao,
            pontoTuristico.Cidade,
            pontoTuristico.Estado,
            pontoTuristico.CriadoEm);
}
