using Microsoft.EntityFrameworkCore;
using numberGet.Data.Entities;
using numberGet.Models;

namespace numberGet.Context
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Todo>  Todos  { get; set; }
        public DbSet<GuneyPersonEntity> GuneyPerson {  get; set; }
    }
}
