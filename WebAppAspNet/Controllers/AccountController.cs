using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebAppAspNet.ViewModels;
using WebAppAspNet.ViewModels.Account;

namespace WebAppAspNet.Controllers;

public class AccountController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, AdressService adressService, HttpClient http, IConfiguration configuration, AccountService accountService, CategoryService categoryService, CourseService courseService) : Controller
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly AdressService _adressService = adressService;
    private readonly HttpClient _http = http;
    private readonly IConfiguration _configuration = configuration;
    private readonly AccountService _accountService = accountService;
    private readonly CategoryService _categoryService = categoryService;
    private readonly CourseService _courseService = courseService;




    [HttpGet]
    [Route("/signup")]
    public IActionResult SignUp()
    {
        if (_signInManager.IsSignedIn(User))
            return RedirectToAction("Details", "Account");

        return View();
    }

    [HttpPost]
    [Route("/signup")]
    public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var exists = await _userManager.Users.AnyAsync(x => x.Email == viewModel.Email);
            if (exists)
            {
                ModelState.AddModelError("AlreadyExists", "User with the same email adress already exists.");
                ViewData["ErrorMessage"] = "User with the same email adress already exists.";
                return View();
            }

            var userEntity = new UserEntity
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Email = viewModel.Email,
                UserName = viewModel.Email
            };

            var result = await _userManager.CreateAsync(userEntity, viewModel.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("SignIn", "Account");
            }
        }
        return View(viewModel);
    }


    [HttpGet]
    [Route("/signin")]
    public IActionResult SignIn(string returnUrl)
    {
        if (_signInManager.IsSignedIn(User))
            return RedirectToAction("Details", "Account");


        ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
        return View();

    }

    //skapar en acesstoken vid inlogging.
    [HttpPost]
    [Route("/signin")]
    public async Task<IActionResult> SignIn(SignInViewModel viewModel, string returnUrl)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    var content = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");
                    var response = await _http.PostAsync($"https://localhost:7127/api/Auth/token?key={_configuration["ApiKey"]}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var token = await response.Content.ReadAsStringAsync();
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            Expires = DateTime.Now.AddDays(1)
                        };

                        Response.Cookies.Append("AccessToken", token, cookieOptions);
                    }

                    return Redirect(returnUrl);
                }


                    



                //var login = new Dictionary<string, string>()
                //{
                //    { "email", viewModel.Email }, { "password", viewModel.Password }
                //};

               


                return RedirectToAction("Details", "Account");
            }

        }

        //ModelState.AddModelError("IncorrectValues", "Incorrect email or password");
        //ViewData["ErrorMessage"] = "Incorrect email or password";
        return View(viewModel);
    }

    [HttpGet]
    [Route("/signout")]
    public new async Task<IActionResult> SignOut()
    {
        Response.Cookies.Delete("AccessToken"); //raderar token-cookien
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


    [HttpPost]
    public async Task<IActionResult> DeleteUser(string id)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(id);


            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found";
                return View();
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }


                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return RedirectToAction("Index", "Home");
            }


        }

        return View();
    }



    //[Authorize]
    //[HttpGet]
    //[Route("/courses")]
    //public async Task<IActionResult> Courses()
    //{

    //    if (HttpContext.Request.Cookies.TryGetValue("AccessToken", out var token))
    //    {

    //        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); //Om accesstoken finns. Körs authentisering.

    //        var response = await _http.GetAsync($"https://localhost:7127/api/Courses?key={_configuration["ApiKey"]}");
    //        if (response.IsSuccessStatusCode)
    //        {

    //            var courses = JsonConvert.DeserializeObject<IEnumerable<Course>>(await response.Content.ReadAsStringAsync());
    //            return View(courses);
    //        }
    //    }

    //    return View();
    //}









    [Route("/courses")]
    [HttpGet]
    public async Task<IActionResult> Courses(string category = "", string searchQuery = "", int pageNumber = 1, int pageSize = 3)
    {

        var courseResult = await _courseService.GetCoursesAsync(category, searchQuery, pageNumber, pageSize);

        var viewModel = new CourseIndexViewModel
        {
            Categories = await _categoryService.GetCategoriesAsync(),
            Courses = courseResult.Courses,
            Pagination = new Pagination
            {
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = courseResult.TotalPages,
                TotalItems = courseResult.TotalItems
            }
        };

        return View(viewModel);
    }












    [Authorize]
    [HttpGet]
    [Route("/courses/singlecourse")]
    public async Task<IActionResult> SingleCourse(int id)
    {

        if (HttpContext.Request.Cookies.TryGetValue("AccessToken", out var token))
        {

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); //Om accesstoken finns. Körs authentisering.

            var response = await _http.GetAsync($"https://localhost:7127/api/Courses/{id}?key={_configuration["ApiKey"]}");
            if (response.IsSuccessStatusCode)
            {

                var course = JsonConvert.DeserializeObject<Course>(await response.Content.ReadAsStringAsync());
                return View(course);
            }
        }

        return View();
    }


    //[HttpPost]
    //public async Task<IActionResult> Join(int id)
    //{
    //    if (HttpContext.Request.Cookies.TryGetValue("AccessToken", out var token))
    //    {

    //        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); //Om accesstoken finns. Körs authentisering.

    //        var response = await _http.GetAsync($"https://localhost:7127/api/Courses/{id}?key={_configuration["ApiKey"]}");
    //        if (response.IsSuccessStatusCode)
    //        {

    //            var course = JsonConvert.DeserializeObject<Course>(await response.Content.ReadAsStringAsync());

    //            var result = await _accountService.SaveCourseToUser(course, userId);
    //        }
    //    }

    //    return View();
    //}

    //[Authorize]
    //[HttpGet]
    //[Route("/courses/singlecourse")]
    //public IActionResult SingleCourse()
    //{
    //    return View();
    //}








    [Authorize]
    [HttpGet]
    [Route("/account/savedcourses")]
    public async Task<IActionResult> SavedCourses()
    {
        var viewModel = new AccountDetailsViewModel();
        viewModel.ProfileInfo = await PopulateProfileInfoAsync();
        viewModel.BasicInfoForm ??= await PopulateBasicInfoAsync();
        viewModel.AdressInfoForm ??= await PopulateAdressInfoAsync();

        return View(viewModel);
    }

    [HttpGet]
    [Route("/account/newpassword")]
    public IActionResult PasswordUpdated()
    {
        return View();
    }

    [Authorize]
    [HttpGet]
    [Route("/account/security")]
    public async Task<IActionResult> Security()
    {
        var viewModel = new AccountDetailsViewModel();
        viewModel.ProfileInfo = await PopulateProfileInfoAsync();
        viewModel.BasicInfoForm ??= await PopulateBasicInfoAsync();
        viewModel.AdressInfoForm ??= await PopulateAdressInfoAsync();

        return View(viewModel);
    }

    [HttpPost]
    [Route("/account/security")]
    public async Task<IActionResult> NewPassword(AccountDetailsViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View(viewModel);
            }

            var result = await _userManager.ChangePasswordAsync(user, viewModel.SecurityForm.CurrentPassword, viewModel.SecurityForm.NewPassword);


            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction("Index", "Home");

        }


        return RedirectToAction("Security", "Account", viewModel);

    }





    [Authorize]
    [HttpGet]
    [Route("/account/details")]
    public async Task<IActionResult> Details()
    {
        var viewModel = new AccountDetailsViewModel();
        viewModel.ProfileInfo = await PopulateProfileInfoAsync();
        viewModel.BasicInfoForm ??= await PopulateBasicInfoAsync();
        viewModel.AdressInfoForm ??= await PopulateAdressInfoAsync();

        return View(viewModel);
    }







    [Authorize]
    [HttpPost]
    [Route("/account/details")]
    public async Task<IActionResult> Details(AccountDetailsViewModel viewModel)
    {



        if (viewModel.BasicInfoForm != null)
        {

            if (viewModel.BasicInfoForm.FirstName != null && viewModel.BasicInfoForm.LastName != null && viewModel.BasicInfoForm.Email != null)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    user.FirstName = viewModel.BasicInfoForm.FirstName;
                    user.LastName = viewModel.BasicInfoForm.LastName;
                    user.Email = viewModel.BasicInfoForm.Email;
                    user.PhoneNumber = viewModel.BasicInfoForm.PhoneNumber;
                    user.Bio = viewModel.BasicInfoForm.Biography;
                    user.UserName = viewModel.BasicInfoForm.Email;

                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("IncorrectValues", "Something went wrong! Unable to save data.");
                        ViewData["ErrorMessage"] = "Something went wrong! Unable to update basic information.";
                    }
                }
            }
        }



        if (viewModel.AdressInfoForm != null)
        {

            if (viewModel.AdressInfoForm.AdressLine_1 != null &&  viewModel.AdressInfoForm.PostalCode != null && viewModel.AdressInfoForm.City != null)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {

                    var adress = await _adressService.GetAdressAsync(user.Id);
                    if (adress != null)
                    {
                        adress.AdressLine_1 = viewModel.AdressInfoForm.AdressLine_1;
                        adress.AdressLine_2 = viewModel.AdressInfoForm.AdressLine_2;
                        adress.PostalCode = viewModel.AdressInfoForm.PostalCode;
                        adress.City = viewModel.AdressInfoForm.City;


                        var result = await _adressService.UpdateAdressAsync(adress);
                        if (!result)
                        {
                            ModelState.AddModelError("IncorrectValues", "Something went wrong! Unable to update adress information.");
                            ViewData["ErrorMessage"] = "Something went wrong! Unable to update adress information.";
                        }
                    }
                    else
                    {
                        adress = new AdressEntity
                        {
                            UserId = user.Id,
                            AdressLine_1 = viewModel.AdressInfoForm.AdressLine_1,
                            AdressLine_2 = viewModel.AdressInfoForm.AdressLine_2,
                            PostalCode = viewModel.AdressInfoForm.PostalCode,
                            City = viewModel.AdressInfoForm.City
                        };

                        var result = await _adressService.CreateAdressAsync(adress);
                        if (!result)
                        {
                            ModelState.AddModelError("IncorrectValues", "Something went wrong! Unable to update adress information.");
                            ViewData["ErrorMessage"] = "Something went wrong! Unable to update adress information.";
                        }
                    }
                }
            }
        }


        viewModel.ProfileInfo = await PopulateProfileInfoAsync();
        viewModel.BasicInfoForm ??= await PopulateBasicInfoAsync();
        viewModel.AdressInfoForm ??= await PopulateAdressInfoAsync();

        return View(viewModel);
    }




    [Route("/account/detailstest")]
    public IActionResult DetailsTest()
    {
        return View();
    }




    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {

        var result = await _accountService.UploadUserProfileImageAsync(User, file);
        return RedirectToAction("Details", "Account");
    }










    private async Task<ProfileInfoViewModel> PopulateProfileInfoAsync()
    {
        var user = await _userManager.GetUserAsync(User);

        return new ProfileInfoViewModel
        {

            FirstName = user!.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            IsExternalAccount = user.IsExternalAccount,


        };
    }

    private async Task<BasicInfoFormViewModel> PopulateBasicInfoAsync()
    {
        var user = await _userManager.GetUserAsync(User);

        return new BasicInfoFormViewModel
        {
            UserId = user!.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber,
            Biography = user.Bio,
        };
    }



    private async Task<AdressInfoFormViewModel> PopulateAdressInfoAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var adress = await _adressService.GetAdressAsync(user.Id);

            if (adress != null)
            {
                return new AdressInfoFormViewModel
                {
                    AdressLine_1 = adress.AdressLine_1,
                    AdressLine_2 = adress.AdressLine_2,
                    PostalCode = adress.PostalCode,
                    City = adress.City,
                };
            }

        }


        return new AdressInfoFormViewModel();

    }























    //[HttpPost]
    //public IActionResult SaveBasicInfo(AccountDetailsViewModel viewModel)
    //{
    //    if (TryValidateModel(viewModel.BasicInfoForm))
    //    {
    //        return RedirectToAction("Home", "Index");
    //    }

    //    return View("Details", viewModel);

    //}


    //[HttpPost]
    //public IActionResult SaveAdressInfo(AccountDetailsViewModel viewModel)
    //{
    //    if (TryValidateModel(viewModel.AdressInfoForm))
    //    {
    //        return RedirectToAction("Home", "Index");
    //    }

    //    return View("Details", viewModel);

    //}
}
