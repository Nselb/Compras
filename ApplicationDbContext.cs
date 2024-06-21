using Compras.Models;
using Microsoft.EntityFrameworkCore;

namespace Compras
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
