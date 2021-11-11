using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyNet6Demo.Infrastructure.DbContexts;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log_.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "MyNet6CoreDemo";
    options.DefaultAuthenticateScheme = "MyNet6CoreDemo";
    options.DefaultChallengeScheme = "MyNet6CoreDemo";
}).AddCookie("MyNet6CoreDemo", options =>
{
    options.Cookie.Name = "MyNet6CoreDemo.Cookie";
    options.LoginPath = "/";
});

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

// app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapControllers();

app.Run();