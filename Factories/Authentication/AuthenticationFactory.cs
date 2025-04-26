using numberGet.Data.Entities;
using numberGet.Models.AuthenticationModels;
using System;
using System.Threading.Tasks;

namespace numberGet.Factories.SignUpFactory
{
    public class AuthenticationFactory : IAuthenticationFactory
    {
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
            if (model == null)
            {
                var emptyModel = new SignUpEntity();
                return await Task.FromResult(emptyModel);
            }

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
    }
}
