using RtspViewer.Services;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Настройка логирования
var logFileName = $"logs/{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log";
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File(logFileName, rollingInterval: RollingInterval.Infinite)
    .CreateLogger();

builder.Host.UseSerilog();

// Добавляем сервисы
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<RtspStreamService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Video}/{action=Index}/{id?}");

app.Run();