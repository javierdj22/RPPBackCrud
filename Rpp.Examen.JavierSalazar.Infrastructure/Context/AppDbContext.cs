// Infrastructure/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using Rpp.Examen.JavierSalazar.Domain.Entities;

namespace Rpp.Examen.JavierSalazar.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Persona> Personas { get; set; } = null!;
        public DbSet<Trabajador> Trabajador { get; set; } = null!;
        public DbSet<Hijo> Hijos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tablas
            modelBuilder.Entity<Persona>().ToTable("Persona");
            modelBuilder.Entity<Trabajador>().ToTable("Trabajador");
            modelBuilder.Entity<Hijo>().ToTable("Hijos");

            // Relaciones
            modelBuilder.Entity<Trabajador>()
                .HasOne(t => t.Persona)
                .WithMany(p => p.Trabajadores)
                .HasForeignKey(t => t.IdPersona);

            modelBuilder.Entity<Hijo>()
                .HasOne(h => h.Trabajador)
                .WithMany(t => t.Hijos)
                .HasForeignKey(h => h.IdTrabajador);

            modelBuilder.Entity<Hijo>()
                .HasOne(h => h.Persona)
                .WithMany(p => p.Hijos)
                .HasForeignKey(h => h.IdHijo);
        }
    }
}
