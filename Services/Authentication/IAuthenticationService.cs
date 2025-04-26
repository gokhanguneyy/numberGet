using numberGet.Data.Entities;
using numberGet.Models.AuthenticationModels;
using System.Threading.Tasks;

namespace numberGet.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> SignUpAddAsync(SignUpViewModel registerModel);
        Task<bool> AnyEmail(string email);
        Task<bool> AnyNickName(string nickName);
    }
}
