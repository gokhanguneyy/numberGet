using Microsoft.EntityFrameworkCore;
using numberGet.Context;
using System.Threading.Tasks;

namespace numberGet.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<bool> Add(T entity)
        {
            _dbSet.Add(entity);
            var result = await _context.SaveChangesAsync();
            if(result <=0)
                return false;
            return true;




            // bu kontrolü servis katmanında yapmamız gerekir diye düşünüyorum.
            /*
            if(entity != null)
            {
                _dbSet.Add(entity);
                var result = await _context.SaveChangesAsync();
                if(result <= 0)
                {
                    return false;   
                }
                return true;
            }
            return false;
            */
        }
    }
}
