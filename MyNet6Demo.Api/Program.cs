using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyNet6Demo.Infrastructure.DbContexts;
using MyNet6Demo.Api.Filters;
using MyNet6Demo.Api.Extensions;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Infrastructure.Repositories;
using MyNet6Demo.Core.BackgroundServices;
using FluentValidation.AspNetCore;
using System.Reflection;
using MediatR;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log_.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ModelStateActionFilter>();
    options.Filters.Add<HttpGlobalExceptionFilter>();
}).AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssembly(Assembly.Load("MyNet6Demo.Core")));

builder.Services.AddMediatR(Assembly.Load("MyNet6Demo.Core"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "MyNet6CoreDemo";
    options.DefaultAuthenticateScheme = "MyNet6CoreDemo";
    options.DefaultChallengeScheme = "MyNet6CoreDemo";
}).AddCookie("MyNet6CoreDemo", options =>
{
    options.Cookie.Name = "MyNet6CoreDemo.Cookie";
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();

builder.Services.AddHostedService<SomeBackgroundService>();

var app = builder.Build();

await app.MigrateSchemaAsync();

// app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapControllers();

app.Run();