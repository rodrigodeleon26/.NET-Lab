using BL.IBLs;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthWebApi.Controllers
{
    public class UserRegistrationWithRolesModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public static class EndpointsPrueba
    {
        public static IEndpointRouteBuilder MapPruebaEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/prueba/register", RegisterUserWithRoles);
            return app;
        }

        [AllowAnonymous]
        private static async Task<IResult> RegisterUserWithRoles(
        UserManager<AppUsers> userManager,
        [FromBody] UserRegistrationWithRolesModel UserRegistrationWithRolesModel)
        {
            AppUsers user = new AppUsers
            {
                Email = UserRegistrationWithRolesModel.Email,
                UserName = UserRegistrationWithRolesModel.Email,
                FullName = UserRegistrationWithRolesModel.Email,
            };

            var result = await userManager.CreateAsync(user, UserRegistrationWithRolesModel.Password);

            await userManager.AddToRoleAsync(user, UserRegistrationWithRolesModel.Role);
            if (result.Succeeded)
                return Results.Ok(result);
            else
                return Results.BadRequest(result.Errors);
        }
    }
}
