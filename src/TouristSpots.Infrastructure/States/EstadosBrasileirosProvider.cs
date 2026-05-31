namespace TouristSpots.Infrastructure.Estados;

public static class EstadosBrasileirosProvider
{
    public static readonly IReadOnlyList<EstadoBrasileiro> Estados =
    [
        new("AC", "Acre"),
        new("AL", "Alagoas"),
        new("AP", "Amapa"),
        new("AM", "Amazonas"),
        new("BA", "Bahia"),
        new("CE", "Ceara"),
        new("DF", "Distrito Federal"),
        new("ES", "Espirito Santo"),
        new("GO", "Goias"),
        new("MA", "Maranhao"),
        new("MT", "Mato Grosso"),
        new("MS", "Mato Grosso do Sul"),
        new("MG", "Minas Gerais"),
        new("PA", "Para"),
        new("PB", "Paraiba"),
        new("PR", "Parana"),
        new("PE", "Pernambuco"),
        new("PI", "Piaui"),
        new("RJ", "Rio de Janeiro"),
        new("RN", "Rio Grande do Norte"),
        new("RS", "Rio Grande do Sul"),
        new("RO", "Rondonia"),
        new("RR", "Roraima"),
        new("SC", "Santa Catarina"),
        new("SP", "Sao Paulo"),
        new("SE", "Sergipe"),
        new("TO", "Tocantins")
    ];
}
