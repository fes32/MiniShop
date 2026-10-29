
using MiniShop.Web.Services;
using Microsoft.EntityFrameworkCore;
using MiniShop.Application.Interfaces;
using MiniShop.Application.Services;
using MiniShop.Infrastructure.Persistence;
using MiniShop.Infrastructure.Repositories;
using MiniShop.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Контроллеры API из сборки MiniShop.Api
builder.Services.AddControllers()
    .AddApplicationPart(typeof(MiniShop.Api.Controllers.ProductsController).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// База данных SQLite в папке LocalAppData пользователя
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        $"Data Source={DatabasePath.GetDatabasePath()}"));

// Сервисы и репозитории
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<CartService>();

// Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();

// Swagger доступен в режиме разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseAntiforgery();

// Маршруты контроллеров API
app.MapControllers();

// Регистрация Blazor — только один раз
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

var url = "http://localhost:5000";

app.Lifetime.ApplicationStarted.Register(() =>
{
    try
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Не удалось открыть браузер: {ex.Message}");
    }
});

app.Run();

