

using Infrastructure.Entities;

namespace Infrastructure.Models;

public class UserCourseViewModel
{
    public string? UserId { get; set; }
    public int? CourseId { get; set; }

    public List<UserCourseEntity>? UserC { get; set; }
}
