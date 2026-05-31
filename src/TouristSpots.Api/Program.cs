using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TouristSpots.Application.PontosTuristicos;
using TouristSpots.Domain;
using TouristSpots.Infrastructure;
using TouristSpots.Infrastructure.Data;
using TouristSpots.Infrastructure.Estados;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddProblemDetails();
builder.Services.AddScoped<PontoTuristicoServico>();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionFeature?.Error;

        var problem = exception switch
        {
            ExcecaoDominio excecaoDominio => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Dados do ponto turistico invalidos.",
                Detail = excecaoDominio.Message
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Ocorreu um erro inesperado."
            }
        };

        context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
        await Results.Problem(problem).ExecuteAsync(context);
    });
});

app.UseCors("Frontend");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PontosTuristicosDbContext>();
    dbContext.Database.EnsureCreated();
}

app.MapGet("/api/saude", () => Results.Ok(new { status = "Saudavel" }));

app.MapGet("/api/estados", () => Results.Ok(EstadosBrasileirosProvider.Estados));

var pontosTuristicos = app.MapGroup("/api/pontos-turisticos");

pontosTuristicos.MapGet("/", async (
    PontoTuristicoServico servico,
    string? busca,
    int pagina,
    int tamanhoPagina,
    CancellationToken cancellationToken) =>
{
    var resultado = await servico.PesquisarAsync(
        busca,
        pagina == 0 ? 1 : pagina,
        tamanhoPagina == 0 ? 8 : tamanhoPagina,
        cancellationToken);

    return Results.Ok(resultado);
});

pontosTuristicos.MapGet("/{id:guid}", async (
    Guid id,
    PontoTuristicoServico servico,
    CancellationToken cancellationToken) =>
{
    var pontoTuristico = await servico.ObterPorIdAsync(id, cancellationToken);

    return pontoTuristico is null ? Results.NotFound() : Results.Ok(pontoTuristico);
});

pontosTuristicos.MapPost("/", async (
    CadastrarPontoTuristicoRequest request,
    PontoTuristicoServico servico,
    CancellationToken cancellationToken) =>
{
    var cadastrado = await servico.CadastrarAsync(request, cancellationToken);

    return Results.Created($"/api/pontos-turisticos/{cadastrado.Id}", cadastrado);
});

app.Run();
