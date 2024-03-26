using Infrastructure.Helpers;

namespace WebAppAspNet.ViewModels;

public class DeleteUserViewModel
{
    public int Id { get; set; }



    [CheckBoxRequired(ErrorMessage = "Check this!")]
    public bool deleteConfirmation { get; set; }
}
