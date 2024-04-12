

using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;

namespace Infrastructure.Services;

public class AccountService
{
    private readonly AppDbContext _context;
    private readonly UserManager<UserEntity> _userManager;
    private readonly IConfiguration _configuration;




    public AccountService(AppDbContext context, UserManager<UserEntity> userManager, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<bool> UploadUserProfileImageAsync(ClaimsPrincipal user, IFormFile file)
    {
        try
        {


            if (user != null && file != null && file.Length != 0)
            {
                var userEntity = await _userManager.GetUserAsync(user);
                if (userEntity != null)
                {
                    var fileName = $"p_{userEntity.Id}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), _configuration["FileUploadPath"]!, fileName);

                    using var fs = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(fs);

                    userEntity.ProfileImage = fileName;
                    _context.Update(userEntity);
                    await _context.SaveChangesAsync();

                    return true;


                }
            }


        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return false;
    }


    public async Task<bool> DeleteSubscriberFromDatabase(SubscriberEntity entity)
    {
       
        
        
          _context.Subscribers.Remove(entity);
        await _context.SaveChangesAsync();
        return true;

       


        
    }

    public async Task<SubscriberEntity> GetSubscriber(string email)
    {
        var subscriber = await _context.Subscribers.FirstOrDefaultAsync(x => x.Email == email);
        if (subscriber != null)
        {
            return subscriber;
        }
        else
        {
            return null!;
        }
    }

    

   
}
