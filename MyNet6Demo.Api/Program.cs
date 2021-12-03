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
using MyNet6Demo.Domain.DomainEvents;
using MyNet6Demo.Core.Interfaces;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

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

builder.Services.AddScoped<IArtistRepository, ArtistRepository>();

builder.Services.AddScoped<ICsvFileBuilder, CsvFileBuilder>();

builder.Services.AddSingleton<IRabbitMQConnectionManager, RabbitMQConnectionManager>();

builder.Services.AddSingleton<IMessageBusClient, RabbitMQMessageBusClient>();
// builder.Services.AddHostedService<SomeBackgroundService>();

builder.Services.AddHealthChecks()
    .Add(new HealthCheckRegistration("Mysql", sp => new MySqlHealthCheck(sp.GetRequiredService<AppDbContext>()), default, default))
    .Add(new HealthCheckRegistration("RabbitMQ", sp => new RabbitMQHealthCheck(sp.GetRequiredService<IRabbitMQConnectionManager>()), default, default));

builder.Services.AddSingleton<IDomainEventProcessor, DomainEventProcessor>((sp) =>
{
    var processor = new DomainEventProcessor(sp);

    processor.Subscribe<AlbumCreatedEvent, AlbumCreatedEventHandler>();

    processor.Subscribe<AlbumUpdatedEvent, AlbumUpdatedEventHandler>();

    processor.Subscribe<ArtistCreatedEvent, ArtistCreatedEventHandler>();

    processor.Subscribe<ArtistUpdatedEvent, ArtistUpdatedEventHandler>();

    return processor;
});

builder.Services.AddTransient<AlbumCreatedEventHandler>();

builder.Services.AddTransient<AlbumUpdatedEventHandler>();

builder.Services.AddTransient<ArtistCreatedEventHandler>();

builder.Services.AddTransient<ArtistUpdatedEventHandler>();

builder.Services.AddHostedService<RabbitMQMessageBusSubscriber>();

if (Boolean.Parse(builder.Configuration["FirebaseMessagingOn"]) == true)
{
    Log.Information("--> Firebase Messaging On");
    if (FirebaseApp.DefaultInstance is null)
    {
        FirebaseApp.Create(new AppOptions()
        {
            // Please Set up GOOGLE_APPLICATION_CREDENTIALS Env
            // Credential = GoogleCredential.FromFile("firebase_credential.json")
            Credential = GoogleCredential.FromFile(builder.Configuration["FirebaseCredential"])
        });
    }

    builder.Services.AddSingleton<IFirebaseMessagingService, FirebaseMessagingService>();
    Log.Information("--> Firebase Messaging Registration Success");
}
else
{
    builder.Services.AddSingleton<IFirebaseMessagingService, FirebaseMessagingMockService>();
}


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