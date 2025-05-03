using Microsoft.EntityFrameworkCore;
using numberGet.Context;
using System;
using System.Linq.Expressions;
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
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public async Task<T> GetUserByExpressionAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.SingleOrDefaultAsync(expression);
        }

        public async Task<T> GetUserById(int id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}
