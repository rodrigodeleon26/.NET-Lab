using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthWebApi.Controllers
{
    public static class EndpointsPrueba
    {
        public static IEndpointRouteBuilder MapPruebaEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/datosPersonales", datosPersonales);
            return app;
        }

        [Authorize(Roles = "Admin")]
        private static async Task<IResult> datosPersonales(
            ClaimsPrincipal user,
            UserManager <AppUsers> userManager)
        {
            string userID = user.Claims.First(x => x.Type == "userId").Value;
            var userDetails = await userManager.FindByIdAsync(userID);
            return Results.Ok(
                new 
                {
                    Email = userDetails?.Email,
                    FullName = userDetails?.FullName
                });
        }
    }
}
