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

        public async Task<bool> AnyEmail(string email)
        {
            var anyEmail = await _signUpRepository.AnyAsync(x=>x.Email == email);
            return anyEmail;
        }
        public async Task<bool> AnyNickName(string nickName)
        {
            var anyNickName = await _signUpRepository.AnyAsync(x=>x.UserName == nickName);
            return anyNickName;
        }

        public async Task<string> SignUpAddAsync(SignUpViewModel signUpViewModel)
        {
            var signUpModel = await _authenticationFactory.SignUpModelFactory(signUpViewModel);
            if(signUpModel != null)
            {
                var signUpResult = await _signUpRepository.Add(signUpModel);
                var signUpResultMessage = await _authenticationFactory.SignUpErrorMessageFactory(signUpResult);
                return signUpResultMessage;
            }
            else
            {
                return "LÜTFEN KENDİ VERİLERİNİZİ GİRİNİZ";
            }

        }
    }
}
