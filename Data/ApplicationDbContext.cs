using Microsoft.EntityFrameworkCore;
using DataVisionAPI.Models;

namespace DataVisionAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Usuario_)
                      .HasColumnName("Usuario")
                      .HasMaxLength(50)
                      .IsRequired();
                entity.HasIndex(e => e.Usuario_).IsUnique();
                entity.Property(e => e.Password)
                      .HasMaxLength(255)
                      .IsRequired();
                entity.Property(e => e.Rol)
                      .HasMaxLength(50)
                      .IsRequired();
            });

            // Configuración de Log
            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FechaConsulta)
                      .HasDefaultValueSql("GETDATE()")
                      .IsRequired();
                entity.Property(e => e.EndpointConsultado)
                      .HasMaxLength(255)
                      .IsRequired();

                // Relación con Usuario
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Logs)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Datos iniciales (opcional)
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Usuario administrador por defecto - Contraseñas hasheadas con BCrypt
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Usuario_ = "admin",
                    Password = "$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewpYYGiF1XQvCgOy", // admin123 hasheada
                    Rol = "Admin"
                },
                new Usuario
                {
                    Id = 2,
                    Usuario_ = "user",
                    Password = "$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewpYYGiF1XQvCgOy", // user123 hasheada
                    Rol = "User"
                }
            );
        }
    }
}