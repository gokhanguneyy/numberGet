using numberGet.Data.Entities;
using numberGet.Models.AuthenticationModels;
using System.Threading.Tasks;

namespace numberGet.Factories.Authentication
{
    public interface IAuthenticationFactory
    {
        Task<SignUpEntity> SignUpModelFactory(SignUpViewModel model);
        Task<string> EncryptPasswordFactory(string password);
        Task<bool> DecryptPasswordFactory(string signInPassword, string encryptPassword);
        Task<string> SignUpErrorMessageFactory(bool signUpResult);
    }
}
