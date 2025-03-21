using Microsoft.AspNetCore.Mvc;
using numberGet.Enums;
using numberGet.Models.GameModels;
using numberGet.Models.HomePageModels;
using System;

namespace numberGet.Controllers
{
    public class HomePageController : Controller
    {
        private static int SecretNumber;
        private static int RemainingAttempts=10;
        private static GameLevelsEnum SelectedLevel;

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(BeforeStartingGameModel gameLevel)
        {
            string gameLevelValue = gameLevel.GameDifficultyLevel;
            Enum.TryParse(typeof(GameLevelsEnum), gameLevelValue, true, out object result);
            var userTryCount  = (int)result;


            return RedirectToAction("Guess");
        }

        [HttpGet]
        public IActionResult Guess()
        {
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
