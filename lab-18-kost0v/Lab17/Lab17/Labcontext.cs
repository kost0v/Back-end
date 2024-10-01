using Laba17;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;
var builder = WebApplication.CreateBuilder(args);
// Настройка подключения к базе данных MySQL
var connectionString =
"Server=127.0.0.1;Port=3306;Database=exampleshema;User=root;Password=pugpug12;";
builder.Services.AddDbContext<LABAcontext>(options =>
 options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0,
36))));
// Настройка кэширования
builder.Services.AddMemoryCache(); // Внутренний кэш и кэш памяти
builder.Services.AddDistributedMemoryCache(); // Использование распределенного кэша в
builder.Services.AddControllersWithViews(); // Регистрация контроллеров и

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching(); // Использование кэширования ответов
// Настройка Prometheus для сбора метрик
app.UseMetricServer();
app.UseHttpMetrics();
// Настройка маршрутизации
app.UseRouting();
// Определение маршрута по умолчанию для контроллеров
app.MapControllerRoute(
 name: "default",
 pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseStaticFiles();
app.Run();
