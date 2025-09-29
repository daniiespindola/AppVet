using Microsoft.EntityFrameworkCore;
using Tp_AppVet.Models;

namespace Tp_AppVet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
        public DbSet<Cliente> Clientes { get; set; }
    }
}
