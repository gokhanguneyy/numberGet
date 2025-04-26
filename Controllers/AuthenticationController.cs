using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using numberGet.Services.Authentication;
using numberGet.Models.AuthenticationModels;

namespace numberGet.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationServices _authenticationService;

        public AuthenticationController(IAuthenticationServices authenticationService)
        {
            _authenticationService = authenticationService;
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
            return RedirectToAction("Home", "HomePage", new { signUpResult });
        }
    }
}
