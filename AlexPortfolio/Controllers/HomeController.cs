using AlexPortfolio.Models;
using AlexPortfolio.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AlexPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        [TempData]
        public string SuccessMessage { get; set; } = "";

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet, Route("", Name = "root")]
        public IActionResult Index()
        {
            ViewData["RecaptchaSiteKey"] = _configuration["Recaptcha:SiteKey"];
            return View(new ContactModel
            {
                SuccessMessage = SuccessMessage
            });

        }

        [HttpGet, Route("/FormSuccess", Name = "FormSuccess")]
        public IActionResult FormSuccess()
        {
            SuccessMessage = "Thank you for contacting me! I'll respond soon!";

            return RedirectToAction(nameof(Index));

        }

        [HttpGet, Route("/FormError", Name = "FormError")]
        public IActionResult FormError()
        {
            SuccessMessage = "Your form didn't submit, please check your details and try again later!";

            return RedirectToAction(nameof(Index));

        }

        [HttpGet, Route("/privacy", Name = "privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet, Route("/error", Name = "error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}