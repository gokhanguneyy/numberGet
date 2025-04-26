using numberGet.Data;
using numberGet.Data.Entities;
using numberGet.Factories.SignUpFactory;
using numberGet.Models.AuthenticationModels;
using System.Threading.Tasks;

namespace numberGet.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationFactory _authenticationFactory;
        private readonly IRepository<SignUpEntity> _signUpRepository;

        public AuthenticationService(IRepository<SignUpEntity> signUpRepository, IAuthenticationFactory authenticationFactory)
        {
            _signUpRepository = signUpRepository;
            _authenticationFactory = authenticationFactory;
        }

        public async Task<string> SignUpAddAsync(SignUpViewModel signUpViewModel)
        {
            var signUpModel = await _authenticationFactory.SignUpModelFactory(signUpViewModel);
            var signUpResult = await _signUpRepository.Add(signUpModel);
            var signUpResultMessage = await _authenticationFactory.SignUpErrorMessageFactory(signUpResult);    
            return signUpResultMessage;
        }
    }
}
