using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAppAspNet.Controllers
{
    public class SubscribersController : Controller
    {
        [Route("/testing")]
        [HttpGet]
        public IActionResult Subscribe()
        {
            
            return View(new SuberscribeViewModel());
        }

        [Route("/testing")]
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
}
