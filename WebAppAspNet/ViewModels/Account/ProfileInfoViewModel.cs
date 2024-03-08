using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAppAspNet.ViewModels.Account;

public class ProfileInfoViewModel
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;


    public string Email { get; set; } = null!;
    public string ProfileImageUrl { get; set; } = "profile-image.svg";
}