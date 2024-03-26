

using Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace WebAppAspNet.ViewModels.Account;

public class AccountDetailsViewModel
{
    public ProfileInfoViewModel? ProfileInfo { get; set; }
    public BasicInfoFormViewModel? BasicInfoForm { get; set; }
    public AdressInfoFormViewModel? AdressInfoForm { get; set; }

    public SecurityFormViewModel? SecurityForm { get; set; }
    public DeleteUserViewModel? DeleteUser { get; set; }
    public SuberscribeViewModel? Subscribe { get; set; }


}
