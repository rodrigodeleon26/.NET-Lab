using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DAL;
using Shared;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddIdentityApiEndpoints<AppUsers>()
    .AddEntityFrameworkStores<DBContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Número máximo de intentos de inicio de sesión fallidos
    options.Lockout.MaxFailedAccessAttempts = 5;
    // Tiempo que la cuenta queda bloqueada después de alcanzar el número máximo de intentos de inicio de sesión fallidos
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    // Requerimientos de la password
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;
    // Requerimientos del usuario
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(GlobalFunctions.GetConnectionString()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

# region Condig. CORS
app.UseCors(options =>
    options.WithOrigins("https://localhost:5010", "https://localhost:5011", "https://localhost:5012", "http://localhost:4200")  
           .AllowAnyHeader()
           .AllowAnyMethod());
# endregion

app.UseAuthorization();

app.MapControllers();

// Endpoints nativos de Identity
app.MapIdentityApi<AppUsers>();

app.MapPost("api/auth/register", async (
    UserManager<AppUsers> userManager,
    [FromBody] UserRegistrationModel userRegistrationModel
    ) =>
{
    AppUsers user = new AppUsers
    {
        Email = userRegistrationModel.Email,
        UserName = userRegistrationModel.Email,
        FullName = userRegistrationModel.FullName
    };
    var result = await userManager.CreateAsync(
        user, 
        userRegistrationModel.Password);

    if (result.Succeeded)
        return Results.Ok(result);
    else
        return Results.BadRequest(result.Errors);
});

app.Run();

public class UserRegistrationModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
}
