namespace TouristSpots.Application.PontosTuristicos;

public sealed record CadastrarPontoTuristicoRequest(
    string Nome,
    string Descricao,
    string Localizacao,
    string Cidade,
    string Estado);
