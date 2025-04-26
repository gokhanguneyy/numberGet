using numberGet.Data.Entities;
using numberGet.Models.AuthenticationModels;
using numberGet.Services.Authentication;
using System;
using System.Threading.Tasks;

namespace numberGet.Factories.SignUpFactory
{
    public class AuthenticationFactory : IAuthenticationFactory
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationFactory(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public Task<string> DecryptPasswordFactory(string encryptPassword)
        {
            throw new NotImplementedException();
        }

        public Task<string> EncryptPasswordFactory(string password)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return Task.FromResult(hashedPassword);
        }

        public async Task<string> SignUpErrorMessageFactory(bool signUpResult)
        {
            if (signUpResult)
                return "KAYIT BAŞARILI. LÜTFEN GİRİŞ YAPINIZ";
            else
                return "KAYIT BAŞARISIZ. LÜTFEN TEKRAR DENEYİNİZ";
        }

        public async Task<SignUpEntity> SignUpModelFactory(SignUpViewModel model)
        {
            var anyEmail = await _authenticationService.AnyEmail(model.Email);
            var anyNickName = await _authenticationService.AnyNickName(model.UserName);
            if((anyEmail == false)&&(anyNickName == false))
            {
                var hashedPassword = await EncryptPasswordFactory(model.Password);
                var hashedConfirmPassword = await EncryptPasswordFactory(model.ConfirmPassword);

                var registerModel = new SignUpEntity
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = hashedPassword,
                    ConfirmPassword = hashedConfirmPassword,
                    CreatedTime = DateTime.Now,
                };

                return await Task.FromResult(registerModel);
            }
            else
            {
                var emptyModel = new SignUpEntity();
                return await Task.FromResult(emptyModel);
            }           
        }
    }
}
