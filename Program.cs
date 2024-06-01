using DependencyInjectionAutomatic.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services;
using PRN221_Assignment.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<social_mediaContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddRazorPages();
builder.Services.AutoRegisterServices();
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });



#region Service Connect
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
