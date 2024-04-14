

using Infrastructure.Dtos;
using Infrastructure.Models;

namespace Infrastructure.Entities;

public class CourseEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Price { get; set; }
    public string? DiscountPrice { get; set; }
    public string? Hours { get; set; }
    public bool IsBestseller { get; set; }
    public string? LikesInNumbers { get; set; }
    public string? LikesInProcent { get; set; }
    public string? Author { get; set; }

    public string? ImageUrl { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }

    public int? CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }


    public ICollection<UserCourseEntity>? UserCourses { get; set; }
}
