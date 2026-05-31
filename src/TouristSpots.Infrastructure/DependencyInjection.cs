using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TouristSpots.Application.PontosTuristicos;
using TouristSpots.Infrastructure.Data;
using TouristSpots.Infrastructure.PontosTuristicos;

namespace TouristSpots.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PontosTuristicos")
            ?? "Data Source=pontos-turisticos.db";

        services.AddDbContext<PontosTuristicosDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<IPontoTuristicoRepositorio, EfPontoTuristicoRepositorio>();

        return services;
    }
}
