using Microsoft.EntityFrameworkCore;
using Tp_AppVet.Models;

namespace Tp_AppVet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<FichaMedica> FichaMedicas { get; set; }
        public DbSet<Turno> Turnos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 Cliente → Usuario
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Veterinario → Usuario
            modelBuilder.Entity<Veterinario>()
                .HasOne(v => v.Usuario)
                .WithMany()
                .HasForeignKey(v => v.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Mascota → Cliente (uno a muchos)
            modelBuilder.Entity<Mascota>()
                .HasOne(m => m.Cliente)
                .WithMany(c => c.Mascotas)
                .HasForeignKey(m => m.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Ficha Médica → Mascota (uno a uno)
            modelBuilder.Entity<FichaMedica>()
                .HasOne(f => f.Mascota)
                .WithOne(m => m.FichaMedica)
                .HasForeignKey<FichaMedica>(f => f.MascotaId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Turno → Mascota
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Mascota)
                .WithMany(m => m.Turnos)
                .HasForeignKey(t => t.MascotaId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Turno → Cliente
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Cliente)
                .WithMany(c => c.Turnos)
                .HasForeignKey(t => t.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Turno → Veterinario
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Veterinario)
                .WithMany(v => v.Turnos)
                .HasForeignKey(t => t.VeterinarioId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
