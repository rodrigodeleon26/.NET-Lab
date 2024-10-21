using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DAL;
using Shared;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options2 =>
    {
        options2.SaveToken = false;
        options2.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalFunctions.GetSecretKey())),
            ValidateIssuer = false,
            ValidateAudience = false
        };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

# region Config. CORS
app.UseCors(options =>
    options.WithOrigins(GlobalFunctions.GetAllowedOrigins())
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

app.MapPost("api/auth/login", async (
    UserManager<AppUsers> userManager,
    [FromBody] UserLoginModel userLoginModel) =>
{
    var user = await userManager.FindByEmailAsync(userLoginModel.Email);
    if (user != null && await userManager.CheckPasswordAsync(user, userLoginModel.Password))
    {
        var signInKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(GlobalFunctions.GetSecretKey() ?? "DefaultSecretKey")
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(
                signInKey,
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);
        return Results.Ok(new { token });
    }
    else
    {
        return Results.BadRequest(new { message = "Usuario o contraseña incorrectos" });
    }
});

app.Run();

public class UserRegistrationModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
}

public class UserLoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}
