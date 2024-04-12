using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Factory;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Filters;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[UseApiKey]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context)
    {
        _context = context;
    }

    //[HttpGet]
    //public async Task<IActionResult> GetAll() => Ok(await _context.Courses.ToListAsync());

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
        if (course != null) 
        {
            return Ok(course);
        }

        return NotFound();
    }


    //[HttpPost]
    //public async Task<IActionResult> CreateOne(CourseRegistrationForm form)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var courseEntity = new CourseEntity
    //        {
    //            Title = form.Title,
    //            Price = form.Price,
    //            DiscountPrice = form.DiscountPrice,
    //            Hours = form.Hours,
    //            IsBestseller = form.IsBestseller,
    //            LikesInNumbers = form.LikesInNumbers,
    //            LikesInProcent = form.LikesInProcent,
    //            Author = form.Author,
    //            ImageUrl = form.ImageUrl,

    //        };


    //        _context.Courses.Add(courseEntity);
    //        await _context.SaveChangesAsync();


    //        return Created("", (Course)courseEntity);
    //    }

    //    return BadRequest();
    //}

    //[HttpPost]
    //public async Task<IActionResult> CreateOne(Course course, string categoryName)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {



    //            var categoryEntity = new Category
    //            {
    //                CategoryName = categoryName
    //            };

    //            var courseEntity = new CourseEntity
    //            {
    //                Id = course.Id,
    //                Title = course.Title,
    //                Author = course.Author,
    //                Price = course.Price,
    //                DiscountPrice = course.DiscountPrice,
    //                Hours = course.Hours,
    //                IsBestseller = course.IsBestseller,
    //                LikesInProcent = course.LikesInProcent,
    //                LikesInNumbers = course.LikesInNumbers,
    //                ImageUrl = course.ImageUrl,
    //                CategoryId = categoryEntity.Id,


    //            };

    //            _context.Courses.Add(courseEntity);
    //            await _context.SaveChangesAsync();
    //            return Created("", courseEntity);


    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ex);
    //        }
    //    }
    //    return BadRequest();
    //}

    
    [HttpGet]
    public async Task<IActionResult> GetAll(string category = "", string searchQuery = "", int pageNumber = 1, int pageSize = 3)
    {
        var query = _context.Courses.Include(i => i.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(category) && category != "all")
            query = query.Where(x => x.Category!.CategoryName == category);


        if (!string.IsNullOrEmpty(searchQuery))
            query = query.Where(x => x.Title.Contains(searchQuery) || x.Author.Contains(searchQuery));

        query = query.OrderByDescending(o => o.LastUpdated);

        var courses = await query.ToListAsync();

        var response = new CourseResult
        {
            Succeeded = true,
            TotalItems = await query.CountAsync()
        };
        response.TotalPages = (int)Math.Ceiling(response.TotalItems / (double)pageSize);
        response.Courses = CourseFactory.Create(await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync());



        return Ok(response);
    }
}
