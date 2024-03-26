

using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebAppAspNet.ViewModels;

namespace WebAppAspNet.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
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
            using var http = new HttpClient();


            var url = $"https://localhost:7127/api/subscribers?email={viewModel.Email}";
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            var response = await http.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                viewModel.IsSubscribed = true;
            }
        }

        return View(viewModel);
    }
}
