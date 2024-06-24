using AlexPortfolio.Models;
using AlexPortfolio.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlexPortfolio.Controllers
{
    [ApiController]
    public class MailController : Controller
    {
        private readonly IMailService _mailService;
        //injecting the IMailService into the constructor
        public MailController(IMailService _MailService)
        {
            _mailService = _MailService;
        }

        [HttpPost]
        [Route("SendMail")]
        public bool SendMail(MailModel mailData)
        {
            return _mailService.SendMail(mailData);
        }
    }
}
