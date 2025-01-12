using GameServer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GameServer.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) 
        { 
        }
    }
}
