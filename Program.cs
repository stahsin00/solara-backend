using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Solara.Data;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from the .env file
Env.Load();

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPass = Environment.GetEnvironmentVariable("DB_PASS");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");

var connectionString = $"server={dbHost};port=3306;database={dbName};user={dbUser};password={dbPass}";

builder.Services.AddDbContext<CharacterContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 23))));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
