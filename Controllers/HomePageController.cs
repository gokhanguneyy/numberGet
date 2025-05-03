using Microsoft.AspNetCore.Mvc;

namespace numberGet.Controllers
{
    public class HomePageController : Controller
    {
        [HttpGet]
        public IActionResult Home(string errorMessage = null)
        {
            if (errorMessage != null)
            {
                ViewData["signUpMessage"] = errorMessage;
            }
            return View();
        }

        [HttpGet]
        public IActionResult GoRegisterPage()
        {
            return RedirectToAction("SignUp", "Authentication");
        }

        [HttpPost]
        public  IActionResult FromHomeToGameLevel()
        {
            return RedirectToAction("GameLevel", "Game");
        }

        [HttpPost]
        public IActionResult PlayAsGuest()
        {
            return RedirectToAction("GameLevel", "Game");
        }
    }
}
