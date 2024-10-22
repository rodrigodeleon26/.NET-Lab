using BL.IBLs;
using DAL;
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
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Documento { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public static class Auth
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/auth/register", RegisterUser);
            app.MapPost("api/auth/login", LoginUser);
            return app;
        }

        [AllowAnonymous]
        private static async Task<IResult> RegisterUser(
        UserManager<AppUsers> userManager,
        IBL_Pacientes blPacientes,
        DBContext db,
        [FromBody] UserRegistrationModel userRegistrationModel)
        {
            Paciente paciente = blPacientes.getXDocumento(userRegistrationModel.Documento);

            if (paciente == null)
            {
                paciente = new Paciente
                {
                    Nombres = userRegistrationModel.Nombres.ToUpper(),
                    Apellidos = userRegistrationModel.Apellidos.ToUpper(),
                    Documento = userRegistrationModel.Documento,
                    Email = userRegistrationModel.Email
                };
                blPacientes.addPaciente(paciente);

                //// Asegúrate de que el paciente se ha guardado correctamente y tiene un Id asignado
                //paciente = blPacientes.getXDocumento(userRegistrationModel.Documento);
                //if (paciente == null)
                //{
                //    return Results.BadRequest(new { message = "Error al guardar el paciente." });
                //}
            }
            else
            {
                AppUsers userAux = userManager.Users.FirstOrDefault(x => x.PacienteId == paciente.Id);
                if (userAux != null)
                {
                    return Results.BadRequest(new
                    {
                        code = "DuplicateDocumento",
                        description = $"El paciente con documento {userRegistrationModel.Documento} ya tiene un usuario asociado, el mismo es {userAux.UserName}"
                    });
                }
            }

            AppUsers user = new AppUsers
            {
                Email = userRegistrationModel.Email,
                UserName = userRegistrationModel.Email,
                FullName = $"{userRegistrationModel.Nombres.ToUpper()} {userRegistrationModel.Apellidos.ToUpper()}",
            };

            user.Paciente = db.Pacientes.Find(paciente.Id);
            var result = await userManager.CreateAsync(user, userRegistrationModel.Password);

            await userManager.AddToRoleAsync(user, "PACIENTE");
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
                var roles = await userManager.GetRolesAsync(user);

                var signInKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(GlobalFunctions.GetSecretKey() ?? "DefaultSecretKey")
                );

                ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim("userId", user.Id.ToString()),
                    new Claim("email", user.Email!),
                    new Claim("fullName", user.FullName),
                    new Claim(ClaimTypes.Role, roles.First()),
                });

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
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
