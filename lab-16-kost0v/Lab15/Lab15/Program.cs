using Lab15.Middleware;
using Microsoft.AspNetCore.Rewrite;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication("CookieAuth")
 .AddCookie("CookieAuth", options =>
 {
     options.Cookie.Name = "UserLoginCookie";
     options.LoginPath = "/Account/Login";
     options.AccessDeniedPath = "/Account/AccessDenied";
 });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});
var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
var rewriteOptions = new RewriteOptions()
 .AddRedirect("^Admin$", "Admin/Index")
 .AddRedirect("^User$", "User/Index");
app.UseRewriter(rewriteOptions);
app.UseRoleCheck();
app.MapControllerRoute(
 name: "default",
 pattern: "{controller=Account}/{action=Login}/{id?}");
app.Run();