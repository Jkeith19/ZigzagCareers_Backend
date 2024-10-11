using Data.Entity.Model;
using Microsoft.EntityFrameworkCore;

namespace Data.Entity.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<Book> Books { get; set; }
    }
}
