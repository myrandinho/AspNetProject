

using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using WebAppAspNet.ViewModels;

namespace WebAppAspNet.Controllers;

public class HomeController(AppDbContext context, AccountService accountService) : Controller
{

    private readonly AppDbContext _context = context;
    private readonly AccountService _accountService = accountService;



    [Route("/")]
    public IActionResult Index()
    {
        if (HttpContext.Request.Cookies.TryGetValue("AccessToken", out var token))
        {

        }
        return View();
    }

    [Route("/error")]
    public IActionResult Error404(int statusCode) => View();




    [Route("/contact")]
    [HttpGet]
    public IActionResult Contact()
    {
        return View();
    }

    //[Route("/testing")]
    [HttpGet]
    public IActionResult Subscribe()
    {

        return View(new SuberscribeViewModel());
    }

    //[Route("/testing")]
    [HttpPost]
    public async Task<IActionResult> Subscribe(SuberscribeViewModel viewModel)
    {
        if (ModelState.IsValid)
        {

            try
            {
                using var http = new HttpClient();

                var content = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");
                var response = await http.PostAsync("https://localhost:7127/api/subscribers?key=OWNhNmM2NTUtZjcxMC00ODA3LTg0YTgtYzAxODc1ZWFhZGZm", content);


                if (response.IsSuccessStatusCode)
                {
                    TempData["Status"] = "Success";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["Status"] = "AlreadyExists";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    TempData["Status"] = "Unauthorized";
                }

            }
            catch
            {
                TempData["Status"] = "ConnectionFailed";
            }
        }
        else
        {
            TempData["Status"] = "Invalid";
        }

        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> Unsubscribe(string email)
    {
        var subscriber = await _accountService.GetSubscriber(email);
        if (subscriber != null)
        {
            var result = await _accountService.DeleteSubscriberFromDatabase(subscriber);
            return RedirectToAction("Index", "Home");
        }
        else
            return RedirectToAction("Error404", "Home");
    }



    //[HttpDelete("{email}")]
    //public async Task<IActionResult> Unsubscribe(string email)
    //{
    //    var subscriber = await _context.Subscribers.FirstOrDefaultAsync(x => x.Email == email);
    //    if (subscriber != null)
    //    {
    //        _context.Subscribers.Remove(subscriber);
    //        await _context.SaveChangesAsync();

    //        return Ok();
    //    }

    //    return NotFound();
    //}
}
