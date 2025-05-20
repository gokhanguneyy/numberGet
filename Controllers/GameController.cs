using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public async Task<IActionResult> Guess(BeforeStartingGameModel beforeStartingGameModel)
        {
            Enum.TryParse(typeof(GameLevelsEnum), beforeStartingGameModel.GameDifficultyLevel, true, out object result);
            var userTryCount = (int)result;

            int secretNumber = 0;
            switch ((GameLevelsEnum)result)
            {
                case GameLevelsEnum.EASY:
                    secretNumber = new Random().Next(1, 11);
                    break;
                case GameLevelsEnum.MEDIUM:
                    secretNumber = new Random().Next(1, 51);
                    break;
                case GameLevelsEnum.HARD:
                    secretNumber = new Random().Next(1, 101);
                    break;
            }

            HttpContext.Session.SetInt32("SecretNumber", secretNumber);
            HttpContext.Session.SetInt32("RemainingAttempts", userTryCount);
            HttpContext.Session.SetString("SelectedLevel", beforeStartingGameModel.GameDifficultyLevel.ToString());
            HttpContext.Session.SetInt32("Score", 0); // Puanı sıfırla

            var model = new GameModel
            {
                RemainingAttempts = userTryCount,
                SelectedLevel = beforeStartingGameModel.GameDifficultyLevel.ToString(),
                Score = 0
            };
            return View(model);
        }



        [HttpPost]
        public IActionResult Guess(GameModel model)
        {
            var secretNumber = HttpContext.Session.GetInt32("SecretNumber") ?? 0;
            var remainingAttempts = HttpContext.Session.GetInt32("RemainingAttempts") ?? 0;
            var selectedLevel = HttpContext.Session.GetString("SelectedLevel") ?? "-";
            var score = HttpContext.Session.GetInt32("Score") ?? 0;

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
                remainingAttempts--;

                bool isCorrect = model.UserGuess == secretNumber;
                bool isFailed = remainingAttempts == 0 && !isCorrect;

                // Puan hesaplama
                int correctPoint = 0;
                int wrongPenalty = 0;

                switch (selectedLevel)
                {
                    case "EASY":
                        correctPoint = 20; wrongPenalty = 2;
                        break;
                    case "MEDIUM":
                        correctPoint = 50; wrongPenalty = 5;
                        break;
                    case "HARD":
                        correctPoint = 100; wrongPenalty = 10;
                        break;
                }

                if (isCorrect)
                {
                    model.Message = "Tebrikler! Doğru tahmin ettiniz";
                    model.IsGameOver = true;
                    score += correctPoint;
                }
                else
                {
                    if (isFailed)
                    {
                        model.Message = $"Üzgünüm, kaybettiniz! Doğru sayı: {secretNumber}";
                        model.IsGameOver = true;
                        score -= wrongPenalty;
                    }
                    else
                    {
                        model.Message = model.UserGuess < secretNumber
                            ? "Daha büyük bir sayı tahmin edin!"
                            : "Daha küçük bir sayı tahmin edin!";
                        score -= wrongPenalty;
                    }
                }
            }

            // Session güncelle
            HttpContext.Session.SetInt32("RemainingAttempts", remainingAttempts);
            HttpContext.Session.SetInt32("Score", score);

            model.RemainingAttempts = remainingAttempts;
            model.SelectedLevel = selectedLevel;
            model.Score = score;

            return View(model);
        }


    }
}
