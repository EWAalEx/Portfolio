using AlexPortfolio.Models;
using AlexPortfolio.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AlexPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IMailService _mailService;

        [TempData]
        public string SuccessMessage { get; set; }

        public HomeController(ILogger<HomeController> logger, IMailService _MailService)
        {
            _mailService = _MailService;
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
                MailModel mailData = new MailModel();

                mailData.EmailToName = "Alex Ellis-Wilson";
                mailData.EmailToId = "alexelliswilson@gmail.com";
                mailData.EmailSubject = "Email From Portfolio Site!";
                //mailData.EmailBody = "New Email from your portfolio site!\nEmail From: " + model.Name + "\nProvided Email: " + model.Email + "\nProvided Phone Number: " + model.Phone + "\nMessage: " + model.Message;

                mailData.EmailBody = @"<!DOCTYPE html><html><head><style>body {font-family: Arial, sans-serif;line-height: 1.6;color: #333;}.container {width: 100%;max-width: 600px;margin: 0 auto;padding: 20px;border: 1px solid #ddd;border-radius: 10px;background-color: #f9f9f9;}h2 {color: #4CAF50;}.info {margin-bottom: 20px;}.info strong {display: inline-block;width: 150px;}</style></head>
<body>
    <div class='container'>
        <h2>New Email from your Portfolio Site!</h2>
        <div class='info'>
            <strong>Email From:</strong> <span>" + model.Name + @"</span><br>
            <strong>Provided Email:</strong> <span>" + model.Email + @"</span><br>
            <strong>Provided Phone Number:</strong> <span>" + model.Phone + @"</span></div><div class='message'>
            <strong>Message:</strong><p>" + model.Message + @"</p>
        </div>
    </div>
</body>
</html>";

                SuccessMessage = "Thank you for contacting me! I'll respond soon!";

                //do something with form data

                logger.LogInformation("Contact message submitted: {Message}", model);

                _mailService.SendMail(mailData);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
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