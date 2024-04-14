

using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities;

public class UserEntity : IdentityUser
{
    [ProtectedPersonalData]
    public string FirstName { get; set; } = null!;

    [ProtectedPersonalData]
    public string LastName { get; set; } = null!;

    [ProtectedPersonalData]
    public string? Bio { get; set; }

    public int? AdressId { get; set; }
    public ICollection<AdressEntity> Adresses { get; set; } = [];

    public bool IsExternalAccount { get; set; } = false;

    public string? ProfileImage { get; set; } = "avatar.jpg";


    public ICollection<UserCourseEntity>? UserCourses { get; set; }
}
