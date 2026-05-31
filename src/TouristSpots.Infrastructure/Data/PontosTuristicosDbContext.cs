using Microsoft.EntityFrameworkCore;
using TouristSpots.Domain;

namespace TouristSpots.Infrastructure.Data;

public sealed class PontosTuristicosDbContext : DbContext
{
    public PontosTuristicosDbContext(DbContextOptions<PontosTuristicosDbContext> options)
        : base(options)
    {
    }

    public DbSet<PontoTuristico> PontosTuristicos => Set<PontoTuristico>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PontoTuristico>(entidade =>
        {
            entidade.ToTable("PontosTuristicos");
            entidade.HasKey(pontoTuristico => pontoTuristico.Id);

            entidade.Property(pontoTuristico => pontoTuristico.Nome)
                .HasMaxLength(120)
                .IsRequired();

            entidade.Property(pontoTuristico => pontoTuristico.Descricao)
                .HasMaxLength(PontoTuristico.TamanhoMaximoDescricao)
                .IsRequired();

            entidade.Property(pontoTuristico => pontoTuristico.Localizacao)
                .HasMaxLength(200)
                .IsRequired();

            entidade.Property(pontoTuristico => pontoTuristico.Cidade)
                .HasMaxLength(120)
                .IsRequired();

            entidade.Property(pontoTuristico => pontoTuristico.Estado)
                .HasMaxLength(2)
                .IsRequired();

            entidade.Property(pontoTuristico => pontoTuristico.CriadoEm)
                .IsRequired();

            entidade.HasIndex(pontoTuristico => pontoTuristico.CriadoEm);
            entidade.HasIndex(pontoTuristico => pontoTuristico.Nome);
        });
    }
}
