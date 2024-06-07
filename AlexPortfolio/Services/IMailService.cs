using AlexPortfolio.Models;

namespace AlexPortfolio.Services
{
    //interface for email sending
    public interface IMailService
    {
        bool SendMail(MailModel mailData);
    }
}
