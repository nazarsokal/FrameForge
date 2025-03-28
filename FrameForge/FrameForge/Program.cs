using Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<FrameForgeDbContext>(options =>
{
    var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Database connection string is missing!");
    }

    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

if(builder.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();