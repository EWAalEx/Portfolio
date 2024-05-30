using AlexPortfolio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AlexPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        [TempData]
        public string SuccessMessage { get; set; }

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        [HttpGet, Route("", Name = "root")]
        public IActionResult Index()
        {
            return View(new ContactModel
            {
                SuccessMessage = SuccessMessage
            });

        }

        [HttpPost, Route("", Name="root")]
        public IActionResult Index([FromForm] ContactModel model)
        {
            if (ModelState.IsValid)
            {
                ContactModel tempModel = model;

                SuccessMessage = "Thank you for contacting me! I'll respond soon!";

                //do something with form data

                logger.LogInformation("Contact message submitted: {Message}", model);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}