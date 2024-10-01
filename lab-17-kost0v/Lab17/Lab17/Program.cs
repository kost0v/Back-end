using Lab17;
var builder = WebApplication.CreateBuilder(args);
var connectionString =
"Server=127.0.0.1;Port=3306;Database=exampleshema;User=root;Password=pugpug12;";
builder.Services.AddDbContext<Labcontext>(options =>
 options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0,
36))));
builder.Services.AddMemoryCache(); 
builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddControllersWithViews(); 
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
app.UseResponseCaching(); 
app.UseRouting();
app.MapControllerRoute(
 name: "default",
 pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseStaticFiles();
app.Run();