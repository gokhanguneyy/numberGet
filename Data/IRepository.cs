using System.Threading.Tasks;

namespace numberGet.Data
{
    public interface IRepository<T> where T : class
    {
        Task<bool> Add(T entity);
    }
}
