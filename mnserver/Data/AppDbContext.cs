using Microsoft.EntityFrameworkCore;
using mnserver.Models;

namespace mnserver.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base((options)) { }
        public DbSet<Client> Client { get; set; }
    }
}
