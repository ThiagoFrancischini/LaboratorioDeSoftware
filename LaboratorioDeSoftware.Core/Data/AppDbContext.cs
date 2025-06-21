using LaboratorioDeSoftware.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LaboratorioDeSoftware.Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Laboratorio> Laboratorios { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<CategoriaItem> Categorias { get; set; }
    public DbSet<Equipamento> Equipamentos { get; set; }
    public DbSet<TagEquipamento> TagsEquipamento { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);
        });

        modelBuilder.Entity<Laboratorio>(entity =>
        {
            entity.HasKey(l => l.Id);

            entity.HasOne(l => l.Responsavel)
                  .WithMany()
                  .HasForeignKey(l => l.ResponsavelId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Equipamento>(entity =>
        {
            entity.HasKey(e => e.Id);
                        
            entity.HasOne(e => e.Produto)
                .WithMany(p => p.Equipamentos)
                .HasForeignKey(e => e.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
                            
            entity.HasOne(e => e.Laboratorio)
                .WithMany(l => l.Equipamentos)
                .HasForeignKey(e => e.LaboratorioId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TagEquipamento>()
            .HasOne(t => t.Equipamento)
            .WithMany(e => e.Tags)      
            .HasForeignKey(t => t.EquipamentoId) 
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>()
            .HaveColumnType("timestamp without time zone");
    }
}