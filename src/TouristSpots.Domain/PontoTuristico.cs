namespace TouristSpots.Domain;

public sealed class PontoTuristico
{
    public const int TamanhoMaximoDescricao = 100;

    private PontoTuristico()
    {
        Nome = string.Empty;
        Descricao = string.Empty;
        Localizacao = string.Empty;
        Cidade = string.Empty;
        Estado = string.Empty;
    }

    public PontoTuristico(string nome, string descricao, string localizacao, string cidade, string estado, DateTime? criadoEm = null)
    {
        Id = Guid.NewGuid();
        Nome = ValidarCampoObrigatorio(nome, nameof(Nome), 120);
        Descricao = ValidarCampoObrigatorio(descricao, nameof(Descricao), TamanhoMaximoDescricao);
        Localizacao = ValidarCampoObrigatorio(localizacao, nameof(Localizacao), 200);
        Cidade = ValidarCampoObrigatorio(cidade, nameof(Cidade), 120);
        Estado = ValidarCampoObrigatorio(estado, nameof(Estado), 2).ToUpperInvariant();
        CriadoEm = criadoEm ?? DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public string Localizacao { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public DateTime CriadoEm { get; private set; }

    private static string ValidarCampoObrigatorio(string valor, string nomeCampo, int tamanhoMaximo)
    {
        var valorNormalizado = valor?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(valorNormalizado))
        {
            throw new ExcecaoDominio($"O campo {nomeCampo} e obrigatorio.");
        }

        if (valorNormalizado.Length > tamanhoMaximo)
        {
            throw new ExcecaoDominio($"O campo {nomeCampo} deve ter no maximo {tamanhoMaximo} caracteres.");
        }

        return valorNormalizado;
    }
}
