using Microsoft.EntityFrameworkCore;
using numberGet.Models;

namespace numberGet.Context
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Todo>  Todos  { get; set; }
    }
}
