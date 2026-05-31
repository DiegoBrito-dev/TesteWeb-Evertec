namespace TouristSpots.Application.PontosTuristicos;

public sealed record PontoTuristicoResumoResponse(
    Guid Id,
    string Nome,
    string Localizacao,
    string Cidade,
    string Estado,
    DateTime CriadoEm);
