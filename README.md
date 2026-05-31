# Teste Pratico .NET - Pontos Turisticos

Aplicacao em camadas para cadastro, listagem paginada, busca e detalhamento de pontos turisticos.

## Stack

- .NET 8
- ASP.NET Core Minimal APIs
- Entity Framework Core
- SQLite
- React 18 no front-end
- xUnit para testes unitarios

## Estrutura

- `src/TouristSpots.Domain`: entidade e regras de dominio.
- `src/TouristSpots.Application`: contratos, DTOs e casos de uso.
- `src/TouristSpots.Infrastructure`: EF Core, SQLite, repositorios e lista de estados.
- `src/TouristSpots.Api`: endpoints HTTP e configuracao da aplicacao.
- `frontend`: SPA React independente consumindo a API via `fetch`.
- `tests/TouristSpots.Tests`: testes unitarios.

## Como executar a API

```powershell
dotnet restore TouristSpotsChallenge.slnx
dotnet run --project src\TouristSpots.Api\TouristSpots.Api.csproj --urls http://localhost:5196
```

O banco SQLite e criado automaticamente no primeiro start com o arquivo `pontos-turisticos.db`.

## Como executar o front-end

Abra o arquivo `frontend/index.html` no navegador enquanto a API estiver rodando em `http://localhost:5196`.

Caso queira servir por HTTP local, tambem pode usar qualquer servidor estatico apontando para a pasta `frontend`.

## Endpoints principais

- `GET /api/pontos-turisticos?pagina=1&tamanhoPagina=8&busca=termo`
- `GET /api/pontos-turisticos/{id}`
- `POST /api/pontos-turisticos`
- `GET /api/estados`
- `GET /api/saude`

Exemplo de cadastro:

```json
{
  "nome": "Cristo Redentor",
  "descricao": "Monumento historico no Corcovado",
  "localizacao": "Parque Nacional da Tijuca",
  "cidade": "Rio de Janeiro",
  "estado": "RJ"
}
```

## Testes

```powershell
dotnet test TouristSpotsChallenge.slnx
```

## Commits solicitados no teste

O ambiente atual nao possui Git instalado/acessivel no PATH. Para cumprir a exigencia de pelo menos dois commits, execute em uma maquina com Git:

```powershell
git init
git add TouristSpotsChallenge.slnx .gitignore README.md src tests
git commit -m "Implementar API de pontos turisticos"
git add frontend
git commit -m "Implementar frontend React"
```
