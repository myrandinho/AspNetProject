using Infrastructure.Contexts;
using Infrastructure.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;



    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _context.Categories.OrderBy(o => o.CategoryName).ToListAsync();
        return Ok(CategoryFactory.Create(categories));
    }
}
