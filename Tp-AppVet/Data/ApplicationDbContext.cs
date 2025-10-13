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
    }
}
