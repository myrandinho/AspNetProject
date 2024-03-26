

using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Dtos;

public class CourseRegistrationForm
{
    [Required]
    public string Title { get; set; } = null!;
    public string? Price { get; set; }
    public string? DiscountPrice { get; set; }
    public string? Hours { get; set; }
    public bool IsBestseller { get; set; } = false;
    public string? LikesInNumbers { get; set; }
    public string? LikesInProcent { get; set; }
    public string? Author { get; set; }
}
