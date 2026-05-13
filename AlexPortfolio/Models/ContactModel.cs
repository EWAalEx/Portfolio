using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AlexPortfolio.Models
{
    public record ContactModel : IContactModel
    {
        [Required, Display(Name = "Full Name")]
        public string Name { get; init; }
        [Required, Display(Name = "Phone")]
        public string Phone { get; init; }
        [Required, EmailAddress, Display(Name = "Email Address")]
        public string Email { get; init; }
        [Required, Display(Name = "Message")]
        public string Message { get; init; }
        public bool HasSuccessMessage => 
            SuccessMessage is not null;
        
        public string? SuccessMessage { get; init; }

        [Required(ErrorMessage = "Recaptcha must be completed to submit")]
        [FromForm(Name = "g-recaptcha-response")]
        public string? RecaptchaResponse { get; set; }
    }
}