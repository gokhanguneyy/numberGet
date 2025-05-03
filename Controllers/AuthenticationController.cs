using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using numberGet.Services.Authentication;
using numberGet.Models.AuthenticationModels;
using numberGet.Factories.Authentication;
using numberGet.Data.Entities;
using numberGet.Data;

namespace numberGet.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationServices _authenticationService;
        private readonly IAuthenticationFactory _authenticationFactory;

        public AuthenticationController(IAuthenticationServices authenticationService, IAuthenticationFactory authenticationFactory)
        {
            _authenticationService = authenticationService;
            _authenticationFactory = authenticationFactory;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var signUpResult = await _authenticationService.SignUpAddAsync(model);
            return RedirectToAction("Home", "HomePage", new { errorMessage = signUpResult });
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModels signInModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _authenticationService.GetUserByEmail(signInModel.Email);
                if (user == null)
                {
                    return RedirectToAction("Home", "HomePage", new { errorMessage = "KULLANICI BULUNAMADI" });
                }
                else
                {
                    return await CheckPassword(signInModel, user);
                }
            }
            return RedirectToAction("Home", "HomePage", new { errorMessage = "LÜTFEN BİLGİLERİNİZİ KONTROL EDİNİZ" });
        }

        private async Task<IActionResult> CheckPassword(SignInModels signInModel, SignUpEntity user)
        {
            bool result = await _authenticationFactory.DecryptPasswordFactory(signInModel.Password, user.Password);
            if (result)
            {
                return RedirectToAction("GameLevel", "Game", new {userId = user.Id}); 
            }
            else
            {
                return RedirectToAction("Home", "HomePage", new { errorMessage = "GİRİŞ BAŞARISIZ" });
            }
        }
    }
}
