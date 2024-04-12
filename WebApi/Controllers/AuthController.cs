using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Filters;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AppDbContext context, IConfiguration configuration, SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IConfiguration _configuration = configuration;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;




    [UseApiKey]
    [HttpPost]
    [Route("token")]
    public IActionResult GetToken(UserLoginForm form)
    {
        if (ModelState.IsValid)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Email, form.Email),
                    new(ClaimTypes.Name, form.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(tokenString);
        }

        return Unauthorized();
    }


    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(UserLoginForm form)
    {
        if (ModelState.IsValid)
        {
            var userEntity = await _userManager.FindByEmailAsync(form.Email);
            var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password,false, false);
            if (result.Succeeded)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new(ClaimTypes.NameIdentifier, userEntity.Id.ToString()),
                        new(ClaimTypes.Email, userEntity.Email),
                        new(ClaimTypes.Name, userEntity.Email)
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);





                return Ok(new { Token = tokenString });
            }
            return BadRequest();
            
        }
        return Unauthorized();
    }
}
