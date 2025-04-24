using Microsoft.AspNetCore.Mvc;
using numberGet.Data;
using numberGet.Data.Entities;
using numberGet.Enums;
using numberGet.Models;
using numberGet.Models.GameModels;
using numberGet.Models.HomePageModels;
using System;
using System.Threading.Tasks;

namespace numberGet.Controllers
{
    public class HomePageController : Controller
    {
        private readonly IRepository<GuneyPersonEntity> _guneyPersonRepository;

        public HomePageController(IRepository<GuneyPersonEntity> guneyPersonRepository)
        {
            _guneyPersonRepository = guneyPersonRepository;
        }

        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }

        [HttpPost]
        public  async Task<IActionResult> FromHomeToGameLevel()
        {
            return RedirectToAction("GameLevel", "HomePage");
        }

        [HttpGet]
        public async Task<IActionResult> FromHomeToRegisterPage()
        {
            return RedirectToAction("SignUp", "HomePage");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return RedirectToAction("Home");
        }



        private static int SecretNumber;
        private static int RemainingAttempts;
        private static GameLevelsEnum SelectedLevel;

        [HttpGet]
        public IActionResult GameLevel()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GameLevel(BeforeStartingGameModel gameLevel)
        {
            string gameLevelValue = gameLevel.GameDifficultyLevel;
            Enum.TryParse(typeof(GameLevelsEnum), gameLevelValue, true, out object result);
            var userTryCount  = (int)result;


            return RedirectToAction("Guess");
        }

        [HttpGet]
        public IActionResult Guess()
        {
            RemainingAttempts = 10;
            var model = new GameModel
            {
                RemainingAttempts = RemainingAttempts,
                SelectedLevel = SelectedLevel
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Guess(GameModel model)
        {
            if (model.UserGuess == null)
            {
                model.Message = "Lütfen bir sayı giriniz!";
            }
            else if (model.IsGameOver)
            {
                model.Message = "Oyun bitti! Tekrar oynamak için yeniden başlatın.";
            }
            else
            {
                RemainingAttempts--;

                if (model.UserGuess < SecretNumber)
                {
                    model.Message = "Daha büyük bir sayı tahmin edin!";
                }
                else if (model.UserGuess > SecretNumber)
                {
                    model.Message = "Daha küçük bir sayı tahmin edin!";
                }
                else
                {
                    model.Message = "Tebrikler! Doğru tahmin ettiniz";
                    model.IsGameOver = true;
                }

                if (RemainingAttempts == 0 && model.UserGuess != SecretNumber)
                {
                    model.Message = $"Üzgünüm, kaybettiniz! Doğru sayı: {SecretNumber}";
                    model.IsGameOver = true;
                }
            }

            model.RemainingAttempts = RemainingAttempts;
            model.SelectedLevel = SelectedLevel;
            return View(model);
        }

    }
}
