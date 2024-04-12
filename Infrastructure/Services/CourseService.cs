

using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Infrastructure.Services;

public class CourseService(HttpClient http, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
{
    private readonly HttpClient _http = http;
    private readonly IConfiguration _configuration = configuration;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


    public async Task<CourseResult> GetCoursesAsync(string category = "", string searchQuery = "", int pageNumber = 1, int pageSize = 3)
    {
        if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("AccessToken", out var token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); //Om accesstoken finns. Körs authentisering.

            //var response = await _http.GetAsync(_configuration["ApiUris:Courses"]);
            var response = await _http.GetAsync($"{_configuration["ApiUris:Courses"]}?key={_configuration["ApiKey"]}&category={Uri.UnescapeDataString(category)}&searchQuery={Uri.UnescapeDataString(searchQuery)}&pageNumber={pageNumber}&pageSize={pageSize}");
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<CourseResult>(await response.Content.ReadAsStringAsync());
                if (result != null && result.Succeeded)
                {
                    return result;
                }
                    
            }
        }
        

        return null!;
    }
}