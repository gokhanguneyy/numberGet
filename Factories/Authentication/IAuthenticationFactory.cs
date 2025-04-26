using numberGet.Data.Entities;
using numberGet.Models.AuthenticationModels;
using System.Threading.Tasks;

namespace numberGet.Factories.Authentication
{
    public interface IAuthenticationFactory
    {
        Task<SignUpEntity> SignUpModelFactory(SignUpViewModel model);
        Task<string> EncryptPasswordFactory(string password);
        Task<string> DecryptPasswordFactory(string encryptPassword);
        Task<string> SignUpErrorMessageFactory(bool signUpResult);
    }
}
