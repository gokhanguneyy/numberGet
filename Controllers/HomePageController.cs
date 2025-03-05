using Microsoft.AspNetCore.Mvc;
using numberGet.Models;
using System.Diagnostics;

namespace numberGet.Controllers
{
    public class HomePageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
