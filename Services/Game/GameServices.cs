using numberGet.Data;
using numberGet.Data.Entities;
using numberGet.Enums;
using System;
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

        public async Task<int> DetermineSecretNumber(object result)
        {
            int secretNumber = 0;
            switch ((GameLevelsEnum)result)
            {
                case GameLevelsEnum.EASY:
                    secretNumber = new Random().Next(1, 11);
                    break;
                case GameLevelsEnum.MEDIUM:
                    secretNumber = new Random().Next(1, 51);
                    break;
                case GameLevelsEnum.HARD:
                    secretNumber = new Random().Next(1, 101);
                    break;
                default:
                    secretNumber = 0;
                    break;
            }
            return secretNumber;
        }

        public Task<SignUpEntity> GetUserById(int id)
        {
            if(id == 0) return null;
            var users = _signUpRepository.GetUserById(id);
            return users == null ? null : users;
        }
    }
}
