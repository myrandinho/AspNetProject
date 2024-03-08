using System.ComponentModel.DataAnnotations;

namespace WebAppAspNet.ViewModels.Account;

public class AdressInfoFormViewModel
{
    [Required(ErrorMessage = "A valid adress line is required")]
    [DataType(DataType.Text)]
    [Display(Name = "Adress line 1", Prompt = "Enter your first adress line")]
    public string AdressLine_1 { get; set; } = null!;

    
    [DataType(DataType.Text)]
    [Display(Name = "Adress line 2", Prompt = "Enter your second adress line")]
    public string? AdressLine_2 { get; set; }

    [Required(ErrorMessage = "A valid postal code is required")]
    [DataType(DataType.Text)]
    [Display(Name = "Postal code", Prompt = "Enter your postal code")]
    public string PostalCode { get; set; } = null!;

    [Required(ErrorMessage = "A valid city is required")]
    [DataType(DataType.Text)]
    [Display(Name = "City", Prompt = "Enter your city")]
    public string City { get; set; } = null!;
}
