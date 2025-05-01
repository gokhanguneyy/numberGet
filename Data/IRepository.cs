using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace numberGet.Data
{
    public interface IRepository<T> where T : class
    {
        Task<bool> Add(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task<T> GetUserByExpressionAsync(Expression<Func<T, bool>> expression);
    }
}
