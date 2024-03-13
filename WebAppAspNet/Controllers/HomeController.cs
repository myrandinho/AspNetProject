using Microsoft.AspNetCore.Mvc;

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
}
