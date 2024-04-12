using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAppAspNet.Controllers
{
    public class AuthController(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager) : Controller
    {

        private readonly SignInManager<UserEntity> _signInManager = signInManager;
        private readonly UserManager<UserEntity> _userManager = userManager;

        [HttpGet]
        public IActionResult Facebook()
        {
            var authProps = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", Url.Action("FacebookCallback"));
            return new ChallengeResult("Facebook", authProps);
        }



        [HttpGet]
        public async Task<IActionResult> FacebookCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info != null)
            {
                var userEntity = new UserEntity
                {
                    FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!,
                    LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!,
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)!,
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email)!,
                    IsExternalAccount = true
                };


                var user = await _userManager.FindByEmailAsync(userEntity.Email);
                if (user == null)
                {
                    var result = await _userManager.CreateAsync(userEntity);
                    if (result.Succeeded)
                    {
                        user = await _userManager.FindByEmailAsync(userEntity.Email);
                    }
                }
                if (user != null)
                {
                    if (user.FirstName != userEntity.FirstName || user.LastName != userEntity.LastName || user.Email != userEntity.Email)
                    {

                        user.FirstName = userEntity.FirstName;
                        user.LastName = userEntity.LastName;
                        user.Email = userEntity.Email;
                        user.IsExternalAccount = true;

                        await _userManager.UpdateAsync(user);
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (HttpContext.User != null)
                    {
                        return RedirectToAction("Details", "Account");
                    }
                }

            }
            ModelState.AddModelError("InvalidFacebookauthentication", "danger|Failed to authenticate with Facebook.");
            ViewData["StatusMessage"] = "danger|Failed to authenticate with Facebook.";
            return RedirectToAction("SignIn", "Home");
        }
    }
}
