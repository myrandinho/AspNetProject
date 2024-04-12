using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models;

public class Course
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
    public string Category { get; set; } = null!; //nyligen tillagd.


    public static implicit operator Course(CourseEntity courseEntity)
    {
        return new Course
        {
            Id = courseEntity.Id,
            Title = courseEntity.Title,
            Price = courseEntity.Price,
            DiscountPrice = courseEntity.DiscountPrice,
            Hours = courseEntity.Hours,
            IsBestseller = courseEntity.IsBestseller,
            LikesInNumbers = courseEntity.LikesInNumbers,
            LikesInProcent = courseEntity.LikesInProcent,
            Author = courseEntity.Author,
            ImageUrl = courseEntity.ImageUrl,
            Category = courseEntity.Category.CategoryName

        };
    }
}
