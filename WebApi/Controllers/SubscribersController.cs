
using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Filters;


namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscribersController : ControllerBase
{
    private readonly AppDbContext _context;

    public SubscribersController(AppDbContext context)
    {
        _context = context;
    }

    


    #region READ
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var subscribers = await _context.Subscribers.ToListAsync();
        if (subscribers.Count != 0)
        {
            return Ok(subscribers);
        }

        return NotFound();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        if (ModelState.IsValid)
        {
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(x => x.Id == id);
            if (subscriber != null)
            {
                return Ok(subscriber);
            }

            return NotFound();
        }
        return BadRequest();

    }
    #endregion

    #region UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, string email)
    {
        if (ModelState.IsValid)
        {
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(x => x.Id == id);
            if (subscriber != null)
            {
                subscriber.Email = email;
                _context.Subscribers.Update(subscriber);
                await _context.SaveChangesAsync();

                return Ok(subscriber);
            }

            return NotFound();
        }
        return BadRequest();
    }
    #endregion

    #region DELETE
    [UseApiKey]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOne(int id)
    {
        if (ModelState.IsValid)
        {
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(x => x.Id == id);
            if (subscriber != null)
            {
                _context.Subscribers.Remove(subscriber);
                await _context.SaveChangesAsync();

                return Ok();
            }

            return NotFound();
        }
        return BadRequest();
    }
    #endregion


    [UseApiKey]
    [HttpPost]
    public async Task<IActionResult> Subscribe(Subscriber dto)
    {
        if (ModelState.IsValid)
        {
            if (! await _context.Subscribers.AnyAsync(x => x.Email == dto.Email))
            {
                _context.Subscribers.Add(dto);
                await _context.SaveChangesAsync();
                return Created("", null);
            }
            else
            {
                return Conflict();
            }
        }

        return BadRequest();
    }
}

