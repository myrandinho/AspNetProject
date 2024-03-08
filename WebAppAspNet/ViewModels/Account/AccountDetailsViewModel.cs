

using System.ComponentModel.DataAnnotations;

namespace WebAppAspNet.ViewModels.Account;

public class AccountDetailsViewModel
{
    public ProfileInfoViewModel? ProfileInfo { get; set; }
    public BasicInfoFormViewModel? BasicInfoForm { get; set; }
    public AdressInfoFormViewModel? AdressInfoForm { get; set; }

}
