using numberGet.Data.Entities;
using System.Threading.Tasks;

namespace numberGet.Services.Game
{
    public interface IGameServices
    {
        Task<SignUpEntity> GetUserById(int id);
    }
}
