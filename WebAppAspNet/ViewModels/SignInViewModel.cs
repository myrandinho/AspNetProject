using System.ComponentModel.DataAnnotations;

namespace WebAppAspNet.ViewModels;

public class SignInViewModel
{
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email name", Prompt = "Your email")]
    [Required]
    public string Email { get; set; } = null!;



    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Your password")]
    [Required]
    public string Password { get; set; } = null!;


    public bool RememberMe { get; set; } 
}
