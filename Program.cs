using DotNetEnv;
using Microsoft.Extensions.FileProviders;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using StackExchange.Redis;

using Solara.Data;
using Solara.Services;


var builder = WebApplication.CreateBuilder(args);


// Load environment variables from the .env file
Env.Load();

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPass = Environment.GetEnvironmentVariable("DB_PASS");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");

var connectionString = $"server={dbHost};port=3306;database={dbName};user={dbUser};password={dbPass}";

var clientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
var clientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");

var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "/";

var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST") ?? "localhost";
var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT") ?? "6379";


// Configure services
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 23))));

builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = clientId!;            // TODO: handle cases where env is not set 
    options.ClientSecret = clientSecret!;
    options.CallbackPath = "/signin-google";
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", builder =>
    {
        builder.WithOrigins(frontendUrl)
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect($"{redisHost}:{redisPort}"));
builder.Services.AddHostedService<GameTickService>();
builder.Services.AddSingleton<UserSocketManager>();
builder.Services.AddSingleton<RedisCacheService>();


var app = builder.Build();

// TODO : look into Swagger/OpenAPI


// Set up middleware
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// TODO: temp
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseWebSockets();
app.UseRouting();
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
