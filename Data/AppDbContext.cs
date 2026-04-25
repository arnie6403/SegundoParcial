using Microsoft.EntityFrameworkCore;
using SegundoParcial.Modelos;
using SegundoParcial.Modelos;

namespace SegundoParcial.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(p => p.IdPaciente);

                entity.Property(p => p.Estado)
                      .IsRequired();

                entity.Property(p => p.MedicoResponsable)
                      .IsRequired();
            });
        }
    }
}
