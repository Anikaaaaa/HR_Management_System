using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HR_System_Assignment.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<HR_System_AssignmentContext>(options =>
  //  options.UseSqlServer(builder.Configuration.GetConnectionString("HR_System_AssignmentContext") ?? throw new InvalidOperationException("Connection string 'HR_System_AssignmentContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.LoginPath = "/UserLogin/Login";
        //options.SlidingExpiration = true;
        //options.AccessDeniedPath = "/UserLogin/Login";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserLogin}/{action=Login}/{id?}");

app.Run();
