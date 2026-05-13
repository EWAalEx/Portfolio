using AlexPortfolio.Models;
using AlexPortfolio.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlexPortfolio.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;

        public ContactController(ILogger<ContactController> logger, IMailService mailService, IConfiguration configuration, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _mailService = mailService;
            _configuration = configuration;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public IActionResult ContactForm([FromForm] ContactModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("FormError", "Home");
                }

                if (string.IsNullOrWhiteSpace(model.RecaptchaResponse))
                {
                    return RedirectToAction("FormError", "Home");
                }

                bool recaptchaResult = false;

                try
                {
                    recaptchaResult = _authenticationService.CreateGoogleCaptchaAssessment(model.RecaptchaResponse, "CONTACT_FORM");

                }
                catch (Exception e)
                {
                    // Something went wrong with the recaptcha, log the error and fail the form submit
                    _logger.LogWarning("An error occurred when evaluating the ReCaptcha result, error: {e}", e);
                    return RedirectToAction("FormError", "Home");
                }

                if (!recaptchaResult)
                {
                    _logger.LogWarning("An error occurred when evaluating the ReCaptcha result {recaptchaResult}", recaptchaResult);

                    return RedirectToAction("FormError", "Home");
                }


                MailModel mailData = new MailModel();

                mailData.EmailToName = _configuration["Email:ToName"];
                mailData.EmailToId = _configuration["Email:Email"];
                mailData.EmailSubject = "Email From Portfolio Site!";

                mailData.EmailBody = @"
                        <!DOCTYPE html>
                        <html>
                            <head>
                                <style>
                                    body {font-family: Arial, sans-serif;line-height: 1.6;color: #333;}
                                    .container {width: 100%;max-width: 600px;margin: 0 auto;padding: 20px;border: 1px solid #ddd;border-radius: 10px;background-color: #f9f9f9;}h2 {color: #4CAF50;}
                                    .info {margin-bottom: 20px;}
                                    .info strong {display: inline-block;width: 150px;}
                                </style>
                            </head>
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

                //do something with form data

                _logger.LogInformation("Contact message submitted: {Message}", model);

                _mailService.SendMail(mailData);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while submitting a Contact Form");
                ModelState.AddModelError("Recaptcha", "Something went wrong");

                return RedirectToAction("FormError", "Home");
            }

            return RedirectToAction("FormSuccess", "Home");
        }
    }
}
