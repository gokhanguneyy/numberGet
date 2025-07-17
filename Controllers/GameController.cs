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

            int secretNumber = await _gameServices.DetermineSecretNumber(result);

            HttpContext.Session.SetInt32("SecretNumber", secretNumber);
            HttpContext.Session.SetInt32("RemainingAttempts", userTryCount);
            HttpContext.Session.SetString("SelectedLevel", beforeStartingGameModel.GameDifficultyLevel.ToString());
            HttpContext.Session.SetInt32("Score", 0);

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
                model.Message = "PLEASE ENTER A NUMBER!";
            }
            else if (model.IsGameOver)
            {
                model.Message = "GAME OVER! RESTART TO PLAY AGAIN.";
            }
            else
            {
                remainingAttempts--;

                bool isCorrect = model.UserGuess == secretNumber;
                bool isFailed = remainingAttempts == 0 && !isCorrect;


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
                    model.Message = "CONGRATULATIONS! YOU GUESSED CORRECTLY.";
                    model.IsGameOver = true;
                    score += correctPoint;
                }
                else
                {
                    if (isFailed)
                    {
                        model.Message = $"SORRY, YOU LOST! THE CORRECT NUMBER WAS: {secretNumber}";
                        model.IsGameOver = true;
                        score -= wrongPenalty;
                    }
                    else
                    {
                        model.Message = model.UserGuess < secretNumber
                            ? "GUESS A HIGHER NUMBER!"
                            : "GUESS A LOWER NUMBER!";
                        score -= wrongPenalty;
                    }
                }
            }

            HttpContext.Session.SetInt32("RemainingAttempts", remainingAttempts);
            HttpContext.Session.SetInt32("Score", score);

            model.RemainingAttempts = remainingAttempts;
            model.SelectedLevel = selectedLevel;
            model.Score = score;

            return View(model);
        }


    }
}
