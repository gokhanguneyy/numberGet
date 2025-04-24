using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
        private readonly IRepository<SignUpEntity> _signUpEntity;

        public HomePageController(IRepository<GuneyPersonEntity> guneyPersonRepository, IRepository<SignUpEntity> signUpEntity)
        {
            _guneyPersonRepository = guneyPersonRepository;
            _signUpEntity = signUpEntity;
        }

        [HttpGet]
        public IActionResult Home(string errorMessage = null)
        {
            if (errorMessage != null)
            {
                ViewData["signUpMessage"] = errorMessage;
            }
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

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            var hashedConfirmPassword = BCrypt.Net.BCrypt.HashPassword(model.ConfirmPassword);


            var registerModel = new SignUpEntity
            {
                Name = model.Name,
                Surname = model.Surname,
                UserName = model.UserName,
                Email = model.Email,
                Password = hashedPassword,
                ConfirmPassword = hashedConfirmPassword,
                CreatedTime = DateTime.Now,
            };

            var result = await _signUpEntity.Add(registerModel);
            if(!result)
                return View(model);
            // bir tane hata sayfası oluştur   

            return RedirectToAction("Home", new { errorMessage = "Kayıt başarısız!" });
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
