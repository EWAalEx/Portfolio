using AlexPortfolio.Configuration;
using AlexPortfolio.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AlexPortfolio.Services;

public class MailService : IMailService
{
    //retrieve smtp settings from appsettings.json
    private readonly MailSettings _mailSettings;
    public MailService(IOptions<MailSettings> mailSettingsOptions)
    {
        _mailSettings = mailSettingsOptions.Value;

    }

    public bool SendMail(MailModel mailData)
    {
        try {
            //create and send email
            using (MimeMessage emailMessage = new MimeMessage())
            {
                MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                emailMessage.From.Add(emailFrom);

                MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                emailMessage.To.Add(emailTo);

                //keep incase i want it cc'd to seperate inbox
                //emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                emailMessage.Subject = mailData.EmailSubject;

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = mailData.EmailBody;

                emailMessage.Body = emailBodyBuilder.ToMessageBody();


                //send email with MailKit SmtpClient
                using (SmtpClient mailClient = new SmtpClient())
                {
                    mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                    mailClient.Send(emailMessage);
                    mailClient.Disconnect(true);
                }
            }

        } catch
        {
            return false;
        }

        return true;
    }
}