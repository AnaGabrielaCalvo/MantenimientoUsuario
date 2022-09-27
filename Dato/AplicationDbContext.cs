using Microsoft.EntityFrameworkCore;
using WebApplication6.Models;

namespace WebApplication6.Dato
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = string.Format(@"Data Source=CR-LAP-ANA-CAL\ANACALVO;Initial Catalog=Babel;Persist Security Info=True;User ID=sa;Password=AmJo_378902*");
            options.UseSqlServer(connectionString);
        }

        public DbSet<Usuario> Usuario { get; set; }

    }
}
