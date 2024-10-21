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
using AuthWebApi.Extensions;
using AuthWebApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSwaggerExplorer()
                .InjectDBContext()
                .AddIdentityHandlersAndStores()
                .ConfigureIdentityOptions()
                .AddIdentityAuth();

var app = builder.Build();

app.ConfigureSwaggerExplorer()
   .ConfigureCORS()
   .AddIdentityAuthMiddlewares()
   .UseHttpsRedirection();

app.MapControllers();
// Endpoints nativos de Identity
//app.MapIdentityApi<AppUsers>();
app.MapAuthEndpoints();

app.Run();