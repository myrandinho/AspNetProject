

using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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


    public async Task<bool> SaveCourseToUser(CourseEntity course, string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return false;
        }

        var courseCheck = await _context.Courses.FindAsync(course.Id);
        if (courseCheck == null)
        {
            
            return false;
        }

        var existingUserCourse = await _context.UserCourses.FindAsync(userId, course.Id);
        if (existingUserCourse != null)
        {
            return false;
        }

        var userCourse = new UserCourseEntity
        {
            UserId = userId,
            CourseId = course.Id,
        };

        _context.UserCourses.Add(userCourse);

        await _context.SaveChangesAsync();

        return true;

    }

    public async Task<CourseEntity> GetCourseByIdAsync(int courseId)
    {

        var result = await _context.Courses.FindAsync(courseId);
        if (result != null)
        {
            return result;
        }

        return null!;
    }

    public async Task<UserCourseEntity> GetUserCourseByIdAsync(string userId, int courseId)
    {

        var result = await _context.UserCourses.FindAsync(userId, courseId);
        if (result != null)
        {
            return result;
        }

        return null!;
    }

    public async Task<UserCourseEntity> GetUserCourse(string UserId)
    {
        var result = await _context.UserCourses.FindAsync(UserId);
        if (result != null)
        {
            return result;
        }
        return null!;
    }

    public async Task<List<UserCourseEntity>> GetUserCourses(string userId)
    {
        var userCourses = await _context.UserCourses
            .Where(uc => uc.UserId == userId)
            .ToListAsync();

        return userCourses;
    }


    public List<CourseEntity> GetUserCoursesAsList(string userId)
    {
        // Hämta alla kurser för den angivna användaren från UserCourse-tabellen
        var userCourses = _context.UserCourses
            .Where(uc => uc.UserId == userId)
            .Include(uc => uc.Course) // Inkludera kursdata
            .Select(uc => uc.Course)
            .ToList();

        return userCourses;
    }

    public async Task<bool> DeleteUserSavedCourse(string Id, int courseId)
    {
        var userCourse = await _context.UserCourses.FindAsync(Id, courseId);
        if (userCourse != null)
        {
            _context.UserCourses.Remove(userCourse);

            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> DeleteUserCourses(string userId)
    {
        var userCourses = await _context.UserCourses.Where(uc => uc.UserId == userId).ToListAsync();

        if (userCourses != null && userCourses.Any())
        {
            _context.UserCourses.RemoveRange(userCourses);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }



}
