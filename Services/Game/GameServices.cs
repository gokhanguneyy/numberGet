using numberGet.Data;
using numberGet.Data.Entities;
using System.Threading.Tasks;

namespace numberGet.Services.Game
{
    public class GameServices : IGameServices
    {
        private readonly IRepository<SignUpEntity> _signUpRepository;

        public GameServices(IRepository<SignUpEntity> signUpRepository)
        {
            _signUpRepository = signUpRepository;
        }

        public Task<SignUpEntity> GetUserById(int id)
        {
            if(id == 0) return null;
            var users = _signUpRepository.GetUserById(id);
            return users == null ? null : users;
        }
    }
}
