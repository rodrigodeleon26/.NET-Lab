using AuthWebApi.Extensions;
using BL.BLs;
using BL.IBLs;
using DAL;
using DAL.DALs;
using DAL.IDALs;

try 
{ 
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                builder.WithOrigins("https://localhost:5010", "https://localhost:5011", "https://localhost:5012", "http://localhost:4200", "https://localhost:443")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
    });

    // Add services to the container.

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                builder.WithOrigins("https://localhost:5010", "https://localhost:5011", "https://localhost:5012", "http://localhost:4200", "https://localhost:443")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
    });

    // Add services to the container.
    builder.Services.AddControllers();

    builder.Services.AddSwaggerExplorer()
                .InjectDBContext()
                .InjectDALandBL()
                .AddIdentityAuth();

    var app = builder.Build();

    app.ConfigureSwaggerExplorer()
   .ConfigureCORS()
   .AddIdentityAuthMiddlewares();
   //.UseHttpsRedirection();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}
