using System.ComponentModel.DataAnnotations;

namespace WebAppAspNet.ViewModels.Account;

public class ContactFormViewModel
{
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Full name", Prompt = "Enter your full name")]
    public string FullName { get; set; } = null!;

    [Required]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Enter your email")]
    public string Email { get; set; } = null!;



    [Display(Name = "Service (optional)", Prompt = "Choose the service you are interested in")]
    public string? Service { get; set; }


    [Required]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Message", Prompt = "Enter your message here...")]
    public string Message { get; set; } = null!;
}
