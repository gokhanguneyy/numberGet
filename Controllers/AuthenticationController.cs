using Microsoft.AspNetCore.Mvc;
using numberGet.Data.Entities;
using System.Threading.Tasks;
using numberGet.Data;
using numberGet.Factories.SignUpFactory;
using numberGet.Services.Authentication;
using numberGet.Models.AuthenticationModels;

namespace numberGet.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IRepository<SignUpEntity> _signUpEntity;
        private readonly IAuthenticationFactory _signUpFactory;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IRepository<SignUpEntity> signUpEntity, IAuthenticationFactory signUpFactory, IAuthenticationService authenticationService)
        {
            _signUpEntity = signUpEntity;
            _signUpFactory = signUpFactory;
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
