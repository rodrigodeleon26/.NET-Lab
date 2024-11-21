using AuthWebApi.Services;
using BL.IBLs;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared;
using Shared.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
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

    public class RefreshTokenModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class ForgotPasswordModel
    {
        public string Email { get; set; }
    }

    public class ResendConfirmationEmailModel
    {
        public string Email { get; set; }
    }

    public class ConfirmEmailModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class GenerateQrCodeModel
    {
        public string Email { get; set; }
    }

    public class TwoFactorCodeModel
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }

    public class TwoFactorAuthModel
    {
        public string Email { get; set; }
    }

    public static class Auth
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/auth/register", RegisterUser);
            app.MapPost("api/auth/login", LoginUser);
            app.MapPost("api/auth/refreshToken", RefreshToken);
            app.MapPost("api/auth/resendConfirmationEmail", ResendConfirmationEmail);
            app.MapPost("api/auth/confirmEmail", ConfirmEmail);
            app.MapPost("api/auth/forgotPassword", ForgotPassword);
            app.MapPost("api/auth/resetPassword", ResetPassword); 
            app.MapPost("api/auth/generateQrCode", GenerateQrCode); 
            app.MapPost("api/auth/validateTwoFactorCode", ValidateTwoFactorCode);
            app.MapPost("api/auth/enableTwoFactorAuth", EnableTwoFactorAuth); 
            app.MapPost("api/auth/disableTwoFactorAuth", DisableTwoFactorAuth); 
            return app;
        }

        [AllowAnonymous]
        private static async Task<IResult> RegisterUser(
        UserManager<AppUsers> userManager,
        IBL_Pacientes blPacientes,
        DBContext db,
        [FromBody] UserRegistrationModel userRegistrationModel)
        {
            //Medico medico = blAdministrativo.getMedicoByDocumento(userRegistrationModel.Documento);

            //if (medico == null)
            //{
            //    medico = new Medico
            //    {
            //        Nombres = userRegistrationModel.Nombres.ToUpper(),
            //        Apellidos = userRegistrationModel.Apellidos.ToUpper(),
            //        Documento = userRegistrationModel.Documento,
            //        Email = userRegistrationModel.Email,
            //    };
            //    blAdministrativo.addMedico(medico);
            //}

            AppUsers user = new AppUsers
            {
                Email = userRegistrationModel.Email,
                UserName = userRegistrationModel.Email,
                FullName = $"{userRegistrationModel.Nombres.ToUpper()} {userRegistrationModel.Apellidos.ToUpper()}",
            };

            //user.Paciente = db.Pacientes.Find(paciente.Id);
            //user.Medico = db.Medicos.Find(medico.Id);
            var result = await userManager.CreateAsync(user, userRegistrationModel.Password);

            await userManager.AddToRoleAsync(user, "ADMIN");
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
                var tokens = await GenerateTokens(user, userManager);
                return Results.Ok(tokens);
            }
            else
            {
                return Results.BadRequest(new { message = "Usuario o contraseña incorrectos" });
            }
        }

        [AllowAnonymous]
        private static async Task<IResult> RefreshToken(
            UserManager<AppUsers> userManager,
            [FromBody] RefreshTokenModel tokenRefreshModel)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == tokenRefreshModel.RefreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Results.Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(GlobalFunctions.GetSecretKey() ?? "DefaultSecretKey");

            try
            {
                var principal = tokenHandler.ValidateToken(tokenRefreshModel.Token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    //ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var expiration = jwtToken.ValidTo;

                var tokens = await GenerateTokens(user, userManager);
                return Results.Ok(tokens);
            }
            catch (SecurityTokenException ex)
            {
                return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                return Results.Unauthorized();
            }
        }

        [AllowAnonymous]
        private static async Task<IResult> ResendConfirmationEmail(
            UserManager<AppUsers> userManager,
            EmailService emailService,
            [FromBody] ResendConfirmationEmailModel resendConfirmationEmailModel)
        {
            var user = await userManager.FindByEmailAsync(resendConfirmationEmailModel.Email);
            if (user == null)
            {
                return Results.BadRequest(new { message = "Usuario no encontrado" });
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);
            var confirmationLink = $"http://localhost:4200/confirmEmail?&token={encodedToken}";

            var htmlMessage = $@"
            <h1>Confirmación de Email</h1>
            <p>Hola {user.FullName},</p>
            <p>Gracias por registrarte. Por favor, confirma tu email haciendo clic en el siguiente enlace:</p>
            <a href='{confirmationLink}'>Confirmar Email</a>
            <p>Si no puedes hacer clic en el enlace, copia y pega la siguiente URL en tu navegador:</p>
            <p>{confirmationLink}</p>
            <p>Saludos,</p>
            <p>El equipo de AuthWebApi</p>";

            await emailService.SendEmailAsync(user.Email, "Confirmación de Email", htmlMessage);

            return Results.Ok(new { message = "Email de confirmación enviado" });
        }

        [AllowAnonymous]
        private static async Task<IResult> ConfirmEmail(
            UserManager<AppUsers> userManager,
            [FromBody] ConfirmEmailModel confirmEmailModel)
        {
            var user = await userManager.FindByEmailAsync(confirmEmailModel.Email);
            if (user == null)
            {
                return Results.BadRequest(new { message = "Usuario no encontrado" });
            }

            var result = await userManager.ConfirmEmailAsync(user, confirmEmailModel.Token);
            if (result.Succeeded)
            {
                var tokens = await GenerateTokens(user, userManager);
                return Results.Ok(new { message = "Email confirmado exitosamente", tokens});
            }
            else
            {
                return Results.BadRequest(new { message = "Error al confirmar el email" });
            }
        }

        [AllowAnonymous]
        private static async Task<IResult> ForgotPassword(
            UserManager<AppUsers> userManager,
            EmailService emailService,
            [FromBody] ForgotPasswordModel forgotPasswordModel)
        {
            var user = await userManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user == null)
            {
                return Results.BadRequest(new { message = "Usuario no encontrado" });
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedEmail = WebUtility.UrlEncode(user.Email);
            var encodedToken = WebUtility.UrlEncode(token);
            var resetLink = $"http://localhost:4200/resetPassword?email={encodedEmail}&token={encodedToken}";

            var htmlMessage = $@"
            <h1>Restablecimiento de Contraseña</h1>
            <p>Hola {user.FullName},</p>
            <p>Has solicitado restablecer tu contraseña. Por favor, haz clic en el siguiente enlace para restablecer tu contraseña:</p>
            <a href='{resetLink}'>Restablecer Contraseña</a>
            <p>Si no puedes hacer clic en el enlace, copia y pega la siguiente URL en tu navegador:</p>
            <p>{resetLink}</p>
            <p>Saludos,</p>
            <p>El equipo de AuthWebApi</p>";

            await emailService.SendEmailAsync(user.Email, "Restablecimiento de Contraseña", htmlMessage);

            return Results.Ok(new { message = "Email de restablecimiento de contraseña enviado" });
        }

        [AllowAnonymous]
        private static async Task<IResult> ResetPassword(
            UserManager<AppUsers> userManager,
            [FromBody] ResetPasswordModel resetPasswordModel)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
            {
                return Results.BadRequest(new { message = "Usuario no encontrado" });
            }

            var result = await userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.NewPassword);
            if (result.Succeeded)
            {
                return Results.Ok(new { message = "Contraseña restablecida exitosamente" });
            }
            else
            {
                return Results.BadRequest(new { message = "Error al restablecer la contraseña", errors = result.Errors });
            }
        }

        [AllowAnonymous]
        private static async Task<IResult> GenerateQrCode(
            UserManager<AppUsers> userManager,
            TwoFactorAuthService twoFactorAuthService,
            [FromBody] GenerateQrCodeModel generateQrCodeModel)
        {
            var user = await userManager.FindByEmailAsync(generateQrCodeModel.Email);
            if (user == null)
            {
                return Results.BadRequest(new { message = "Usuario no encontrado" });
            }

            if (!user.TwoFactorEnabled)
            {
                return Results.BadRequest(new { message = "La autenticación de dos factores no está habilitada" });
            }

            var (qrCodeImageUrl, manualEntrySetupCode) = await twoFactorAuthService.GenerateQrCodeAsync(user);
            return Results.Ok(new { qrCodeImageUrl, manualEntrySetupCode });
        }

        [AllowAnonymous]
        private static IResult ValidateTwoFactorCode(
            UserManager<AppUsers> userManager,
            TwoFactorAuthService twoFactorAuthService,
            [FromBody] TwoFactorCodeModel twoFactorCodeModel)
        {
            var user = userManager.FindByEmailAsync(twoFactorCodeModel.Email).Result;
            if (user == null)
            {
                return Results.BadRequest(new { message = "Usuario no encontrado" });
            }

            if (!user.TwoFactorEnabled)
            {
                return Results.BadRequest(new { message = "La autenticación de dos factores no está habilitada" });
            }

            var isValid = twoFactorAuthService.ValidateTwoFactorCode(user, twoFactorCodeModel.Code);
            if (isValid)
            {
                return Results.Ok(new { message = "Código 2FA válido" });
            }
            else
            {
                return Results.BadRequest(new { message = "Código 2FA inválido" });
            }
        }

        [AllowAnonymous]
        private static async Task<IResult> EnableTwoFactorAuth(
        UserManager<AppUsers> userManager,
        TwoFactorAuthService twoFactorAuthService,
        [FromBody] TwoFactorAuthModel twoFactorAuthModel)
        {
            var user = await userManager.FindByEmailAsync(twoFactorAuthModel.Email);
            if (user == null)
            {
                return Results.BadRequest(new { message = "Usuario no encontrado" });
            }

            if (user.TwoFactorEnabled)
            {
                return Results.BadRequest(new { message = "La autenticación de dos factores ya está habilitada" });
            }

            user.TwoFactorEnabled = true;
            await userManager.UpdateAsync(user);

            var tokens = await GenerateTokens(user, userManager);

            return Results.Ok(new { message = "Autenticación de dos factores habilitada", tokens});
        }

        [AllowAnonymous]
        private static async Task<IResult> DisableTwoFactorAuth(
            UserManager<AppUsers> userManager,
            [FromBody] TwoFactorAuthModel twoFactorAuthModel)
        {
            var user = await userManager.FindByEmailAsync(twoFactorAuthModel.Email);
            if (user == null)
            {
                return Results.BadRequest(new { message = "Usuario no encontrado" });
            }

            if (!user.TwoFactorEnabled)
            {
                return Results.BadRequest(new { message = "La autenticación de dos factores no está habilitada" });
            }

            user.TwoFactorAuthKey = null;
            user.TwoFactorEnabled = false;
            await userManager.UpdateAsync(user);

            var tokens = await GenerateTokens(user, userManager);

            return Results.Ok(new { message = "Autenticación de dos factores desactivada", tokens });
        }

        private static async Task<object> GenerateTokens(AppUsers user, UserManager<AppUsers> userManager)
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
                new Claim("emailConfirmed", user.EmailConfirmed.ToString()),
                new Claim("TwoFactorEnabled", user.TwoFactorEnabled.ToString())
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

            // Generar el refresh token
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await userManager.UpdateAsync(user);

            return new { token, refreshToken };
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
