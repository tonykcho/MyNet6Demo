using Serilog;

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
}).AddCookie(options =>
{
    options.Cookie.Name = "MyNet6CoreDemo.Cookie";
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapControllers();

app.Run();