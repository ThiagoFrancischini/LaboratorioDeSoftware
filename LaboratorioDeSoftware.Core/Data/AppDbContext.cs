using LaboratorioDeSoftware.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LaboratorioDeSoftware.Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Laboratorio> Laboratorios { get; set; }
    public DbSet<Produto> Produtos { get; set; }

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
    }
}
