using Microsoft.EntityFrameworkCore;
using WebApiNet.Core.Entities;

namespace WebApiNet.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Alquiler> Alquileres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar relaciones explícitamente
            modelBuilder.Entity<Alquiler>()
                .HasOne(a => a.Cliente)
                .WithMany(c => c.Alquileres)
                .HasForeignKey(a => a.ClienteDni)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Alquiler>()
                .HasOne(a => a.Vehiculo)
                .WithMany(v => v.Alquileres)
                .HasForeignKey(a => a.VehiculoMatricula)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}