using System.ComponentModel.DataAnnotations;

namespace WebAppAspNet.ViewModels.Account;

public class SecurityFormViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Current password")]
    public string CurrentPassword { get; set; } = null!;



    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string NewPassword { get; set; } = null!;



    [DataType(DataType.Password)]
    [Display(Name = "Confirm new password")]
    [Compare("NewPassword", ErrorMessage =
        "The new password and confirmation password do not match.")]
    public string ConfirmedPassword { get; set; } = null!;
    
}
