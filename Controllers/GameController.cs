using Microsoft.AspNetCore.Mvc;
using numberGet.Enums;
using numberGet.Models.GameModels;
using numberGet.Models.HomePageModels;
using numberGet.Services.Game;
using System;
using System.Threading.Tasks;

namespace numberGet.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameServices _gameServices;

        public GameController(IGameServices gameServices)
        {
            _gameServices = gameServices;
        }

        [HttpGet]
        public async Task<IActionResult> GameLevel(int? userId = null)
        {
            var model = new BeforeStartingGameModel();  
            if(userId == null)
            {
                model.UserId = 0;
                model.UserNickName = "GUEST";
            }
            else
            {
                model.UserId = userId ?? 0;
                var users = await _gameServices.GetUserById(userId  ?? 0);
                model.UserNickName = users.UserName;
            }
            return View(model);
        }


        private static int SecretNumber;
        private static int RemainingAttempts;

        [HttpGet]
        public async Task<IActionResult> Guess(BeforeStartingGameModel beforeStartingGameModel)
        {
            Enum.TryParse(typeof(GameLevelsEnum), beforeStartingGameModel.GameDifficultyLevel, true, out object result);
            var userTryCount = (int)result;

            RemainingAttempts = userTryCount;
            var model = new GameModel
            {
                RemainingAttempts = RemainingAttempts,
                SelectedLevel = beforeStartingGameModel.GameDifficultyLevel.ToString()
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
            return View(model);
        }
    }
}
