using Autowriter;
using Autowriter.Database;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<RazorViewEngineOptions>(options =>
    options.ViewLocationExpanders.Add(new FeatureViewLocationExpander()));

builder.Services.AddSingleton<IDbBootstrapper, DbBootstrapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Services.GetService<IDbBootstrapper>().Bootstrap();

app.Run();
