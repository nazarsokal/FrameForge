using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using Services.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<LeaderboardService>();
builder.Services.AddScoped<TestsServise>();
builder.Services.AddSignalR();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<BattleSingleton>();

builder.WebHost.UseUrls("http://*:5118", "https://*:7287");

builder.Services.AddScoped<IProgressMapService, ProgressMapService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IGoogleOAuthService, GoogleOAuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAzureStorageService, AzureStorageService>();
builder.Services.AddScoped<IBattleService, BattleService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IAlgorithmsService, AlgorithmsService>();


builder.Services.AddDbContext<FrameForgeDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Database connection string is missing!");
    }

    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Debug);
});
var app = builder.Build();

if(builder.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.MapHub<BattleHub>("/battleHub");
app.UseSession();

app.Run();