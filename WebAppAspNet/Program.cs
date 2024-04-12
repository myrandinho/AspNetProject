

using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Helpers.Middlewares;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(x => x.LowercaseUrls = true);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddDefaultIdentity<UserEntity>(x =>
{
    
    x.User.RequireUniqueEmail = true;
    x.SignIn.RequireConfirmedAccount = false;
    x.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(x =>
{
    
    x.LoginPath = "/signin";
    x.LogoutPath = "/signout";
    x.AccessDeniedPath = "/denied";

    x.Cookie.HttpOnly = true;
    x.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    x.SlidingExpiration = true;
    x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddHttpClient();

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<AdressService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<UserManager<UserEntity>>();




builder.Services.AddAuthentication().AddFacebook(x =>
{
    x.AppId = "1377132983001372";
    x.AppSecret = "0f9bc2a49df805a4cdb41e0afb798f63";
    x.Fields.Add("first_name");
    x.Fields.Add("last_name");
});


var app = builder.Build();
app.UseStatusCodePagesWithReExecute("/error", "?statusCode={0}");
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseUserSessionValidation();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
