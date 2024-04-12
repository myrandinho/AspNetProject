
namespace Infrastructure.Models;

public class CourseIndexViewModel
{
    public IEnumerable<Course>? Courses { get; set; }
    public IEnumerable<Category>? Categories { get; set; }
    public Pagination? Pagination { get; set; }
}
