using Microsoft.Extensions.DependencyInjection;
using numberGet.Data.Entities;
using numberGet.Models.AuthenticationModels;
using numberGet.Services.Authentication;
using System;
using System.Threading.Tasks;

namespace numberGet.Factories.Authentication
{
    public class AuthenticationFactory : IAuthenticationFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AuthenticationFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
                return "REGISTRATION SUCCESSFUL. PLEASE LOG IN";
            else
                return "REGISTRATION FAILED. PLEASE TRY AGAIN";
        }

        public async Task<SignUpEntity> SignUpModelFactory(SignUpViewModel model)
        {
            var authenticationService = _serviceProvider.GetRequiredService<IAuthenticationServices>();
            var anyEmail = await authenticationService.AnyEmail(model.Email);
            var anyNickName = await authenticationService.AnyNickName(model.UserName);
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
                return null;
            }           
        }
    }
}
