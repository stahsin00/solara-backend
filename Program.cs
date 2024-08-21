using DotNetEnv;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

using Solara.Data;


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


var app = builder.Build();

// TODO : look into Swagger/OpenAPI


// Set up middleware
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
