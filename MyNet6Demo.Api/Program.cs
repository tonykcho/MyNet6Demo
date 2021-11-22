using Serilog;
using MyNet6Demo.Infrastructure.DbContexts;
using MyNet6Demo.Api.Filters;
using MyNet6Demo.Api.Extensions;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Infrastructure.Repositories;
using MyNet6Demo.Core.BackgroundServices;
using FluentValidation.AspNetCore;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyNet6Demo.Api.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MyNet6Demo.Core.Services;

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

builder.Services.AddAutoMapper(Assembly.Load("MyNet6Demo.Core"));

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
builder.Services.AddScoped<ISongRepository, SongRepository>();

builder.Services.AddScoped<ICsvFileBuilder, CsvFileBuilder>();

builder.Services.AddHostedService<SomeBackgroundService>();

builder.Services.AddHealthChecks()
    .Add(new HealthCheckRegistration("Mysql", sp => new MySqlHealthCheck(sp.GetRequiredService<AppDbContext>()), default, default));

var app = builder.Build();

await app.MigrateSchemaAsync();

// app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapHealthChecks("api/health_checks", new HealthCheckOptions()
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            Status = report.Status.ToString(),
            Checks = report.Entries.Select(x => new
            {
                Component = x.Key,
                Status = x.Value.Status.ToString(),
                Description = x.Value.Description
            })
        });
    }
});

app.MapControllers();

app.Run();