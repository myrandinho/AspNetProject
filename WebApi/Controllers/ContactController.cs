using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Filters;
using WebAppAspNet.ViewModels.Account;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;



    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var contacs = await _context.ContactForm.OrderBy(x => x.FullName).ToListAsync();
        if(contacs.Any())
            return Ok(contacs);

        return BadRequest("The list is empty");


    }

    [UseApiKey]
    [HttpPost]
    public async Task<IActionResult> PostForm(ContactFormViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            if (!await _context.ContactForm.AnyAsync(x => x.Email == viewModel.Email))
            {
                var contactFormEntity = new ContactFormEntity
                {
                    FullName = viewModel.FullName,
                    Email = viewModel.Email,
                    Service = viewModel.Service,
                    Message = viewModel.Message,
                };


                _context.ContactForm.Add(contactFormEntity);
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
