using Autowriter.Database;
using MediatR;
using Microsoft.Data.Sqlite;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddSingleton<IDbBootstrapper, DbBootstrapper>();
builder.Services.AddSingleton<IDataRepository, DataRepository>();
builder.Services.AddSingleton<IDbConnection>((serviceProvider) =>
    new SqliteConnection(builder.Configuration.GetSection("DatabaseName").Value));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.UseStatusCodePages();
app.Services.GetService<IDbBootstrapper>().Bootstrap();

app.Run();
