namespace TouristSpots.Application.PontosTuristicos;

public sealed record PontoTuristicoDetalheResponse(
    Guid Id,
    string Nome,
    string Descricao,
    string Localizacao,
    string Cidade,
    string Estado,
    DateTime CriadoEm);
