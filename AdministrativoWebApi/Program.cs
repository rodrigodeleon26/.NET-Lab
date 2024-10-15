using BL.BLs;
using BL.IBLs;
using DAL.DALs;
using DAL.IDALs;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    /**********************************************************/
    /** Add Dependencies                                     **/
    /**********************************************************/
    #region Inyeccion de dependencias

    // DALs
    builder.Services.AddTransient<IDAL_Administrativo, DAL_Administrativo_EF>();

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

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}