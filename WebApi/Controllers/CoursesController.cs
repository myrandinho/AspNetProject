using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Factory;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.Filters;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[UseApiKey]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOne(NewCourseRegistrationForm form)
    {
        if (ModelState.IsValid)
        {
            if (!form.CategoryName.IsNullOrEmpty())
            {
                var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == form.CategoryName);

                if (existingCategory == null)
                {
                    var categoryEntity = new CategoryEntity { CategoryName = form.CategoryName };
                    _context.Categories.Add(categoryEntity);
                    await _context.SaveChangesAsync();



                    var courseEntity = new CourseEntity
                    {
                        Title = form.Title,
                        Price = form.Price,
                        DiscountPrice = form.DiscountPrice,
                        Hours = form.Hours,
                        IsBestseller = form.IsBestseller,
                        LikesInNumbers = form.LikesInNumbers,
                        LikesInProcent = form.LikesInProcent,
                        Author = form.Author,
                        ImageUrl = form.ImageUrl,
                        CategoryId = categoryEntity.Id,

                    };


                    _context.Courses.Add(courseEntity);
                    await _context.SaveChangesAsync();


                    return Created("", (Course)courseEntity);

                }
                else
                {
                    var courseEntity = new CourseEntity
                    {
                        Title = form.Title,
                        Price = form.Price,
                        DiscountPrice = form.DiscountPrice,
                        Hours = form.Hours,
                        IsBestseller = form.IsBestseller,
                        LikesInNumbers = form.LikesInNumbers,
                        LikesInProcent = form.LikesInProcent,
                        Author = form.Author,
                        ImageUrl = form.ImageUrl,
                        CategoryId = existingCategory.Id

                    };


                    _context.Courses.Add(courseEntity);
                    await _context.SaveChangesAsync();


                    return Created("", (Course)courseEntity);
                }
                
            }
            return NotFound();

        }

        return BadRequest();
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        if (ModelState.IsValid)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course != null)
            {
                return Ok(course);
            }

            return NotFound();
        }
        return BadRequest();
    }



    [HttpGet]
    public async Task<IActionResult> GetAll(string category = "", string searchQuery = "", int pageNumber = 1, int pageSize = 9)
    {
        if (ModelState.IsValid)
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
        return BadRequest();
    }



    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, string title)
    {
        if (ModelState.IsValid)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course != null)
            {
                course.Title = title;
                _context.Courses.Update(course);
                await _context.SaveChangesAsync();

                return Ok(course);
            }

            return NotFound();
        }
        return BadRequest();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOne(int id)
    {
        if (ModelState.IsValid)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();

                return Ok();
            }

            return NotFound();
        }
        return BadRequest();
    }



}
