using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthWebApi.Controllers
{
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

    public static class Auth
    {
        public static IEndpointRouteBuilder MapAuthEndpoints (this IEndpointRouteBuilder app)
        {
            app.MapPost("api/auth/register", RegisterUser);

            app.MapPost("api/auth/login", LoginUser); 
            return app;
        }

        [AllowAnonymous]
        private static async Task<IResult> RegisterUser(UserManager<AppUsers> userManager,
            [FromBody] UserRegistrationModel userRegistrationModel)
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
        }

        [AllowAnonymous]
        private static async Task<IResult> LoginUser(
            UserManager<AppUsers> userManager,
                [FromBody] UserLoginModel userLoginModel)
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
        }
    }
}
