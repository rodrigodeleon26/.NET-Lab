using AdministrativoWebApi;
using BL.BLs;
using BL.IBLs;
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
                builder.WithOrigins("https://localhost:5010", "https://localhost:5011", "https://localhost:5012", "http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
    });

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    /**********************************************************/
    /** Add Dependencies                                     **/
    /**********************************************************/
    #region Inyeccion de dependencias

    // DALs
    builder.Services.AddScoped<IDAL_Administrativo, DAL_Administrativo_EF>();

    // Registrar las dependencias necesarias
    builder.Services.AddScoped<IDAL_Pacientes, DAL_Paciente_Service>();
    builder.Services.AddHttpClient<IDAL_Pacientes, DAL_Paciente_Service>();

    // BLs
    builder.Services.AddTransient<IBL_Administrativo, BL_Administrativo>();

    #endregion

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseHttpsRedirection();

    app.UseCors("AllowSpecificOrigin");

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}